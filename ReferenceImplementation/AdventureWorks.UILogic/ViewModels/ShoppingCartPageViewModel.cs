// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Services;
using Microsoft.Practices.StoreApps.Infrastructure;
using Microsoft.Practices.StoreApps.Infrastructure.Interfaces;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.PubSubEvents;
using System.Threading.Tasks;
using System.Net.Http;
using System.Globalization;

namespace AdventureWorks.UILogic.ViewModels
{
    public class ShoppingCartPageViewModel : ViewModel
    {
        private ShoppingCart _shoppingCart;
        private ShoppingCartItemViewModel _selectedItem;
        private ObservableCollection<ShoppingCartItemViewModel> _shoppingCartItemViewModels;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly INavigationService _navigationService;
        private readonly IAccountService _accountService;
        private readonly IFlyoutService _flyoutService;
        private readonly IResourceLoader _resourceLoader;
        private readonly IAlertMessageService _alertMessageService;
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private readonly IOrderRepository _orderRepository;
        private bool _isEditPopupOpened;
        private bool _isBottomAppBarOpened;

        public ShoppingCartPageViewModel(IShoppingCartRepository shoppingCartRepository, INavigationService navigationService, IAccountService accountService,
                                         IFlyoutService flyoutService, IResourceLoader resourceLoader, IAlertMessageService alertMessageService,
                                         ICheckoutDataRepository checkoutDataRepository, IOrderRepository orderRepository, IEventAggregator eventAggregator)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _navigationService = navigationService;
            _accountService = accountService;
            _flyoutService = flyoutService;
            _resourceLoader = resourceLoader;
            _alertMessageService = alertMessageService;
            _checkoutDataRepository = checkoutDataRepository;
            _orderRepository = orderRepository;

            CheckoutCommand = DelegateCommand.FromAsyncHandler(CheckoutAsync, CanCheckout);
            EditAmountCommand = new DelegateCommand(OpenEditAmountFlyout);
            RemoveCommand = DelegateCommand<ShoppingCartItemViewModel>.FromAsyncHandler(Remove);
            GoBackCommand = new DelegateCommand(navigationService.GoBack);
            IncrementCountCommand = DelegateCommand.FromAsyncHandler(IncrementCount);
            DecrementCountCommand = DelegateCommand.FromAsyncHandler(DecrementCount, CanDecrementCount);

            // Subscribe to the ShoppingCartUpdated event
            if (eventAggregator != null)
            {
                eventAggregator.GetEvent<ShoppingCartUpdatedEvent>().Subscribe(UpdateShoppingCartAsync);
            }
        }

        public string FullPrice
        {
            get
            {
                if (_shoppingCart == null || _shoppingCart.Currency == null) return string.Empty;
                var currencyFormatter = new CurrencyFormatter(_shoppingCart.Currency);
                return currencyFormatter.FormatDouble(Math.Round(CalculateFullPrice(), 2));
            }
        }

        public string TotalDiscount
        {
            get
            {
                if (_shoppingCart == null || _shoppingCart.Currency == null) return string.Empty;
                var currencyFormatter = new CurrencyFormatter(_shoppingCart.Currency);
                return currencyFormatter.FormatDouble(Math.Round(CalculateDiscount(), 2));
            }
        }

        public string TotalPrice
        {
            get
            {
                if (_shoppingCart == null || _shoppingCart.Currency == null) return string.Empty;
                var currencyFormatter = new CurrencyFormatter(_shoppingCart.Currency);
                return currencyFormatter.FormatDouble(Math.Round(CalculateFullPrice() - CalculateDiscount(), 2));
            }
        }

        // <snippet610>
        public ShoppingCartItemViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (SetProperty(ref _selectedItem, value))
                {
                    
                    if (_selectedItem != null)
                    {
                        // Display the AppBar 
                        IsBottomAppBarOpened = true;

                        IncrementCountCommand.RaiseCanExecuteChanged();
                        DecrementCountCommand.RaiseCanExecuteChanged();
                    }
                }
            }
        }
        // </snippet610>

        public ObservableCollection<ShoppingCartItemViewModel> ShoppingCartItemViewModels
        {
            get { return _shoppingCartItemViewModels; }
            private set { SetProperty(ref _shoppingCartItemViewModels, value); }
        }

        public DelegateCommand CheckoutCommand { get; private set; }

        public DelegateCommand EditAmountCommand { get; private set; }

        public DelegateCommand GoBackCommand { get; private set; }

        public DelegateCommand<ShoppingCartItemViewModel> RemoveCommand { get; private set; }

        public DelegateCommand IncrementCountCommand { get; private set; }

        public DelegateCommand DecrementCountCommand { get; private set; }

        public bool IsEditPopupOpened
        {
            get { return _isEditPopupOpened; }
            set { SetProperty(ref _isEditPopupOpened, value); }
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

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            await UpdateShoppingCartInfoAsync();
        }

        public async void UpdateShoppingCartAsync(object notUsed)
        {
            await UpdateShoppingCartInfoAsync();
        }

        private async Task UpdateShoppingCartInfoAsync()
        {
            string errorMessage = string.Empty;
            
            try
            {
                _shoppingCart = await _shoppingCartRepository.GetShoppingCartAsync();

                if (_shoppingCart != null && _shoppingCart.ShoppingCartItems != null)
                {
                    ShoppingCartItemViewModels = new ObservableCollection<ShoppingCartItemViewModel>();
                    foreach (var item in _shoppingCart.ShoppingCartItems)
                    {
                        var shoppingCartItemViewModel = new ShoppingCartItemViewModel(item);
                        shoppingCartItemViewModel.PropertyChanged += shoppingCartItemViewModel_PropertyChanged;
                        ShoppingCartItemViewModels.Add(shoppingCartItemViewModel);
                    }
                    CheckoutCommand.RaiseCanExecuteChanged();
                    OnPropertyChanged("FullPrice");
                    OnPropertyChanged("TotalDiscount");
                    OnPropertyChanged("TotalPrice");
                }
            }
            catch (HttpRequestException ex)
            {
                errorMessage = string.Format(CultureInfo.CurrentCulture, _resourceLoader.GetString("GeneralServiceErrorMessage"), Environment.NewLine, ex.Message);
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                await _alertMessageService.ShowAsync(errorMessage, _resourceLoader.GetString("ErrorServiceUnreachable"));
            }
        }

        private void shoppingCartItemViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Quantity")
            {
                OnPropertyChanged("FullPrice");
                OnPropertyChanged("TotalDiscount");
                OnPropertyChanged("TotalPrice");
            }
        }

        private bool CanCheckout()
        {
            return _shoppingCartItemViewModels != null && _shoppingCartItemViewModels.Count > 0;
        }

        private async Task CheckoutAsync()
        {
            string errorMessage = string.Empty;
            try
            {
                if (await _accountService.VerifyUserAuthenticationAsync() == null)
                {
                    // Set up navigate action depending on the application's state
                    var navigateAction = await ResolveNavigationActionAsync();
                    _flyoutService.ShowFlyout("SignIn", null, navigateAction);
                }
                else
                {
                    // Set up navigate action depending on the application's state
                    var navigateAction = await ResolveNavigationActionAsync();

                    // Execute the navigate action
                    navigateAction();
                }
            }
            catch (HttpRequestException ex)
            {
                errorMessage = string.Format(CultureInfo.CurrentCulture, _resourceLoader.GetString("GeneralServiceErrorMessage"), Environment.NewLine, ex.Message);
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                await _alertMessageService.ShowAsync(errorMessage, _resourceLoader.GetString("ErrorServiceUnreachable"));
            }
        }

        private async Task<Action> ResolveNavigationActionAsync()
        {
            Action navigateAction = null;
            var navigationServiceReference = _navigationService;

            // Retrieve the default information for the Order
            var defaultShippingAddress = _checkoutDataRepository.GetDefaultShippingAddress();
            var defaultBillingAddress = _checkoutDataRepository.GetDefaultBillingAddress();
            var defaultPaymentMethod = await _checkoutDataRepository.GetDefaultPaymentMethodAsync();

            if (defaultShippingAddress == null || defaultBillingAddress == null || defaultPaymentMethod == null)
            {
                // Navigate to the CheckoutHubPage to fill the missing default information
                navigateAction = () => navigationServiceReference.Navigate("CheckoutHub", null);
            }
            else
            {
                // Create the Order with the current information. Then navigate to the CheckoutSummaryPage
                var shoppingCartReference = _shoppingCart;
                var orderRepositoryReference = _orderRepository;
                var accountServiceReference = _accountService;
                navigateAction = async () =>
                    {
                        var user = accountServiceReference.SignedInUser;
                        await orderRepositoryReference.CreateBasicOrderAsync(user.UserName, shoppingCartReference,
                                                                                   defaultShippingAddress,
                                                                                   defaultBillingAddress,
                                                                                   defaultPaymentMethod);

                        navigationServiceReference.Navigate("CheckoutSummary", null);
                    };
            }

            return navigateAction;
        }

        private async Task Remove(ShoppingCartItemViewModel item)
        {
            if (item == null) return;

            string errorMessage = string.Empty;
            try
            {
                // Hide the AppBar
                IsBottomAppBarOpened = false;

                await _shoppingCartRepository.RemoveShoppingCartItemAsync(item.Id);
                ShoppingCartItemViewModels.Remove(item);

                CheckoutCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("FullPrice");
                OnPropertyChanged("TotalDiscount");
                OnPropertyChanged("TotalPrice");
            }
            catch (HttpRequestException ex)
            {
                errorMessage = string.Format(CultureInfo.CurrentCulture, _resourceLoader.GetString("GeneralServiceErrorMessage"), Environment.NewLine, ex.Message);
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                await _alertMessageService.ShowAsync(errorMessage, _resourceLoader.GetString("ErrorServiceUnreachable"));
            }
        }

        private void OpenEditAmountFlyout()
        {
            IsEditPopupOpened = true;
        }

        private double CalculateFullPrice()
        {
            double fullPrice = 0;
            foreach (var shoppingCartItemViewModel in _shoppingCartItemViewModels)
            {
                fullPrice += shoppingCartItemViewModel.FullPriceDouble;
            }
            return fullPrice;
        }

        private double CalculateDiscount()
        {
            double discount = 0;
            foreach (var shoppingCartItemViewModel in _shoppingCartItemViewModels)
            {
                discount += shoppingCartItemViewModel.FullPriceDouble - shoppingCartItemViewModel.DiscountedPriceDouble;
            }
            return discount;
        }

        private bool CanDecrementCount()
        {
            if (SelectedItem != null && SelectedItem.Quantity > 1)
            {
                return true;
            }
            return false;
        }

        private async Task DecrementCount()
        {
            string errorMessage = string.Empty;

            try
            {
                await _shoppingCartRepository.RemoveProductFromShoppingCartAsync(SelectedItem.ProductId);
                SelectedItem.Quantity -= 1;
                DecrementCountCommand.RaiseCanExecuteChanged();
            }
            catch (HttpRequestException ex)
            {
                errorMessage = string.Format(CultureInfo.CurrentCulture, _resourceLoader.GetString("GeneralServiceErrorMessage"), Environment.NewLine, ex.Message);
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                await _alertMessageService.ShowAsync(errorMessage, _resourceLoader.GetString("ErrorServiceUnreachable"));
            }
        }

        private async Task IncrementCount()
        {
            string errorMessage = string.Empty;

            try
            {
                await _shoppingCartRepository.AddProductToShoppingCartAsync(SelectedItem.ProductId);
                SelectedItem.Quantity += 1;
                DecrementCountCommand.RaiseCanExecuteChanged();
            }
            catch (HttpRequestException ex)
            {
                errorMessage = string.Format(CultureInfo.CurrentCulture, _resourceLoader.GetString("GeneralServiceErrorMessage"), Environment.NewLine, ex.Message);
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                await _alertMessageService.ShowAsync(errorMessage, _resourceLoader.GetString("ErrorServiceUnreachable"));
            }
        }
    }
}
