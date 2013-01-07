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
using Kona.Infrastructure;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Kona.UILogic.Services;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using System.Security;
using Windows.Globalization.DateTimeFormatting;

namespace Kona.UILogic.ViewModels
{
    public class CheckoutSummaryPageViewModel : ViewModel, INavigationAware
    {
        private ShoppingCart _shoppingCart;
        private string _orderSubtotal;
        private string _shippingCost;
        private string _taxCost;
        private string _grandTotal;
        private string _editingDataTypeLabel;
        private bool _isEditPopupOpened;
        private ShoppingCartItemViewModel _selectedShoppingCartItem;
        private CheckoutDataViewModel _selectedCheckoutData;
        private CheckoutDataViewModel _selectedEditableCheckoutDataViewModel;
        private ShippingMethodViewModel _selectedShippingMethodViewModel;
        private ObservableCollection<ShoppingCartItemViewModel> _shoppingCartItemViewModels;
        private ObservableCollection<CheckoutDataViewModel> _editableCheckoutDataViewModels;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly INavigationService _navigationService;
        private readonly IOrderService _orderService;
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private readonly IAccountService _accountService;
        private ObservableCollection<CheckoutDataViewModel> _checkoutDataViewModels;
        private string _newDataTypeLabel;
        private ISettingsCharmService _settingsCharmService;
        private ReadOnlyCollection<ShippingMethodViewModel> _shippingMethodViewModels;

        public CheckoutSummaryPageViewModel(IShoppingCartRepository shoppingCartRepository,
            INavigationService navigationService, IOrderService orderService, ICheckoutDataRepository checkoutDataRepository,
            IAccountService accountService, ISettingsCharmService settingsCharmService)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _navigationService = navigationService;
            _orderService = orderService;
            _checkoutDataRepository = checkoutDataRepository;
            _settingsCharmService = settingsCharmService;
            SubmitCommand = new DelegateCommand(Submit);
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

        public string NewDataTypeLabel
        {
            get { return _newDataTypeLabel; }
            set { SetProperty(ref _newDataTypeLabel, value); }
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

        public ReadOnlyCollection<ShippingMethodViewModel> ShippingMethodViewModels
        {
            get { return _shippingMethodViewModels; }
            private set { SetProperty(ref _shippingMethodViewModels, value); }
        }

        public ShippingMethodViewModel SelectedShippingMethodViewModel
        {
            get { return _selectedShippingMethodViewModel; }
            set
            {
                SetProperty(ref _selectedShippingMethodViewModel, value);
                SubmitCommand.RaiseCanExecuteChanged();
                UpdatePrices(_selectedShippingMethodViewModel.ShippingMethod.Cost);
            }
        }

        public DelegateCommand SubmitCommand { get; private set; }

        public DelegateCommand GoBackCommand { get; private set; }

        public DelegateCommand<CheckoutDataViewModel> AppBarEditAddressCommand { get; private set; }

        public Action<object> SelectCheckoutDataAction { get; private set; }

        //public DelegateCommand<CheckoutDataViewModel> EditAddressCommand { get; private set; }

        public DelegateCommand<CheckoutDataViewModel> AddNewCommand { get; private set; }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState)
        {
            _shoppingCart = await _shoppingCartRepository.GetShoppingCartAsync();

            if (_shoppingCart != null)
            {
                UpdatePrices(0);

                ShoppingCartItemViewModels = new ObservableCollection<ShoppingCartItemViewModel>();
                foreach (var item in _shoppingCart.ShoppingCartItems)
                {
                    ShoppingCartItemViewModels.Add(new ShoppingCartItemViewModel(item));
                }

                ShippingMethodViewModels = new ReadOnlyCollection<ShippingMethodViewModel>(new List<ShippingMethodViewModel>() { 
                new ShippingMethodViewModel(new ShippingMethod() { Description = "Standard Shipping", EstimatedTime = "5-8 business days", Cost = 7.65 })
                {Currency = _shoppingCart.Currency},
                new ShippingMethodViewModel(new ShippingMethod() { Description = "Expedited Shipping", EstimatedTime = "3-5 business days", Cost = 14.25 })
                {Currency = _shoppingCart.Currency},
                new ShippingMethodViewModel(new ShippingMethod() { Description = "Two-day Shipping", EstimatedTime = "2 business days", Cost = 26.45 })
                {Currency = _shoppingCart.Currency},
                new ShippingMethodViewModel(new ShippingMethod() { Description = "One-day Shipping", EstimatedTime = "1 business day", Cost = 37.32 })
                {Currency = _shoppingCart.Currency}
            });
            }


            var checkoutInfo = _shoppingCartRepository.AddressAndPurchaseInfo;
            Address shippingAddress = checkoutInfo.ShippingAddress;
            Address billingAddress = checkoutInfo.BillingAddress;
            PaymentInfo paymentInfo = checkoutInfo.PaymentInfo;

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

            var resourceLoader = new ResourceLoader();

            var checkoutData = new List<CheckoutDataViewModel>
                { new CheckoutDataViewModel(new { EntityId = shippingAddress.Id, FirstLine = shippingAddress.StreetAddress, SecondLine = shippingAddress.City + "," + shippingAddress.State + " " + shippingAddress.ZipCode, Name = shippingAddress.FirstName + " " + shippingAddress.MiddleInitial + " " + shippingAddress.LastName, DataType = resourceLoader.GetString("ShippingAddress"), Content = shippingAddress }),
                  new CheckoutDataViewModel(new { EntityId = billingAddress.Id, FirstLine = billingAddress.StreetAddress, SecondLine = billingAddress.City + "," + billingAddress.State + " " + billingAddress.ZipCode, Name = billingAddress.FirstName + " " + billingAddress.MiddleInitial + " " + billingAddress.LastName, DataType = resourceLoader.GetString("BillingAddress"), Content = billingAddress }),
                  new CheckoutDataViewModel(new { EntityId = paymentInfo.Id, FirstLine = string.Format(CultureInfo.CurrentUICulture, resourceLoader.GetString("CardEndingIn"), paymentInfo.CardNumber.Substring(paymentInfo.CardNumber.Length - 4)), SecondLine = string.Format(CultureInfo.CurrentUICulture, resourceLoader.GetString("CardExpiringOn"), yearMonthDateTimeFormatter.Format(creditCardExpirationDateOffset)), Name = paymentInfo.CardholderName, DataType = resourceLoader.GetString("Payment"), Content = paymentInfo })
                };
            CheckoutDataViewModels = new ObservableCollection<CheckoutDataViewModel>(checkoutData);

            base.OnNavigatedTo(navigationParameter, navigationMode, viewState);
        }

        private void UpdatePrices(double shippingCost)
        {
            var currencyFormatter = new CurrencyFormatter(_shoppingCart.Currency);
            OrderSubtotal = currencyFormatter.FormatDouble(_shoppingCart.FullPrice);
            ShippingCost = currencyFormatter.FormatDouble(shippingCost);
            var taxCost = Math.Round((_shoppingCart.FullPrice + shippingCost) * _shoppingCart.TaxRate, 2);
            TaxCost = currencyFormatter.FormatDouble(taxCost);
            var grandTotal = Math.Round(_shoppingCart.FullPrice + shippingCost + taxCost, 2);
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
                case "Payment": _settingsCharmService.ShowFlyout("newPaymentMethod");
                    break;
            }
        }

        public void OpenEditAddressFlyout(CheckoutDataViewModel selectedData)
        {
            var resourceLoader = new ResourceLoader();
            if (selectedData == null) return;

            EditingDataTypeLabel = resourceLoader.GetString("Editing") + " " + selectedData.DataType;
            NewDataTypeLabel = resourceLoader.GetString("AddNew") + selectedData.DataType;


            var editableViewModels = new ObservableCollection<CheckoutDataViewModel>() { selectedData };

            if (selectedData.DataType == resourceLoader.GetString("ShippingAddress"))
            {
                var shippingAddresses = _checkoutDataRepository.RetrieveAllShippingAddresses();
                foreach (var address in shippingAddresses)
                {
                    if (address.Id != selectedData.EntityId)
                    {
                        editableViewModels.Add(new CheckoutDataViewModel(new { EntityId = address.Id, FirstLine = address.StreetAddress, SecondLine = address.City + "," + address.State + " " + address.ZipCode, Name = address.FirstName + " " + address.MiddleInitial + " " + address.LastName, DataType = resourceLoader.GetString("ShippingAddress"), Content = address }));
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
                        editableViewModels.Add(new CheckoutDataViewModel(new { EntityId = billingAddress.Id, FirstLine = billingAddress.StreetAddress, SecondLine = billingAddress.City + "," + billingAddress.State + " " + billingAddress.ZipCode, Name = billingAddress.FirstName + " " + billingAddress.MiddleInitial + " " + billingAddress.LastName, DataType = resourceLoader.GetString("BillingAddress"), Content = billingAddress }));
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
                        editableViewModels.Add(new CheckoutDataViewModel(new { EntityId = paymentInfo.Id, FirstLine = string.Format(CultureInfo.CurrentUICulture, resourceLoader.GetString("CardEndingIn"), paymentInfo.CardNumber.Substring(paymentInfo.CardNumber.Length - 4)), SecondLine = string.Format(CultureInfo.CurrentUICulture, resourceLoader.GetString("CardExpiringOn"), yearMonthDateTimeFormatter.Format(creditCardExpirationDateOffset)), Name = paymentInfo.CardholderName, DataType = resourceLoader.GetString("Payment"), Content = paymentInfo }));
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
            if (SelectedShippingMethodViewModel == null) return false;
            else return true;
        }


        public async void Submit()
        {
            Order order = new Order();
            order.ShippingAddress = _shoppingCartRepository.AddressAndPurchaseInfo.ShippingAddress;
            order.BillingAddress = _shoppingCartRepository.AddressAndPurchaseInfo.BillingAddress;
            order.Payment = _shoppingCartRepository.AddressAndPurchaseInfo.PaymentInfo;
            order.Cart = await _shoppingCartRepository.GetShoppingCartAsync();
            order.ShipMethod = SelectedShippingMethodViewModel.ShippingMethod;

            Action submitOrderAction = async () =>
                        {
                            await _orderService.SubmitOrder(order, _accountService.ServerCookieHeader);
                            HandleOrderComplete();
                        };

            if (await _accountService.CheckIfUserSignedIn() != null)
            {
                submitOrderAction();
            }
            else
            {
                _accountService.DisplaySignIn(submitOrderAction, null);
            }
        }

        private async void HandleOrderComplete()
        {
            MessageDialog dialog = new MessageDialog("Order complete");
            await dialog.ShowAsync(); // TODO: Navigate?
            await _shoppingCartRepository.ClearCartAsync();
        }
    }
}