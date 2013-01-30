// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Kona.Infrastructure;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Kona.UILogic.Services;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Navigation;
using Windows.Globalization.DateTimeFormatting;
using System.Net.Http;

namespace Kona.UILogic.ViewModels
{
    public class CheckoutSummaryPageViewModel : ViewModel, INavigationAware, IHandleWindowSizeChanged
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private Order _order;
        private string _orderSubtotal;
        private string _shippingCost;
        private string _taxCost;
        private string _grandTotal;
        private string _editingDataTypeLabel;
        private bool _isEditPopupOpened;
        private bool _isBottomAppBarOpened;
        private IReadOnlyCollection<ShoppingCartItemViewModel> _shoppingCartItemViewModels;
        private IReadOnlyCollection<ShippingMethod> _shippingMethods;
        private IReadOnlyCollection<CheckoutDataViewModel> _checkoutDataViewModels;
        private ShippingMethod _selectedShippingMethod;
        private CheckoutDataViewModel _selectedCheckoutData;
        private readonly INavigationService _navigationService;
        private readonly IOrderService _orderService;
        private readonly IShippingMethodService _shippingMethodService;
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private readonly IAccountService _accountService;
        private readonly ISettingsCharmService _settingsCharmService;
        private readonly IResourceLoader _resourceLoader;
        private bool _isUnsnapping = false;

        public CheckoutSummaryPageViewModel(INavigationService navigationService, IOrderService orderService, ShippingMethodServiceProxy shippingMethodService, ICheckoutDataRepository checkoutDataRepository, IShoppingCartRepository shoppingCartRepository,
                                            IAccountService accountService, ISettingsCharmService settingsCharmService, IResourceLoader resourceLoader)
        {
            _navigationService = navigationService;
            _orderService = orderService;
            _shippingMethodService = shippingMethodService;
            _checkoutDataRepository = checkoutDataRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _settingsCharmService = settingsCharmService;
            _resourceLoader = resourceLoader;
            _accountService = accountService;

            SubmitCommand = new DelegateCommand(Submit, CanSubmit);
            GoBackCommand = new DelegateCommand(_navigationService.GoBack);
            ChangeCheckoutDataCommand = new DelegateCommand<CheckoutDataViewModel>(ChangeCheckoutData);
        }

        public string OrderSubtotal
        {
            get { return _orderSubtotal; }
            set { SetProperty(ref _orderSubtotal, value); }
        }

        public string ShippingCost
        {
            get { return _shippingCost; }
            set { SetProperty(ref _shippingCost, value); }
        }

        public string TaxCost
        {
            get { return _taxCost; }
            set { SetProperty(ref _taxCost, value); }
        }

        public string GrandTotal
        {
            get { return _grandTotal; }
            set { SetProperty(ref _grandTotal, value); }
        }

        public bool IsEditPopupOpened
        {
            get { return _isEditPopupOpened; }
            set { SetProperty(ref _isEditPopupOpened, value); }
        }

        public bool IsBottomAppBarOpened
        {
            get { return _isBottomAppBarOpened; }
            set { SetProperty(ref _isBottomAppBarOpened, value); }
        }

        public string EditingDataTypeLabel
        {
            get { return _editingDataTypeLabel; }
            set { SetProperty(ref _editingDataTypeLabel, value); }
        }

        public IReadOnlyCollection<ShoppingCartItemViewModel> ShoppingCartItemViewModels
        {
            get { return _shoppingCartItemViewModels; }
            private set { SetProperty(ref _shoppingCartItemViewModels, value); }
        }

        public IReadOnlyCollection<CheckoutDataViewModel> CheckoutDataViewModels
        {
            get { return _checkoutDataViewModels; }
            private set { SetProperty(ref _checkoutDataViewModels, value); }
        }

        public IReadOnlyCollection<ShippingMethod> ShippingMethods
        {
            get { return _shippingMethods; }
            private set { SetProperty(ref _shippingMethods, value); }
        }

        public ShippingMethod SelectedShippingMethod
        {
            get { return _selectedShippingMethod; }
            set
            {
                if (SetProperty(ref _selectedShippingMethod, value))
                {
                    SubmitCommand.RaiseCanExecuteChanged();
                    var shippingCost = _selectedShippingMethod != null ? _selectedShippingMethod.Cost : 0;
                    _order.ShippingMethod = _selectedShippingMethod;
                    UpdatePrices(shippingCost);
                }
            }
        }

        public CheckoutDataViewModel SelectedCheckoutData
        {
            get { return _selectedCheckoutData; }
            set
            {
                if (SetProperty(ref _selectedCheckoutData, value))
                {
                    // We open the AppBar if an item is selected, redisplaying it if necessary.
                    if (_selectedCheckoutData != null)
                    {
                        IsBottomAppBarOpened = false;
                        IsBottomAppBarOpened = true;
                    }
                }
            }
        }

        public DelegateCommand SubmitCommand { get; private set; }

        public DelegateCommand GoBackCommand { get; private set; }

        public DelegateCommand<CheckoutDataViewModel> ChangeCheckoutDataCommand { get; private set; }

        public Action<object> SelectCheckoutDataAction { get; private set; }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState)
        {
            if (navigationParameter == null || navigationParameter.GetType() != typeof(Order))
            {
                throw new ArgumentException("navigationParameter");
            }

            _order = (Order)navigationParameter;

            var shoppingCartItemVMs = new List<ShoppingCartItemViewModel>();
            foreach (var item in _order.ShoppingCart.ShoppingCartItems)
            {
                shoppingCartItemVMs.Add(new ShoppingCartItemViewModel(item));
            }

            ShoppingCartItemViewModels = new ReadOnlyCollection<ShoppingCartItemViewModel>(shoppingCartItemVMs);

            var shippingMethods = await _shippingMethodService.GetShippingMethodsAsync();
            ShippingMethods = new ReadOnlyCollection<ShippingMethod>(shippingMethods.ToList());
            SelectedShippingMethod = ShippingMethods.FirstOrDefault(c => c.Id == _order.ShippingMethod.Id);

            Address shippingAddress = _order.ShippingAddress;
            Address billingAddress = _order.BillingAddress;
            PaymentInfo paymentInfo = _order.PaymentInfo;

            var checkoutData = new List<CheckoutDataViewModel>
            {
                new CheckoutDataViewModel(new { EntityId = shippingAddress.Id, FirstLine = shippingAddress.StreetAddress, SecondLine = String.Format("{0}, {1} {2}", shippingAddress.City, shippingAddress.State, shippingAddress.ZipCode), Name = String.Format("{0} {1} {2}", shippingAddress.FirstName, shippingAddress.MiddleInitial, shippingAddress.LastName), DataType = _resourceLoader.GetString("ShippingAddress"), Content = shippingAddress }, _settingsCharmService),
                new CheckoutDataViewModel(new { EntityId = billingAddress.Id, FirstLine = billingAddress.StreetAddress, SecondLine = String.Format("{0}, {1} {2}", billingAddress.City, billingAddress.State, billingAddress.ZipCode), Name = String.Format("{0} {1} {2}", billingAddress.FirstName, billingAddress.MiddleInitial, billingAddress.LastName), DataType = _resourceLoader.GetString("BillingAddress"), Content = billingAddress }, _settingsCharmService),
                new CheckoutDataViewModel(new { EntityId = paymentInfo.Id, FirstLine = string.Format(CultureInfo.CurrentUICulture, _resourceLoader.GetString("CardEndingIn"), paymentInfo.CardNumber.Substring(paymentInfo.CardNumber.Length - 4)), SecondLine = string.Format(CultureInfo.CurrentUICulture, _resourceLoader.GetString("CardExpiringOn"), string.Format(CultureInfo.CurrentCulture,"{0}/{1}", paymentInfo.ExpirationMonth, paymentInfo.ExpirationYear)), Name = paymentInfo.CardholderName, DataType = _resourceLoader.GetString("PaymentInfo"), Content = paymentInfo }, _settingsCharmService)
            };

            CheckoutDataViewModels = new ObservableCollection<CheckoutDataViewModel>(checkoutData);
            base.OnNavigatedTo(navigationParameter, navigationMode, viewState);
        }

        public void ChangeCheckoutData(CheckoutDataViewModel selectedData)
        {
            if (selectedData == null) return;

            // TODO: display checkout data, depending on the selectedData.DataType

            IsEditPopupOpened = true;
        }

        public void WindowCurrentSizeChanged()
        {
            if (_isUnsnapping)
            {
                _settingsCharmService.ShowFlyout("signIn", null, successAction: async () => await SubmitOrderTransactionAsync());
                _isUnsnapping = false;
            }
        }

        private bool CanSubmit()
        {
            return SelectedShippingMethod != null;
        }

        private async void Submit()
        {
            if (await _accountService.GetSignedInUserAsync() == null)
            {
                if (ApplicationView.Value == ApplicationViewState.Snapped)
                {
                    _isUnsnapping = true;
                    ApplicationView.TryUnsnap();
                }
                _settingsCharmService.ShowFlyout("signIn", null, successAction: async () => await SubmitOrderTransactionAsync());
            }
            else
            {
                await SubmitOrderTransactionAsync();
            }
        }

        private async Task<bool> SubmitOrderTransactionAsync()
        {
            string errorMessage = null;
            try
            {
                await _orderService.ProcessOrderAsync(_order, _accountService.ServerCookieHeader);

                string successTitle = _resourceLoader.GetString("OrderPurchasedTitle");
                string successMessage = _resourceLoader.GetString("OrderPurchasedMessage");
                string dialogOkButtonLabel = _resourceLoader.GetString("DialogOkButtonLabel");

                MessageDialog dialog = new MessageDialog(successMessage, successTitle);
                dialog.Commands.Add(new UICommand(dialogOkButtonLabel, async (command) =>
                    {
                        _navigationService.Navigate("Hub", null);
                        _navigationService.ClearHistory();
                        await _shoppingCartRepository.ClearCartAsync();
                    }));

                await dialog.ShowAsync();
                return true;
            }
            catch (ModelValidationException mvex)
            {
                errorMessage = string.Format(CultureInfo.CurrentCulture, ErrorMessagesHelper.GeneralServiceErrorMessage, Environment.NewLine, mvex.Message);
            }
            // <snippet406>
            catch (HttpRequestException ex)
            {
                errorMessage = string.Format(CultureInfo.CurrentCulture, ErrorMessagesHelper.GeneralServiceErrorMessage, Environment.NewLine, ex.Message);
            }
            if (!(string.IsNullOrWhiteSpace(errorMessage)))
            {
                MessageDialog errorDialog = new MessageDialog(errorMessage,
                    ErrorMessagesHelper.ErrorProcessingOrder);
                await errorDialog.ShowAsync();
            }
            // </snippet406>
            return false;
        }
        
        private void UpdatePrices(double shippingCost)
        {
            var currencyFormatter = new CurrencyFormatter(_order.ShoppingCart.Currency);
            
            OrderSubtotal = currencyFormatter.FormatDouble(Math.Round(_order.ShoppingCart.FullPrice, 2));
            ShippingCost = currencyFormatter.FormatDouble(shippingCost);
            
            var taxCost = Math.Round((_order.ShoppingCart.FullPrice + shippingCost) * _order.ShoppingCart.TaxRate, 2);
            TaxCost = currencyFormatter.FormatDouble(taxCost);
            
            var grandTotal = Math.Round(_order.ShoppingCart.FullPrice + shippingCost + taxCost, 2);
            GrandTotal = currencyFormatter.FormatDouble(grandTotal);
        }
    }
}