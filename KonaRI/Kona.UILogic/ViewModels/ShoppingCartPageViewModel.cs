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
using Windows.Globalization.NumberFormatting;
using Windows.UI.Xaml.Navigation;
using Kona.UILogic.Models;
using Windows.UI.ViewManagement;

namespace Kona.UILogic.ViewModels
{
    public class ShoppingCartPageViewModel : ViewModel, INavigationAware
    {
        private string _fullPrice;
        private string _totalDiscount;
        private string _totalPrice;
        private ShoppingCartItemViewModel _selectedItem;
        private ObservableCollection<ShoppingCartItemViewModel> _shoppingCartItemViewModels;
        private IShoppingCartRepository _shoppingCartRepository;
        private INavigationService _navigationService;
        private IAccountService _accountService;

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
            set { SetProperty(ref _selectedItem, value); }
        }

        [RestorableState]
        public ObservableCollection<ShoppingCartItemViewModel> ShoppingCartItemViewModels
        {
            get { return _shoppingCartItemViewModels; }
            private set { SetProperty(ref _shoppingCartItemViewModels, value); }
        }

        public DelegateCommand CheckoutCommand { get; private set; }

        public DelegateCommand GoBackCommand { get; private set; }

        public DelegateCommand<ShoppingCartItemViewModel> RemoveCommand { get; private set; }

        public ShoppingCartPageViewModel(IShoppingCartRepository shoppingCartRepository, INavigationService navigationService, IAccountService accountService)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _navigationService = navigationService;
            _accountService = accountService;

            CheckoutCommand = new DelegateCommand(Checkout, CanCheckout);
            RemoveCommand = new DelegateCommand<ShoppingCartItemViewModel>(Remove);
            GoBackCommand = new DelegateCommand(navigationService.GoBack);
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState)
        {
            _accountService.UserChanged += _accountService_UserChanged;
            UpdateShoppingCart();

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
            UpdateShoppingCart();
        }

        private async void UpdateShoppingCart()
        {
            var shoppingCart = await _shoppingCartRepository.GetShoppingCartAsync();

            if (shoppingCart != null)
            {
                var currencyFormatter = new CurrencyFormatter(shoppingCart.Currency);
                FullPrice = currencyFormatter.FormatDouble(shoppingCart.FullPrice);
                TotalDiscount = currencyFormatter.FormatDouble(shoppingCart.TotalDiscount);
                TotalPrice = currencyFormatter.FormatDouble(shoppingCart.FullPrice - shoppingCart.TotalDiscount);

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
        }

        public async void Checkout()
        {
            var navigationServiceReference = _navigationService;
            Action navigateAction = () => navigationServiceReference.Navigate("CheckoutHub", null);

            if ( await _accountService.CheckIfUserSignedIn() != null)
            {
                navigateAction();
            }
            else
            {
                if (ApplicationView.Value == ApplicationViewState.Snapped)
                {
                    ApplicationView.TryUnsnap();
                }
                _accountService.DisplaySignIn(navigateAction, null);
            }
        }

        public bool CanCheckout()
        {
            return _shoppingCartItemViewModels != null && _shoppingCartItemViewModels.Count > 0;
        }

        public async void Remove(ShoppingCartItemViewModel item)
        {
            await _shoppingCartRepository.RemoveShoppingCartItemAsync(item.Id.ToString());
            ShoppingCartItemViewModels.Remove(item);
        }
    }
}
