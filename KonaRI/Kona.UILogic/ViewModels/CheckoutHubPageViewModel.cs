// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using Kona.Infrastructure;
using Kona.UILogic.Repositories;

namespace Kona.UILogic.ViewModels
{
    public class CheckoutHubPageViewModel : ViewModel, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IShippingAddressUserControlViewModel _shippingAddressPageViewModel;
        private readonly IBillingAddressUserControlViewModel _billingAddressPageViewModel;
        private readonly IPaymentMethodUserControlViewModel _paymentMethodPageViewModel;

        public CheckoutHubPageViewModel(INavigationService navigationService, 
                                        IShoppingCartRepository shoppingCartRepository,
                                        IShippingAddressUserControlViewModel shippingAddressUserControlViewModel, 
                                        IBillingAddressUserControlViewModel billingAddressUserControlViewModel, 
                                        IPaymentMethodUserControlViewModel paymentMethodUserControlViewModel)
        {
            _navigationService = navigationService;
            _shoppingCartRepository = shoppingCartRepository;
            _shippingAddressPageViewModel = shippingAddressUserControlViewModel;
            _billingAddressPageViewModel = billingAddressUserControlViewModel;
            _paymentMethodPageViewModel = paymentMethodUserControlViewModel;

            GoBackCommand = new DelegateCommand(() => navigationService.GoBack(), () => navigationService.CanGoBack());
            GoNextCommand = new DelegateCommand(() => GoNext());
            UseSameAsShippingAddressAction = UseSameAddressAsShipping;

        }

        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand GoNextCommand { get; private set; }
        public Action<object> UseSameAsShippingAddressAction { get; private set; }

        public IShippingAddressUserControlViewModel ShippingAddressPageViewModel { get { return _shippingAddressPageViewModel; } }
        public IBillingAddressUserControlViewModel BillingAddressPageViewModel { get { return _billingAddressPageViewModel; } }
        public IPaymentMethodUserControlViewModel PaymentMethodPageViewModel { get { return _paymentMethodPageViewModel; } }

        public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, System.Collections.Generic.Dictionary<string, object> viewState)
        {
            ShippingAddressPageViewModel.OnNavigatedTo(navigationParameter, navigationMode, viewState);
            BillingAddressPageViewModel.OnNavigatedTo(navigationParameter, navigationMode, viewState);
            PaymentMethodPageViewModel.OnNavigatedTo(navigationParameter, navigationMode, viewState);
            base.OnNavigatedTo(navigationParameter, navigationMode, viewState);
        }

        public override void OnNavigatedFrom(System.Collections.Generic.Dictionary<string, object> viewState, bool suspending)
        {
            ShippingAddressPageViewModel.OnNavigatedFrom(viewState, suspending);
            BillingAddressPageViewModel.OnNavigatedFrom(viewState, suspending);
            PaymentMethodPageViewModel.OnNavigatedFrom(viewState, suspending);
            base.OnNavigatedFrom(viewState, suspending);
        }

        private async void GoNext()
        {
            var shippingAddressValid = await ShippingAddressPageViewModel.ValidateFormAsync();
            var billingAddressValid = await BillingAddressPageViewModel.ValidateFormAsync();
            var paymentMethodValid = await PaymentMethodPageViewModel.ValidateFormAsync();

            if (shippingAddressValid && billingAddressValid && paymentMethodValid)
            {
                ShippingAddressPageViewModel.ProcessForm();
                BillingAddressPageViewModel.ProcessForm();
                PaymentMethodPageViewModel.ProcessForm();
                
                _shoppingCartRepository.AddAddressAndPurchaseInfo(ShippingAddressPageViewModel.Address, BillingAddressPageViewModel.GetAddress(), PaymentMethodPageViewModel.GetPaymentInfo());
                _navigationService.Navigate("CheckoutSummary", null);
            }
        }

        private void UseSameAddressAsShipping(object parameter)
        {
            BillingAddressPageViewModel.FirstName = ShippingAddressPageViewModel.Address.FirstName;
            BillingAddressPageViewModel.MiddleInitial = ShippingAddressPageViewModel.Address.MiddleInitial;
            BillingAddressPageViewModel.LastName = ShippingAddressPageViewModel.Address.LastName;
            BillingAddressPageViewModel.StreetAddress = ShippingAddressPageViewModel.Address.StreetAddress;
            BillingAddressPageViewModel.OptionalAddress = ShippingAddressPageViewModel.Address.OptionalAddress;
            BillingAddressPageViewModel.City = ShippingAddressPageViewModel.Address.City;
            BillingAddressPageViewModel.State = ShippingAddressPageViewModel.Address.State;
            BillingAddressPageViewModel.ZipCode = ShippingAddressPageViewModel.Address.ZipCode;
            BillingAddressPageViewModel.Phone = ShippingAddressPageViewModel.Address.Phone;
        }
    }
}
