// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Windows.Input;
using Kona.Infrastructure;
using Kona.Infrastructure.Flyouts;
using Kona.UILogic.Repositories;

namespace Kona.UILogic.ViewModels
{
    public class BillingAddressFlyoutViewModel : ViewModel, IFlyoutViewModel
    {
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private readonly IBillingAddressUserControlViewModel _viewModel;

        public BillingAddressFlyoutViewModel(IBillingAddressUserControlViewModel billingAddressUserControlViewModel, ICheckoutDataRepository checkoutDataRepository)
        {
            _checkoutDataRepository = checkoutDataRepository;
            _viewModel = billingAddressUserControlViewModel;
            AddCommand = new DelegateCommand(AddBillingAddress);
        }

        public IBillingAddressUserControlViewModel BillingAddressUserControlViewModel { get { return _viewModel; } }
        public ICommand AddCommand { get; set; }
        public Action CloseFlyout { get; set; }
        public Action GoBack { get; set; }

        public void Open() { }

        private async void AddBillingAddress()
        {
            if (await BillingAddressUserControlViewModel.ValidateFormAsync())
            {
                _checkoutDataRepository.SaveBillingAddress(BillingAddressUserControlViewModel.GetAddress());
                CloseFlyout();
                //TODO: Set this as the Billing Address to use
            }
        }
    }
}