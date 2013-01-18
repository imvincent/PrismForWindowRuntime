// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Kona.Infrastructure;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Kona.UILogic.Services;

namespace Kona.UILogic.ViewModels
{
    public class CheckoutHubPageViewModel : ViewModel, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly IAccountService _accountService;
        private readonly IOrderService _orderService;
        private IShippingMethodService _shippingMethodService;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IShippingAddressUserControlViewModel _shippingAddressViewModel;
        private readonly IBillingAddressUserControlViewModel _billingAddressViewModel;
        private readonly IPaymentMethodUserControlViewModel _paymentMethodViewModel;
        private bool _useSameAddressAsShipping;
        private bool _shippingInvalid;
        private bool _billingInvalid;
        private bool _paymentInvalid;

        public CheckoutHubPageViewModel(INavigationService navigationService, IAccountService accountService, IOrderService orderService, ShippingMethodServiceProxy shippingMethodService, IShoppingCartRepository shoppingCartRepository,
                                        IShippingAddressUserControlViewModel shippingAddressViewModel, IBillingAddressUserControlViewModel billingAddressViewModel, IPaymentMethodUserControlViewModel paymentMethodViewModel)
        {
            _navigationService = navigationService;
            _accountService = accountService;
            _orderService = orderService;
            _shippingMethodService = shippingMethodService;
            _shoppingCartRepository = shoppingCartRepository;

            _shippingAddressViewModel = shippingAddressViewModel;
            _billingAddressViewModel = billingAddressViewModel;
            _paymentMethodViewModel = paymentMethodViewModel;

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
                BillingAddressViewModel.IsEnabled = !value;
            }
        }

        [RestorableState]
        public bool ShippingInvalid
        {
            get { return _shippingInvalid; }
            set { SetProperty(ref _shippingInvalid, value); }
        }

        [RestorableState]
        public bool BillingInvalid
        {
            get { return _billingInvalid; }
            set { SetProperty(ref _billingInvalid, value); }
        }

        [RestorableState]
        public bool PaymentInvalid
        {
            get { return _paymentInvalid; }
            set { SetProperty(ref _paymentInvalid, value); }
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
            var billingAddressValid = false;
            if (!UseSameAddressAsShipping)
            {
                billingAddressValid = BillingAddressViewModel.ValidateForm();
            }
            else
            {
                billingAddressValid = true;
                BillingAddressViewModel.UpdateAddressInformation(ShippingAddressViewModel.Address);
            }
            var shippingAddressValid = ShippingAddressViewModel.ValidateForm();
            var paymentMethodValid = PaymentMethodViewModel.ValidateForm();

            BillingInvalid = !billingAddressValid;
            ShippingInvalid = !shippingAddressValid;
            PaymentInvalid = !paymentMethodValid;

            if (shippingAddressValid && billingAddressValid && paymentMethodValid)
            {
                ShippingAddressViewModel.ProcessForm();
                BillingAddressViewModel.ProcessForm();
                PaymentMethodViewModel.ProcessForm();

                Order order = new Order
                {
                    ShoppingCart = await _shoppingCartRepository.GetShoppingCartAsync(),
                    UserId = (await _accountService.GetSignedInUserAsync()).UserName,
                    ShippingAddress = ShippingAddressViewModel.Address,
                    BillingAddress = BillingAddressViewModel.Address,
                    PaymentInfo = PaymentMethodViewModel.PaymentInfo,
                    ShippingMethod = (await _shippingMethodService.GetDefaultShippingMethodAsync())
                };

                var result = await _orderService.CreateOrderAsync(order, _accountService.ServerCookieHeader);

                if (result.IsValid)
                {
                    _navigationService.Navigate("CheckoutSummary", result.Order);
                }
                else
                {
                    // we receive a custom message from the service
                    // we manage it in a way it fits best in our validation strategy
                    DisplayErrorMessages(result.Message);
                }
            }
        }

        private void DisplayErrorMessages(dynamic errorMessages)
        {
            // error messages, from the order perspective
            // [{ PropertyName: string, Message: [{ string }] }]
            var shippingAddressErrors = new Dictionary<string, ReadOnlyCollection<string>>();
            var billingAddressErrors = new Dictionary<string, ReadOnlyCollection<string>>();
            var paymentMethodErrors = new Dictionary<string, ReadOnlyCollection<string>>();

            foreach (dynamic propertyErrors in errorMessages)
            {
                string orderProperty = propertyErrors.Property.Value;
                string entityProperty = orderProperty.Substring(orderProperty.LastIndexOf('.') + 1);
                IList<string> messages = propertyErrors.Messages.ToObject<List<string>>();

                if (orderProperty.ToLower().Contains("shipping")) shippingAddressErrors.Add(entityProperty, new ReadOnlyCollection<string>(messages));
                if (orderProperty.ToLower().Contains("billing")) billingAddressErrors.Add(entityProperty, new ReadOnlyCollection<string>(messages));
                if (orderProperty.ToLower().Contains("payment")) paymentMethodErrors.Add(entityProperty, new ReadOnlyCollection<string>(messages));
            }

            if (shippingAddressErrors.Count > 0) _shippingAddressViewModel.Errors.SetAllErrors(shippingAddressErrors);
            if (billingAddressErrors.Count > 0) _billingAddressViewModel.Errors.SetAllErrors(billingAddressErrors);
            if (paymentMethodErrors.Count > 0) _paymentMethodViewModel.Errors.SetAllErrors(paymentMethodErrors);
        }
    }
}
