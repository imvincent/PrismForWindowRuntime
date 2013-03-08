// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Windows.Input;
using AdventureWorks.UILogic.Models;
using Microsoft.Practices.StoreApps.Infrastructure;
using Microsoft.Practices.StoreApps.Infrastructure.Interfaces;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Services;
using System.Net.Http;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AdventureWorks.UILogic.ViewModels
{
    public class PaymentMethodFlyoutViewModel : ViewModel, IFlyoutViewModel
    {
        private readonly IPaymentMethodUserControlViewModel _paymentMethodViewModel;
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private readonly IResourceLoader _resourceLoader;
        private string _headerLabel;
        private Action _successAction;

        public PaymentMethodFlyoutViewModel(IPaymentMethodUserControlViewModel paymentMethodViewModel, ICheckoutDataRepository checkoutDataRepository,
                                            IResourceLoader resourceLoader)
        {
            _paymentMethodViewModel = paymentMethodViewModel;
            _checkoutDataRepository = checkoutDataRepository;
            _resourceLoader = resourceLoader;

            SaveCommand = DelegateCommand.FromAsyncHandler(SaveAsync);
            GoBackCommand = new DelegateCommand(() => GoBack());
        }

        public IPaymentMethodUserControlViewModel PaymentMethodViewModel
        { 
            get { return _paymentMethodViewModel; } 
        }

        public string HeaderLabel
        {
            get { return _headerLabel; }
            set { SetProperty(ref _headerLabel, value); }
        }

        public Action CloseFlyout { get; set; }

        public Action GoBack { get; set; }

        public ICommand SaveCommand { get; set; }

        public ICommand GoBackCommand { get; private set; }

        public async void Open(object parameter, Action successAction)
        {
            _successAction = successAction;
            var paymentMethodId = parameter as string;

            if (string.IsNullOrWhiteSpace(paymentMethodId))
            {
                HeaderLabel = _resourceLoader.GetString("AddPaymentMethodTitle");
            }
            else
            {
                HeaderLabel = _resourceLoader.GetString("EditPaymentMethodTitle");

                // Update PaymentMethod information
                PaymentMethodViewModel.PaymentMethod = await _checkoutDataRepository.GetPaymentMethodAsync(paymentMethodId);
            }
        }

        private async Task SaveAsync()
        {
            if (PaymentMethodViewModel.ValidateForm())
            {
                await PaymentMethodViewModel.ProcessFormAsync();
                CloseFlyout();

                if (_successAction != null)
                {
                    _successAction();
                    _successAction = null;

                }
            }
        }
    }
}