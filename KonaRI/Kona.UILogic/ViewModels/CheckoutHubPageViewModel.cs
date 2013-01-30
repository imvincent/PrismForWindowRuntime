// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Kona.Infrastructure;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Kona.UILogic.Services;
using System.Net.Http;
using Windows.UI.Popups;
using System.Globalization;
using Windows.UI.ViewManagement;

namespace Kona.UILogic.ViewModels
{
    public class CheckoutHubPageViewModel : ViewModel, INavigationAware, IHandleWindowSizeChanged
    {
        private readonly INavigationService _navigationService;
        private readonly IAccountService _accountService;
        private readonly IOrderService _orderService;
        private IShippingMethodService _shippingMethodService;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IShippingAddressUserControlViewModel _shippingAddressViewModel;
        private readonly IBillingAddressUserControlViewModel _billingAddressViewModel;
        private readonly IPaymentMethodUserControlViewModel _paymentMethodViewModel;
        private readonly ISettingsCharmService _settingsCharmService;
        private bool _useSameAddressAsShipping;
        private bool _isShippingInfoInvalid;
        private bool _isBillingInfoInvalid;
        private bool _isPaymentInfoInvalid;
        private bool _isUnsnapping;

        public CheckoutHubPageViewModel(INavigationService navigationService, IAccountService accountService, IOrderService orderService, ShippingMethodServiceProxy shippingMethodService, IShoppingCartRepository shoppingCartRepository,
                                        IShippingAddressUserControlViewModel shippingAddressViewModel, IBillingAddressUserControlViewModel billingAddressViewModel, IPaymentMethodUserControlViewModel paymentMethodViewModel, ISettingsCharmService settingsCharmService)
        {
            _navigationService = navigationService;
            _accountService = accountService;
            _orderService = orderService;
            _shippingMethodService = shippingMethodService;
            _shoppingCartRepository = shoppingCartRepository;

            _shippingAddressViewModel = shippingAddressViewModel;
            _billingAddressViewModel = billingAddressViewModel;
            _paymentMethodViewModel = paymentMethodViewModel;
            _settingsCharmService = settingsCharmService;

            GoBackCommand = new DelegateCommand(() => navigationService.GoBack(), () => navigationService.CanGoBack());
            GoNextCommand = new DelegateCommand(() => GoNext());
        }

        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand GoNextCommand { get; private set; }

        public IShippingAddressUserControlViewModel ShippingAddressViewModel { get { return _shippingAddressViewModel; } }
        public IBillingAddressUserControlViewModel BillingAddressViewModel { get { return _billingAddressViewModel; } }
        public IPaymentMethodUserControlViewModel PaymentMethodViewModel { get { return _paymentMethodViewModel; } }

        [RestorableState]
        public bool UseSameAddressAsShipping
        {
            get { return _useSameAddressAsShipping; }
            set
            {
                SetProperty(ref _useSameAddressAsShipping, value);

                if (!_useSameAddressAsShipping)
                {
                    // Clean the Billing Address values & errors
                    BillingAddressViewModel.Address = new Address { Id = Guid.NewGuid().ToString() };
                }

                BillingAddressViewModel.IsEnabled = !value;
            }
        }

        [RestorableState]
        public bool IsShippingInfoInvalid
        {
            get { return _isShippingInfoInvalid; }
            set { SetProperty(ref _isShippingInfoInvalid, value); }
        }

        [RestorableState]
        public bool IsBillingInfoInvalid
        {
            get { return _isBillingInfoInvalid; }
            set { SetProperty(ref _isBillingInfoInvalid, value); }
        }

        [RestorableState]
        public bool IsPaymentInfoInvalid
        {
            get { return _isPaymentInfoInvalid; }
            set { SetProperty(ref _isPaymentInfoInvalid, value); }
        }

        public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, System.Collections.Generic.Dictionary<string, object> viewState)
        {
            ShippingAddressViewModel.OnNavigatedTo(navigationParameter, navigationMode, viewState);
            BillingAddressViewModel.OnNavigatedTo(navigationParameter, navigationMode, viewState);
            PaymentMethodViewModel.OnNavigatedTo(navigationParameter, navigationMode, viewState);
            base.OnNavigatedTo(navigationParameter, navigationMode, viewState);
        }

        public override void OnNavigatedFrom(System.Collections.Generic.Dictionary<string, object> viewState, bool suspending)
        {
            ShippingAddressViewModel.OnNavigatedFrom(viewState, suspending);
            BillingAddressViewModel.OnNavigatedFrom(viewState, suspending);
            PaymentMethodViewModel.OnNavigatedFrom(viewState, suspending);
            base.OnNavigatedFrom(viewState, suspending);
        }

        private async void GoNext()
        {
            IsShippingInfoInvalid = ShippingAddressViewModel.ValidateForm() == false;
            IsBillingInfoInvalid = UseSameAddressAsShipping ? false : BillingAddressViewModel.ValidateForm() == false;
            IsPaymentInfoInvalid = PaymentMethodViewModel.ValidateForm() == false;

            if (!IsShippingInfoInvalid && !IsBillingInfoInvalid && !IsPaymentInfoInvalid)
            {
                if (await _accountService.GetSignedInUserAsync() == null)
                {
                    if (ApplicationView.Value == ApplicationViewState.Snapped)
                    {
                        _isUnsnapping = true;
                        ApplicationView.TryUnsnap();
                    }
                    _settingsCharmService.ShowFlyout("signIn", null, successAction: async () => await ProcessFormAsync());
                }
                else
                {
                    await ProcessFormAsync();
                }
            }
        }

        private async Task ProcessFormAsync()
        {
            // Create an order with the values entered in the form 
            Order order = new Order
                {
                    UserId = (await _accountService.GetSignedInUserAsync()).UserName,
                    ShippingAddress = ShippingAddressViewModel.Address,
                    BillingAddress = UseSameAddressAsShipping ? ShippingAddressViewModel.Address : BillingAddressViewModel.Address,
                    PaymentInfo = PaymentMethodViewModel.PaymentInfo,
                    ShippingMethod = await _shippingMethodService.GetBasicShippingMethodAsync(),
                    ShoppingCart = await _shoppingCartRepository.GetShoppingCartAsync()
                };

            try
            {
                // Call the service to create the order
                Order createdOrder = await _orderService.CreateOrderAsync(order, _accountService.ServerCookieHeader);

                // If everything is OK, process the form information
                if (UseSameAddressAsShipping)
                {
                    BillingAddressViewModel.Address = ShippingAddressViewModel.Address;
                }

                ShippingAddressViewModel.ProcessForm();
                BillingAddressViewModel.ProcessForm();
                PaymentMethodViewModel.ProcessForm();

                _navigationService.Navigate("CheckoutSummary", createdOrder);
            }
            catch (ModelValidationException mvex)
            {
                DisplayOrderErrorMessages(mvex.ValidationResult);
            }
            catch (HttpRequestException ex)
            {
                MessageDialog dlg =
                    new MessageDialog(
                        string.Format(CultureInfo.CurrentCulture, ErrorMessagesHelper.GeneralServiceErrorMessage,
                                      Environment.NewLine, ex.Message), ErrorMessagesHelper.ErrorProcessingOrder);
            }
        }

        private void DisplayOrderErrorMessages(ModelValidationResult validationResult)
        {
            var shippingAddressErrors = new Dictionary<string, ReadOnlyCollection<string>>();
            var billingAddressErrors = new Dictionary<string, ReadOnlyCollection<string>>();
            var paymentMethodErrors = new Dictionary<string, ReadOnlyCollection<string>>();

            // Property keys of the form. Format: order.{ShippingAddress/BillingAddress/PaymentInfo}.{Property}
            foreach (var propkey in validationResult.ModelState.Keys)
            {
                string orderPropAndEntityProp = propkey.Substring(propkey.IndexOf('.') + 1); // strip off order. prefix
                string orderProperty = orderPropAndEntityProp.Substring(0, orderPropAndEntityProp.IndexOf('.') + 1);
                string entityProperty = orderPropAndEntityProp.Substring(orderProperty.IndexOf('.') + 1);

                if (orderProperty.ToLower().Contains("shipping")) shippingAddressErrors.Add(entityProperty, new ReadOnlyCollection<string>(validationResult.ModelState[propkey]));
                if (orderProperty.ToLower().Contains("billing") && !UseSameAddressAsShipping) billingAddressErrors.Add(entityProperty, new ReadOnlyCollection<string>(validationResult.ModelState[propkey]));
                if (orderProperty.ToLower().Contains("payment")) paymentMethodErrors.Add(entityProperty, new ReadOnlyCollection<string>(validationResult.ModelState[propkey]));
            }

            if (shippingAddressErrors.Count > 0) _shippingAddressViewModel.Address.Errors.SetAllErrors(shippingAddressErrors);
            if (billingAddressErrors.Count > 0) _billingAddressViewModel.Address.Errors.SetAllErrors(billingAddressErrors);
            if (paymentMethodErrors.Count > 0) _paymentMethodViewModel.PaymentInfo.Errors.SetAllErrors(paymentMethodErrors);
        }

        public void WindowCurrentSizeChanged()
        {
            if (_isUnsnapping)
            {
                _settingsCharmService.ShowFlyout("signIn", null, successAction: async () => await ProcessFormAsync());
                _isUnsnapping = false;
            }
        }
    }
}
