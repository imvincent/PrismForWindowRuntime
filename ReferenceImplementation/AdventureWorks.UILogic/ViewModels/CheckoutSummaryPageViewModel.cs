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
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Services;
using Microsoft.Practices.StoreApps.Infrastructure;
using Microsoft.Practices.StoreApps.Infrastructure.Interfaces;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;

namespace AdventureWorks.UILogic.ViewModels
{
    public class CheckoutSummaryPageViewModel : ViewModel
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private Order _order;
        private string _orderSubtotal;
        private string _shippingCost;
        private string _taxCost;
        private string _grandTotal;
        private bool _isSelectCheckoutDataPopupOpened;
        private bool _isBottomAppBarOpened;
        private string _selectCheckoutDataTypeLabel;
        private IReadOnlyCollection<ShoppingCartItemViewModel> _shoppingCartItemViewModels;
        private IReadOnlyCollection<ShippingMethod> _shippingMethods;
        private ObservableCollection<CheckoutDataViewModel> _checkoutDataViewModels;
        private IReadOnlyCollection<CheckoutDataViewModel> _allCheckoutDataViewModels;
        private ShippingMethod _selectedShippingMethod;
        private CheckoutDataViewModel _selectedCheckoutData;
        private CheckoutDataViewModel _selectedAllCheckoutData;
        private readonly INavigationService _navigationService;
        private readonly IOrderService _orderService;
        private readonly IOrderRepository _orderRepository;
        private readonly IShippingMethodService _shippingMethodService;
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private readonly IAccountService _accountService;
        private readonly IFlyoutService _flyoutService;
        private readonly IResourceLoader _resourceLoader;
        private readonly IAlertMessageService _alertMessageService;

        public CheckoutSummaryPageViewModel(INavigationService navigationService, IOrderService orderService, IOrderRepository orderRepository, IShippingMethodService shippingMethodService, ICheckoutDataRepository checkoutDataRepository, IShoppingCartRepository shoppingCartRepository,
                                            IAccountService accountService, IFlyoutService flyoutService, IResourceLoader resourceLoader, IAlertMessageService alertMessageService)
        {
            _navigationService = navigationService;
            _orderService = orderService;
            _orderRepository = orderRepository;
            _shippingMethodService = shippingMethodService;
            _checkoutDataRepository = checkoutDataRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _flyoutService = flyoutService;
            _resourceLoader = resourceLoader;
            _accountService = accountService;
            _alertMessageService = alertMessageService;

            SubmitCommand = DelegateCommand.FromAsyncHandler(Submit, CanSubmit);
            GoBackCommand = new DelegateCommand(_navigationService.GoBack);

            EditCheckoutDataCommand = new DelegateCommand(EditCheckoutData);
            SelectCheckoutDataCommand = new DelegateCommand(async () => await SelectCheckoutDataAsync());
            AddCheckoutDataCommand = new DelegateCommand(AddCheckoutData);
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

        public bool IsSelectCheckoutDataPopupOpened
        {
            get { return _isSelectCheckoutDataPopupOpened; }
            set { SetProperty(ref _isSelectCheckoutDataPopupOpened, value); }
        }

        public bool IsBottomAppBarOpened
        {
            get { return _isBottomAppBarOpened; }
            set 
            {
                // We always fire the PropertyChanged event because the 
                // AppBar.IsOpen property doesn't notify when the property is set.
                // See http://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.xaml.controls.appbar.isopen.aspx
                _isBottomAppBarOpened = value;
                OnPropertyChanged("IsBottomAppBarOpened");
            }
        }

        public string SelectCheckoutDataLabel
        {
            get { return _selectCheckoutDataTypeLabel; }
            set { SetProperty(ref _selectCheckoutDataTypeLabel, value); }
        }

        public IReadOnlyCollection<ShoppingCartItemViewModel> ShoppingCartItemViewModels
        {
            get { return _shoppingCartItemViewModels; }
            private set { SetProperty(ref _shoppingCartItemViewModels, value); }
        }

        public ObservableCollection<CheckoutDataViewModel> CheckoutDataViewModels
        {
            get { return _checkoutDataViewModels; }
            private set { SetProperty(ref _checkoutDataViewModels, value); }
        }

        public IReadOnlyCollection<CheckoutDataViewModel> AllCheckoutDataViewModels
        {
            get { return _allCheckoutDataViewModels; }
            private set { SetProperty(ref _allCheckoutDataViewModels, value); }
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
                    _order.ShippingMethod = _selectedShippingMethod;
                    SubmitCommand.RaiseCanExecuteChanged();
                    var shippingCost = _selectedShippingMethod != null ? _selectedShippingMethod.Cost : 0;
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
                    if (_selectedCheckoutData != null)
                    {
                        // Display the AppBar
                        IsBottomAppBarOpened = true;
                    }
                }
            }
        }

        public CheckoutDataViewModel SelectedAllCheckoutData
        {
            get { return _selectedAllCheckoutData; }
            set
            {
                var oldValue = _selectedAllCheckoutData;
                if (SetProperty(ref _selectedAllCheckoutData, value) && value != null && oldValue != null)
                {
                    // Update the CheckoutData of the Order
                    UpdateOrderCheckoutData(_selectedAllCheckoutData);
                }
            }
        }

        public DelegateCommand SubmitCommand { get; private set; }

        public DelegateCommand GoBackCommand { get; private set; }

        public DelegateCommand EditCheckoutDataCommand { get; private set; }

        public DelegateCommand SelectCheckoutDataCommand { get; private set; }

        public DelegateCommand AddCheckoutDataCommand { get; private set; }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            // Get latest shopping cart
            var shoppingCart = await _shoppingCartRepository.GetShoppingCartAsync();
            _order = _orderRepository.CurrentOrder;
            _order.ShoppingCart = shoppingCart;

            // Populate the ShoppingCart items
            var shoppingCartItemVMs = _order.ShoppingCart.ShoppingCartItems.Select(item => new ShoppingCartItemViewModel(item));
            ShoppingCartItemViewModels = new ReadOnlyCollection<ShoppingCartItemViewModel>(shoppingCartItemVMs.ToList());

            // Populate the ShippingMethods and set the selected one
            var shippingMethods = await _shippingMethodService.GetShippingMethodsAsync();
            ShippingMethods = new ReadOnlyCollection<ShippingMethod>(shippingMethods.ToList());
            SelectedShippingMethod = _order.ShippingMethod != null ? ShippingMethods.FirstOrDefault(c => c.Id == _order.ShippingMethod.Id) : null;

            // Populate the CheckoutData items (Addresses & payment information)
            CheckoutDataViewModels = new ObservableCollection<CheckoutDataViewModel>
                {
                    CreateCheckoutData(_order.ShippingAddress, Constants.ShippingAddress),
                    CreateCheckoutData(_order.BillingAddress, Constants.BillingAddress),
                    CreateCheckoutData(_order.PaymentMethod)
                };

            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

            if (navigationMode == NavigationMode.Refresh)
            {
                // Restore the selected CheckoutData manually
                string selectedCheckoutData = RetrieveEntityStateValue<string>("selectedCheckoutData", viewModelState);

                if (!string.IsNullOrWhiteSpace(selectedCheckoutData))
                {
                    SelectedCheckoutData = CheckoutDataViewModels.FirstOrDefault(c => c.EntityId == selectedCheckoutData);
                }
            }
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatedFrom(viewModelState, suspending);

            if (SelectedCheckoutData != null)
            {
                // Store the selected CheckoutData manually
                AddEntityStateValue("selectedCheckoutData", SelectedCheckoutData.EntityId, viewModelState);
            }
        }

        private bool CanSubmit()
        {
            return SelectedShippingMethod != null;
        }

        private async Task Submit()
        {
            string errorMessage = string.Empty;
            try
            {
                if (await _accountService.VerifyUserAuthenticationAsync() == null)
                {
                    _flyoutService.ShowFlyout("SignIn", null, async () => await SubmitOrderTransactionAsync());
                }
                else
                {
                    await SubmitOrderTransactionAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                errorMessage = string.Format(CultureInfo.CurrentCulture, _resourceLoader.GetString("GeneralServiceErrorMessage"), Environment.NewLine, ex.Message);
            }
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                await _alertMessageService.ShowAsync(errorMessage, _resourceLoader.GetString("ErrorProcessingOrder"));
            }
        }

        private async Task<bool> SubmitOrderTransactionAsync()
        {
            string errorMessage = string.Empty;

            try
            {
                await _orderService.ProcessOrderAsync(_order, _accountService.ServerCookieHeader);

                string successTitle = _resourceLoader.GetString("OrderPurchasedTitle");
                string successMessage = _resourceLoader.GetString("OrderPurchasedMessage");
                var dialogOkCommand = new DialogCommand()
                    {
                        Label = _resourceLoader.GetString("DialogOkButtonLabel"),
                        Invoked = async () =>
                            {
                                _navigationService.ClearHistory();
                                _navigationService.Navigate("Hub", null);
                                await _shoppingCartRepository.ClearCartAsync();
                            }
                    };

                await _alertMessageService.ShowAsync(successMessage, successTitle, new List<DialogCommand>() { dialogOkCommand });
                return true;
            }
            catch (ModelValidationException mvex)
            {
                errorMessage = string.Format(CultureInfo.CurrentCulture, _resourceLoader.GetString("GeneralServiceErrorMessage"), Environment.NewLine, mvex.Message);
            }
            // <snippet406>
            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                await _alertMessageService.ShowAsync(errorMessage, _resourceLoader.GetString("ErrorProcessingOrder"));
            }

            return false;
            // </snippet406>
        }

        private void AddCheckoutData()
        {
            var selectedData = SelectedCheckoutData;
            if (selectedData == null) return;

            // Add a new address/payment
            string addNewAddressType = selectedData.DataType == Constants.ShippingAddress ? "ShippingAddress"
                                    : selectedData.DataType == Constants.BillingAddress ? "BillingAddress" : "PaymentMethod";

            // Hide the Popup
            IsSelectCheckoutDataPopupOpened = false;

            _navigationService.Navigate(addNewAddressType, null);
        }

        private void EditCheckoutData()
        {
            var selectedData = SelectedCheckoutData;
            if (selectedData == null) return;
            
            // Hide the App Bar
            IsBottomAppBarOpened = false;

            // Edit selected address/payment
            _navigationService.Navigate(selectedData.DataType, selectedData.EntityId);
        }

        private async Task SelectCheckoutDataAsync()
        {
            var selectedData = SelectedCheckoutData;
            IEnumerable<CheckoutDataViewModel> checkoutData = null;

            switch (selectedData.DataType)
            {
                case Constants.ShippingAddress:
                    checkoutData = (await _checkoutDataRepository.GetAllShippingAddressesAsync()).Select(address => CreateCheckoutData(address, Constants.ShippingAddress));
                    AllCheckoutDataViewModels = new ReadOnlyCollection<CheckoutDataViewModel>(checkoutData.ToList());
                    SelectCheckoutDataLabel = string.Format(CultureInfo.CurrentUICulture, _resourceLoader.GetString("SelectCheckoutData"), _resourceLoader.GetString("ShippingAddress"));
                    break;
                case Constants.BillingAddress:
                    checkoutData = (await _checkoutDataRepository.GetAllBillingAddressesAsync()).Select(address => CreateCheckoutData(address, Constants.BillingAddress));
                    AllCheckoutDataViewModels = new ReadOnlyCollection<CheckoutDataViewModel>(checkoutData.ToList());
                    SelectCheckoutDataLabel = string.Format(CultureInfo.CurrentUICulture, _resourceLoader.GetString("SelectCheckoutData"), _resourceLoader.GetString("BillingAddress"));
                    break;
                case Constants.PaymentMethod:
                    checkoutData = (await _checkoutDataRepository.GetAllPaymentMethodsAsync()).Select(CreateCheckoutData);
                    AllCheckoutDataViewModels = new ReadOnlyCollection<CheckoutDataViewModel>(checkoutData.ToList());
                    SelectCheckoutDataLabel = string.Format(CultureInfo.CurrentUICulture, _resourceLoader.GetString("SelectCheckoutData"), _resourceLoader.GetString("PaymentMethod"));
                    break;
            }

            if (AllCheckoutDataViewModels != null)
            {
                IsSelectCheckoutDataPopupOpened = true;

                // Select the order's CheckoutData
                SelectedAllCheckoutData = AllCheckoutDataViewModels.FirstOrDefault(c => c.EntityId == SelectedCheckoutData.EntityId);
            }
        }

        private void UpdatePrices(double shippingCost)
        {
            var currencyFormatter = new CurrencyFormatter(_order.ShoppingCart.Currency);

            OrderSubtotal = currencyFormatter.FormatDouble(Math.Round(_order.ShoppingCart.TotalPrice, 2));
            ShippingCost = currencyFormatter.FormatDouble(shippingCost);

            var taxCost = Math.Round((_order.ShoppingCart.TotalPrice + shippingCost) * _order.ShoppingCart.TaxRate, 2);
            TaxCost = currencyFormatter.FormatDouble(taxCost);

            var grandTotal = Math.Round(_order.ShoppingCart.TotalPrice + shippingCost + taxCost, 2);
            GrandTotal = currencyFormatter.FormatDouble(grandTotal);
        }

        private async void UpdateOrderCheckoutData(CheckoutDataViewModel checkouData)
        {
            // Update order & CheckoutData collection items with the new info
            switch (checkouData.DataType)
            {
                case Constants.ShippingAddress:
                    var updatedShippingAddress = await _checkoutDataRepository.GetShippingAddressAsync(checkouData.EntityId);
                    CheckoutDataViewModels[0] = CreateCheckoutData(updatedShippingAddress, Constants.ShippingAddress);
                    _order.ShippingAddress = updatedShippingAddress;
                    break;
                case Constants.BillingAddress:
                    var updatedBillingAddress = await _checkoutDataRepository.GetBillingAddressAsync(checkouData.EntityId);
                    CheckoutDataViewModels[1] = CreateCheckoutData(updatedBillingAddress, Constants.BillingAddress);
                    _order.BillingAddress = updatedBillingAddress;
                    break;
                case Constants.PaymentMethod:
                    var updatedPaymentMethod = await _checkoutDataRepository.GetPaymentMethodAsync(checkouData.EntityId);
                    CheckoutDataViewModels[2] = CreateCheckoutData(updatedPaymentMethod);
                    _order.PaymentMethod = updatedPaymentMethod;
                    break;
            }
        }

        private CheckoutDataViewModel CreateCheckoutData(Address address, string dataType)
        {
            return new CheckoutDataViewModel()
                {
                    EntityId = address.Id,
                    DataType = dataType,
                    Title = dataType == Constants.ShippingAddress ? _resourceLoader.GetString("ShippingAddress") : _resourceLoader.GetString("BillingAddress"),
                    FirstLine = address.StreetAddress,
                    SecondLine = string.Format(CultureInfo.CurrentUICulture, "{0}, {1} {2}", address.City, address.State, address.ZipCode),
                    BottomLine = string.Format(CultureInfo.CurrentUICulture, "{0} {1}", address.FirstName, address.LastName),
                    LogoUri = dataType == Constants.ShippingAddress ? new Uri(Constants.ShippingAddressLogo, UriKind.Absolute) : new Uri(Constants.BillingAddressLogo, UriKind.Absolute)
                };
        }

        private CheckoutDataViewModel CreateCheckoutData(PaymentMethod paymentMethod)
        {
            return new CheckoutDataViewModel()
                {
                    EntityId = paymentMethod.Id,
                    DataType = Constants.PaymentMethod,
                    Title = _resourceLoader.GetString("PaymentMethod"),
                    FirstLine = string.Format(CultureInfo.CurrentUICulture, _resourceLoader.GetString("CardEndingIn"), paymentMethod.CardNumber.Substring(paymentMethod.CardNumber.Length - 4)),
                    SecondLine = string.Format(CultureInfo.CurrentUICulture, _resourceLoader.GetString("CardExpiringOn"),
                                                string.Format(CultureInfo.CurrentCulture, "{0}/{1}", paymentMethod.ExpirationMonth, paymentMethod.ExpirationYear)),
                    BottomLine = paymentMethod.CardholderName,
                    LogoUri = new Uri(Constants.PaymentMethodLogo, UriKind.Absolute)
                };
        }
    }
}