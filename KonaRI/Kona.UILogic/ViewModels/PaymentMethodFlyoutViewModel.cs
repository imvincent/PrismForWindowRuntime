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
    public class PaymentMethodFlyoutViewModel : ViewModel, IFlyoutViewModel
    {
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private readonly IPaymentMethodUserControlViewModel _viewModel;

        public PaymentMethodFlyoutViewModel(IPaymentMethodUserControlViewModel paymentMethodPageViewModel, ICheckoutDataRepository checkoutDataRepository)
        {
            _checkoutDataRepository = checkoutDataRepository;
            _viewModel = paymentMethodPageViewModel;
            AddCommand = new DelegateCommand(AddPaymentInfo);
        }

        public IPaymentMethodUserControlViewModel PaymentMethodPageViewModel { get { return _viewModel; } }
        public ICommand AddCommand { get; set; }
        public Action CloseFlyout { get; set; }
        public Action GoBack { get; set; }

        public void Open() { }

        private async void AddPaymentInfo()
        {
            if (await PaymentMethodPageViewModel.ValidateFormAsync())
            {
                _checkoutDataRepository.SavePaymentInfo(PaymentMethodPageViewModel.GetPaymentInfo());
                CloseFlyout();
                //TODO: Set this as the payment info to use
            }
        }
    }
}