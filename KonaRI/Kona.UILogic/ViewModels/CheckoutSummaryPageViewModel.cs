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

namespace Kona.UILogic.ViewModels
{
    public class CheckoutSummaryPageViewModel : ViewModel, INavigationAware, IHandleWindowSizeChanged
    {
        private IShoppingCartRepository _shoppingCartRepository;
        private Order _order;
        private string _orderSubtotal;
        private string _shippingCost;
        private string _taxCost;
        private string _grandTotal;
        private string _editingDataTypeLabel;
        private bool _isEditPopupOpened;
        private ShoppingCartItemViewModel _selectedShoppingCartItem;
        private CheckoutDataViewModel _selectedCheckoutData;
        private CheckoutDataViewModel _selectedEditableCheckoutDataViewModel;
        private IReadOnlyCollection<ShippingMethod> _shippingMethods;
        private ShippingMethod _selectedShippingMethod;
        private ObservableCollection<ShoppingCartItemViewModel> _shoppingCartItemViewModels;
        private ObservableCollection<CheckoutDataViewModel> _editableCheckoutDataViewModels;
        private readonly INavigationService _navigationService;
        private readonly IOrderService _orderService;
        private readonly IShippingMethodService _shippingMethodService;
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private readonly IAccountService _accountService;
        private ObservableCollection<CheckoutDataViewModel> _checkoutDataViewModels;
        private ISettingsCharmService _settingsCharmService;
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
            AppBarEditAddressCommand = new DelegateCommand<CheckoutDataViewModel>(OpenEditAddressFlyout);
            AddNewCommand = new DelegateCommand<CheckoutDataViewModel>(OpenAddEntitySettingsFlyout);
            SelectCheckoutDataAction = SelectCheckoutData;
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

        public string EditingDataTypeLabel
        {
            get { return _editingDataTypeLabel; }
            set { SetProperty(ref _editingDataTypeLabel, value); }
        }

        public ShoppingCartItemViewModel SelectedShoppingCartItem
        {
            get { return _selectedShoppingCartItem; }
            set { SetProperty(ref _selectedShoppingCartItem, value); }
        }

        public ObservableCollection<ShoppingCartItemViewModel> ShoppingCartItemViewModels
        {
            get { return _shoppingCartItemViewModels; }
            private set { SetProperty(ref _shoppingCartItemViewModels, value); }
        }

        public CheckoutDataViewModel SelectedEditableCheckoutDataViewModel
        {
            get { return _selectedEditableCheckoutDataViewModel; }
            set { SetProperty(ref _selectedEditableCheckoutDataViewModel, value); }
        }

        public ObservableCollection<CheckoutDataViewModel> CheckoutDataViewModels
        {
            get { return this._checkoutDataViewModels; }
            private set { SetProperty(ref this._checkoutDataViewModels, value); }
        }

        public ObservableCollection<CheckoutDataViewModel> EditableCheckoutDataViewModels
        {
            get { return _editableCheckoutDataViewModels; }
            private set { SetProperty(ref _editableCheckoutDataViewModels, value); }
        }

        public CheckoutDataViewModel SelectedCheckoutData
        {
            get { return _selectedCheckoutData; }
            set { SetProperty(ref _selectedCheckoutData, value); }
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

        public DelegateCommand SubmitCommand { get; private set; }

        public DelegateCommand GoBackCommand { get; private set; }

        public DelegateCommand<CheckoutDataViewModel> AppBarEditAddressCommand { get; private set; }

        public Action<object> SelectCheckoutDataAction { get; private set; }

        public DelegateCommand<CheckoutDataViewModel> AddNewCommand { get; private set; }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState)
        {
            if (navigationParameter == null || navigationParameter.GetType() != typeof(Order))
            {
                throw new ArgumentException("navigationParameter");
            }

            _order = (Order)navigationParameter;

            ShoppingCartItemViewModels = new ObservableCollection<ShoppingCartItemViewModel>();
            foreach (var item in _order.ShoppingCart.ShoppingCartItems)
            {
                ShoppingCartItemViewModels.Add(new ShoppingCartItemViewModel(item));
            }

            var shippingMethods = await _shippingMethodService.GetShippingMethodsAsync();
            ShippingMethods = new ReadOnlyCollection<ShippingMethod>(shippingMethods);
            SelectedShippingMethod = ShippingMethods.FirstOrDefault(c => c.Id == _order.ShippingMethod.Id);

            Address shippingAddress = _order.ShippingAddress;
            Address billingAddress = _order.BillingAddress;
            PaymentInfo paymentInfo = _order.PaymentInfo;

            var tempDateTimeFormatter = new DateTimeFormatter("shortdate");

            var yearMonthDateTimeFormatter = new DateTimeFormatter(YearFormat.Full, MonthFormat.Abbreviated,
                                                                   DayFormat.None, DayOfWeekFormat.None, HourFormat.None, MinuteFormat.None, SecondFormat.None,
                                                                   tempDateTimeFormatter.Languages,
                                                                   tempDateTimeFormatter.GeographicRegion,
                                                                   CalendarIdentifiers.Gregorian,
                                                                   tempDateTimeFormatter.Clock);
            var creditCardExpirationDateOffset =
                new DateTimeOffset(new DateTime(int.Parse(paymentInfo.ExpirationYear),
                                                int.Parse(paymentInfo.ExpirationMonth), 2));

            var checkoutData = new List<CheckoutDataViewModel>
                { new CheckoutDataViewModel(new { EntityId = shippingAddress.Id, FirstLine = shippingAddress.StreetAddress, SecondLine = String.Format("{0}, {1} {2}", shippingAddress.City, shippingAddress.State, shippingAddress.ZipCode), Name = String.Format("{0} {1} {2}", shippingAddress.FirstName, shippingAddress.MiddleInitial, shippingAddress.LastName), DataType = _resourceLoader.GetString("ShippingAddress"), Content = shippingAddress }, _settingsCharmService),
                  new CheckoutDataViewModel(new { EntityId = billingAddress.Id, FirstLine = billingAddress.StreetAddress, SecondLine = String.Format("{0}, {1} {2}", billingAddress.City, billingAddress.State, billingAddress.ZipCode), Name = String.Format("{0} {1} {2}", billingAddress.FirstName, billingAddress.MiddleInitial, billingAddress.LastName), DataType = _resourceLoader.GetString("BillingAddress"), Content = billingAddress }, _settingsCharmService),
                  new CheckoutDataViewModel(new { EntityId = paymentInfo.Id, FirstLine = string.Format(CultureInfo.CurrentUICulture, _resourceLoader.GetString("CardEndingIn"), paymentInfo.CardNumber.Substring(paymentInfo.CardNumber.Length - 4)), SecondLine = string.Format(CultureInfo.CurrentUICulture, _resourceLoader.GetString("CardExpiringOn"), yearMonthDateTimeFormatter.Format(creditCardExpirationDateOffset)), Name = paymentInfo.CardholderName, DataType = _resourceLoader.GetString("Payment"), Content = paymentInfo }, _settingsCharmService)
                };
            CheckoutDataViewModels = new ObservableCollection<CheckoutDataViewModel>(checkoutData);

            base.OnNavigatedTo(navigationParameter, navigationMode, viewState);
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

        public void OpenAddEntitySettingsFlyout(CheckoutDataViewModel data)
        {
            if (data == null) return;

            switch (data.DataType)
            {
                case "Billing Address": _settingsCharmService.ShowFlyout("newBillingAddress");
                    break;
                case "Shipping Address": _settingsCharmService.ShowFlyout("newShippingAddress");
                    break;
                case "PaymentInfo": _settingsCharmService.ShowFlyout("newPaymentMethod");
                    break;
            }
        }

        public void OpenEditAddressFlyout(CheckoutDataViewModel selectedData)
        {
            var resourceLoader = new ResourceLoader();
            if (selectedData == null) return;

            EditingDataTypeLabel = String.Format(CultureInfo.CurrentUICulture, resourceLoader.GetString("Editing"), selectedData.DataType);

            var editableViewModels = new ObservableCollection<CheckoutDataViewModel>() { selectedData };

            if (selectedData.DataType == resourceLoader.GetString("ShippingAddress"))
            {
                var shippingAddresses = _checkoutDataRepository.RetrieveAllShippingAddresses();
                foreach (var address in shippingAddresses)
                {
                    if (address.Id != selectedData.EntityId)
                    {
                        editableViewModels.Add(new CheckoutDataViewModel(new { EntityId = address.Id, FirstLine = address.StreetAddress, SecondLine = String.Format(CultureInfo.CurrentUICulture, "{0}, {1} {2}", address.City, address.State, address.ZipCode), Name = String.Format(CultureInfo.CurrentUICulture, "{0} {1} {2}", address.FirstName, address.MiddleInitial, address.LastName), DataType = resourceLoader.GetString("ShippingAddress"), Content = address }, _settingsCharmService));
                    }
                }
            }
            else if (selectedData.DataType == resourceLoader.GetString("BillingAddress"))
            {
                var billingAddresses = _checkoutDataRepository.RetrieveAllBillingAddresses();
                foreach (var billingAddress in billingAddresses)
                {
                    if (billingAddress.Id != selectedData.EntityId)
                    {
                        editableViewModels.Add(new CheckoutDataViewModel(new { EntityId = billingAddress.Id, FirstLine = billingAddress.StreetAddress, SecondLine = String.Format(CultureInfo.CurrentUICulture, "{0}, {1} {2}", billingAddress.City, billingAddress.State, billingAddress.ZipCode), Name = String.Format(CultureInfo.CurrentUICulture, "{0} {1} {2}", billingAddress.FirstName, billingAddress.MiddleInitial, billingAddress.LastName), DataType = resourceLoader.GetString("BillingAddress"), Content = billingAddress }, _settingsCharmService));
                    }
                }
            }
            else
            {
                var tempDateTimeFormatter = new DateTimeFormatter("shortdate");
                var yearMonthDateTimeFormatter = new DateTimeFormatter(YearFormat.Full, MonthFormat.Abbreviated,
                                                                   DayFormat.None, DayOfWeekFormat.None, HourFormat.None, MinuteFormat.None, SecondFormat.None,
                                                                   tempDateTimeFormatter.Languages,
                                                                   tempDateTimeFormatter.GeographicRegion,
                                                                   CalendarIdentifiers.Gregorian,
                                                                   tempDateTimeFormatter.Clock);
                var paymentInfos = _checkoutDataRepository.RetrieveAllPaymentInformation();
                foreach (var paymentInfo in paymentInfos)
                {
                    if (paymentInfo.Id != selectedData.EntityId)
                    {
                        var creditCardExpirationDateOffset =
    new DateTimeOffset(new DateTime(int.Parse(paymentInfo.ExpirationYear, CultureInfo.CurrentUICulture),
                                    int.Parse(paymentInfo.ExpirationMonth, CultureInfo.CurrentUICulture), 2));
                        editableViewModels.Add(new CheckoutDataViewModel(new { EntityId = paymentInfo.Id, FirstLine = string.Format(CultureInfo.CurrentUICulture, resourceLoader.GetString("CardEndingIn"), paymentInfo.CardNumber.Substring(paymentInfo.CardNumber.Length - 4)), SecondLine = string.Format(CultureInfo.CurrentUICulture, resourceLoader.GetString("CardExpiringOn"), yearMonthDateTimeFormatter.Format(creditCardExpirationDateOffset)), Name = paymentInfo.CardholderName, DataType = resourceLoader.GetString("Payment"), Content = paymentInfo }, _settingsCharmService));
                    }
                }
            }

            EditableCheckoutDataViewModels = editableViewModels;
            IsEditPopupOpened = true;
        }

        public void SelectCheckoutData(object parameter)
        {
            var selectedCheckoutData = parameter as CheckoutDataViewModel;
            if (selectedCheckoutData == null) return;
            int index;

            switch (selectedCheckoutData.DataType)
            {
                case "Shipping Address":
                    index = 0;
                    break;
                case "Billing Address":
                    index = 1;
                    break;
                default:
                    index = 2;
                    break;
            }
            this.CheckoutDataViewModels[index] = selectedCheckoutData;
        }

        private bool CanSubmit()
        {
            return SelectedShippingMethod != null;
        }

        public async void Submit()
        {
            var user = await _accountService.GetSignedInUserAsync();

            if (user != null)
            {
                await SubmitOrderTransactionAsync();
            }
            else
            {
                if (ApplicationView.Value == ApplicationViewState.Snapped)
                {
                    _isUnsnapping = true;
                    ApplicationView.TryUnsnap();
                }

                _settingsCharmService.ShowFlyout("signIn", null, successAction: async () => await SubmitOrderTransactionAsync());
            }
        }

        private async Task<bool> SubmitOrderTransactionAsync()
        {
            var result = await _orderService.ProcessOrderAsync(_order, _accountService.ServerCookieHeader);

            if (result.IsValid)
            {
                    // TODO: replace message with localized strings
                    MessageDialog dialog = new MessageDialog("Your order has been submitted. Thank you for your purchase.", "Order complete");
                    dialog.Commands.Add(new UICommand("OK", async (command) =>
                        {
                            _navigationService.Navigate("Hub", null);
                            await _shoppingCartRepository.ClearCartAsync();
                        }));

                    await dialog.ShowAsync();
                    return true;
            }
            else
            {
                // TODO: replace message with localized strings
                var errorMessage = "The following error messages were received from the service: {0} {1}";
                MessageDialog errorDialog = new MessageDialog(string.Format(CultureInfo.CurrentCulture, errorMessage, Environment.NewLine, result.Message),
                    "There was an error while proccessing your order");
                await errorDialog.ShowAsync();
                return false;   
            }
        }

        public void WindowCurrentSizeChanged()
        {
            if (_isUnsnapping)
            {
                _settingsCharmService.ShowFlyout("signIn", null, successAction: async () => await SubmitOrderTransactionAsync());
                _isUnsnapping = false;
            }
        }
    }
}