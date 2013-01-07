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
    public class ShippingAddressFlyoutViewModel : ViewModel, IFlyoutViewModel
    {
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private readonly IShippingAddressUserControlViewModel _viewModel;

        public ShippingAddressFlyoutViewModel(IShippingAddressUserControlViewModel shippingAddressUserControlViewModel, ICheckoutDataRepository checkoutDataRepository)
        {
            _checkoutDataRepository = checkoutDataRepository;
            _viewModel = shippingAddressUserControlViewModel;
            AddCommand = new DelegateCommand(AddShippingAddress);
        }

        public IShippingAddressUserControlViewModel ShippingAddressUserControlViewModel { get { return _viewModel; } }
        public ICommand AddCommand { get; set; }
        public Action CloseFlyout { get; set; }
        public Action GoBack { get; set; }

        public void Open() { }

        private async void AddShippingAddress()
        {
            if (await ShippingAddressUserControlViewModel.ValidateFormAsync())
            {
                _checkoutDataRepository.SaveShippingAddress(ShippingAddressUserControlViewModel.Address);
                CloseFlyout();
                //TODO: Set this as the shipping address to use
            }
        }
    }
}