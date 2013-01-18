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
using Kona.UILogic.Services;
using Kona.UILogic.Repositories;
using Windows.ApplicationModel.Resources;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Kona.UILogic.Models;
using Windows.UI.ViewManagement;

namespace Kona.UILogic.ViewModels
{
    public class ShoppingCartPageViewModel : ViewModel, INavigationAware, IHandleWindowSizeChanged
    {
        private string _fullPrice;
        private string _totalDiscount;
        private string _totalPrice;
        private ShoppingCartItemViewModel _selectedItem;
        private ObservableCollection<ShoppingCartItemViewModel> _shoppingCartItemViewModels;
        private IShoppingCartRepository _shoppingCartRepository;
        private IAccountService _accountService;
        private readonly ISettingsCharmService _settingsCharmService;
        private readonly Action _navigateAction;
        private bool _isUnsnapping = false;
        private bool _isEditPopupOpened;
        private bool _isBottomAppBarOpened;

        public ShoppingCartPageViewModel(IShoppingCartRepository shoppingCartRepository,
                                         INavigationService navigationService,
                                         IAccountService accountService,
                                         ISettingsCharmService settingsCharmService)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _accountService = accountService;
            _settingsCharmService = settingsCharmService;

            CheckoutCommand = new DelegateCommand(Checkout, CanCheckout);
            EditAmountCommand = new DelegateCommand(OpenEditAmountFlyout);
            RemoveCommand = new DelegateCommand<ShoppingCartItemViewModel>(Remove);
            GoBackCommand = new DelegateCommand(navigationService.GoBack);
            IncrementCountCommand = new DelegateCommand(IncrementCount);
            DecrementCountCommand = new DelegateCommand(DecrementCount, CanDecrementCount);
            var navigationServiceReference = navigationService;
            _navigateAction = () => navigationServiceReference.Navigate("CheckoutHub", null);
        }

        public string FullPrice
        {
            get { return _fullPrice; }
            set { SetProperty(ref _fullPrice, value); }
        }

        public string TotalDiscount
        {
            get { return _totalDiscount; }
            set { SetProperty(ref _totalDiscount, value); }
        }

        public string TotalPrice
        {
            get { return _totalPrice; }
            set { SetProperty(ref _totalPrice, value); }
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
            _accountService.UserChanged += _accountService_UserChanged;
            UpdateShoppingCartAsync();

            base.OnNavigatedTo(navigationParameter, navigationMode, viewState);
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewState, bool suspending)
        {
            if (!suspending)
            {
                _accountService.UserChanged -= _accountService_UserChanged;
            }
            base.OnNavigatedFrom(viewState, suspending);
        }

        void _accountService_UserChanged(object sender, UserChangedEventArgs e)
        {
            UpdateShoppingCartAsync();
        }

        private async void UpdateShoppingCartAsync()
        {
            var shoppingCart = await _shoppingCartRepository.GetShoppingCartAsync();

            if (shoppingCart != null)
            {
                var currencyFormatter = new CurrencyFormatter(shoppingCart.Currency);
                FullPrice = currencyFormatter.FormatDouble(Math.Round(shoppingCart.FullPrice, 2));
                TotalDiscount = currencyFormatter.FormatDouble(Math.Round(shoppingCart.TotalDiscount, 2));
                TotalPrice = currencyFormatter.FormatDouble(Math.Round(shoppingCart.FullPrice - shoppingCart.TotalDiscount, 2));

                if (shoppingCart.ShoppingCartItems != null)
                {
                    ShoppingCartItemViewModels = new ObservableCollection<ShoppingCartItemViewModel>();
                    foreach (var item in shoppingCart.ShoppingCartItems)
                    {
                        ShoppingCartItemViewModels.Add(new ShoppingCartItemViewModel(item));
                    }
                    CheckoutCommand.RaiseCanExecuteChanged();
                }
            }
            else
            {
                FullPrice = string.Empty;
                TotalDiscount = string.Empty;
                TotalPrice = string.Empty;
                ShoppingCartItemViewModels = null;
            }
        }

        public async void Checkout()
        {
            if (await _accountService.GetSignedInUserAsync() != null)
            {
                _navigateAction();
            }
            else
            {
                if (ApplicationView.Value == ApplicationViewState.Snapped)
                {
                    _isUnsnapping = true;
                    ApplicationView.TryUnsnap();
                }
                _settingsCharmService.ShowFlyout("signIn", null, _navigateAction);
            }
        }

        public void WindowCurrentSizeChanged()
        {
            if (_isUnsnapping)
            {
                _settingsCharmService.ShowFlyout("signIn", null, _navigateAction);
                _isUnsnapping = false;
            }
        }

        public bool CanCheckout()
        {
            return _shoppingCartItemViewModels != null && _shoppingCartItemViewModels.Count > 0;
        }

        public void Remove(ShoppingCartItemViewModel item)
        {
            if (item != null)
            {
                _shoppingCartRepository.RemoveShoppingCartItemAsync(item.Id);
            }
            ShoppingCartItemViewModels.Remove(item);
        }

        private void OpenEditAmountFlyout()
        {
            IsEditPopupOpened = true;
        }

        private bool CanDecrementCount()
        {
            if (SelectedItem != null && SelectedItem.Quantity > 0)
            {
                return true;
            }
            return false;
        }

        private void DecrementCount()
        {
            SelectedItem.Quantity = SelectedItem.Quantity - 1;
            DecrementCountCommand.RaiseCanExecuteChanged();
        }

        private void IncrementCount()
        {
            SelectedItem.Quantity = SelectedItem.Quantity + 1;
            DecrementCountCommand.RaiseCanExecuteChanged();
        }
    }
}
