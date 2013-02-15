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
using Kona.Infrastructure.Interfaces;
using Kona.UILogic.Events;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Kona.UILogic.Services;
using Windows.Globalization.NumberFormatting;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.PubSubEvents;
using System.Threading.Tasks;
using System.Net.Http;

namespace Kona.UILogic.ViewModels
{
    public class ShoppingCartPageViewModel : ViewModel, INavigationAware, IHandleWindowSizeChanged
    {
        private ShoppingCartItemViewModel _selectedItem;
        private ObservableCollection<ShoppingCartItemViewModel> _shoppingCartItemViewModels;
        private IShoppingCartRepository _shoppingCartRepository;
        private IAccountService _accountService;
        private readonly ISettingsCharmService _settingsCharmService;
        private readonly IResourceLoader _resourceLoader;
        private readonly IAlertMessageService _alertMessageService;
        private Action _navigateAction;
        private bool _isUnsnapping = false;
        private bool _isEditPopupOpened;
        private bool _isBottomAppBarOpened;
        private ShoppingCart _shoppingCart;

        public ShoppingCartPageViewModel(IShoppingCartRepository shoppingCartRepository,
                                         INavigationService navigationService,
                                         IAccountService accountService,
                                         ISettingsCharmService settingsCharmService,
                                         IEventAggregator eventAggregator,
                                         IResourceLoader resourceLoader,
                                         IAlertMessageService alertMessageService)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _accountService = accountService;
            _settingsCharmService = settingsCharmService;
            _resourceLoader = resourceLoader;
            _alertMessageService = alertMessageService;
            if (eventAggregator != null)
            {
                eventAggregator.GetEvent<ShoppingCartUpdatedEvent>().Subscribe(UpdateShoppingCartAsync);
            }
            CheckoutCommand = DelegateCommand.FromAsyncHandler(Checkout, CanCheckout);
            EditAmountCommand = new DelegateCommand(OpenEditAmountFlyout);
            RemoveCommand = DelegateCommand<ShoppingCartItemViewModel>.FromAsyncHandler(Remove);
            GoBackCommand = new DelegateCommand(navigationService.GoBack);
            IncrementCountCommand = DelegateCommand.FromAsyncHandler(IncrementCount);
            DecrementCountCommand = DelegateCommand.FromAsyncHandler(DecrementCount, CanDecrementCount);
            var navigationServiceReference = navigationService;
            _navigateAction = () => navigationServiceReference.Navigate("CheckoutHub", null);
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

        public ShoppingCartItemViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (SetProperty(ref _selectedItem, value))
                {
                    if (_selectedItem != null)
                    {
                        IncrementCountCommand.RaiseCanExecuteChanged();
                        DecrementCountCommand.RaiseCanExecuteChanged();
                        IsBottomAppBarOpened = true;
                    }
                }
            }
        }

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
            set { SetProperty(ref _isBottomAppBarOpened, value); }
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState)
        {
            UpdateShoppingCartAsync(null);

            base.OnNavigatedTo(navigationParameter, navigationMode, viewState);
        }

        public async void UpdateShoppingCartAsync(object notUsed)
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

        void shoppingCartItemViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Quantity")
            {
                OnPropertyChanged("FullPrice");
                OnPropertyChanged("TotalDiscount");
                OnPropertyChanged("TotalPrice");
            }
        }

        private async Task Checkout()
        {
            try
            {
                if (await _accountService.GetSignedInUserAsync() == null)
                {
                    if (ApplicationView.Value == ApplicationViewState.Snapped)
                    {
                        _isUnsnapping = true;
                        ApplicationView.TryUnsnap();
                    }
                    else
                    {
                        _settingsCharmService.ShowFlyout("signIn", null, _navigateAction);
                        _navigateAction = null;
                    }
                }
                else
                {
                    _navigateAction();
                    _navigateAction = null;
                }

            }
            catch (HttpRequestException)
            {
                var task = _alertMessageService.ShowAsync(_resourceLoader.GetString("ErrorServiceUnreachable"), _resourceLoader.GetString("Error"));
            }
        }

        public void WindowCurrentSizeChanged()
        {
            if (_isUnsnapping)
            {
                _settingsCharmService.ShowFlyout("signIn", null, _navigateAction);
                _isUnsnapping = false;
                _navigateAction = null;
            }
        }

        private bool CanCheckout()
        {
            return _shoppingCartItemViewModels != null && _shoppingCartItemViewModels.Count > 0;
        }

        public async Task Remove(ShoppingCartItemViewModel item)
        {
            if (item != null)
            {
                await _shoppingCartRepository.RemoveShoppingCartItemAsync(item.Id);
            }
            ShoppingCartItemViewModels.Remove(item);
            CheckoutCommand.RaiseCanExecuteChanged();
            OnPropertyChanged("FullPrice");
            OnPropertyChanged("TotalDiscount");
            OnPropertyChanged("TotalPrice");
        }

        private void OpenEditAmountFlyout()
        {
            IsEditPopupOpened = true;
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
            SelectedItem.Quantity = SelectedItem.Quantity - 1;
            DecrementCountCommand.RaiseCanExecuteChanged();
            await _shoppingCartRepository.RemoveProductFromShoppingCartAsync(SelectedItem.ProductId);
        }

        private async Task IncrementCount()
        {
            SelectedItem.Quantity = SelectedItem.Quantity + 1;
            DecrementCountCommand.RaiseCanExecuteChanged();
            await _shoppingCartRepository.AddProductToShoppingCartAsync(SelectedItem.ProductId);
        }
    }
}
