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

        public PaymentMethodFlyoutViewModel(IPaymentMethodUserControlViewModel paymentMethodUserControlViewModel, ICheckoutDataRepository checkoutDataRepository)
        {
            _checkoutDataRepository = checkoutDataRepository;
            _viewModel = paymentMethodUserControlViewModel;
            AddCommand = new DelegateCommand(AddPaymentInfo);
            GoBackCommand = new DelegateCommand(() => GoBack(), () => true);
        }

        public IPaymentMethodUserControlViewModel PaymentMethodPageViewModel { get { return _viewModel; } }
        public ICommand AddCommand { get; set; }
        public Action CloseFlyout { get; set; }
        public Action GoBack { get; set; }
        public ICommand GoBackCommand { get; private set; }

        public void Open(object parameter, Action successAction) { }

        private void AddPaymentInfo()
        {
            if (PaymentMethodPageViewModel.ValidateForm())
            {
                _checkoutDataRepository.SavePaymentInfo(PaymentMethodPageViewModel.PaymentInfo);
                CloseFlyout();
                //TODO: Set this as the payment info to use
            }
        }
    }
}