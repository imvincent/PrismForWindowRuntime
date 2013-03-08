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
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Globalization;

namespace AdventureWorks.UILogic.ViewModels
{
    public class BillingAddressFlyoutViewModel : ViewModel, IFlyoutViewModel
    {
        private readonly IBillingAddressUserControlViewModel _billingAddressViewModel;
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private readonly IResourceLoader _resourceLoader;
        private readonly IValidationService _validationService;
        private readonly IAlertMessageService _alertMessageService;
        private string _headerLabel;
        private Action _successAction;

        public BillingAddressFlyoutViewModel(IBillingAddressUserControlViewModel billingAddressViewModel, ICheckoutDataRepository checkoutDataRepository,
                                              IResourceLoader resourceLoader, IValidationService validationService, IAlertMessageService alertMessageService)
        {
            _billingAddressViewModel = billingAddressViewModel;
            _checkoutDataRepository = checkoutDataRepository;
            _resourceLoader = resourceLoader;
            _validationService = validationService;
            _alertMessageService = alertMessageService;

            SaveCommand = DelegateCommand.FromAsyncHandler(SaveAsync);
            GoBackCommand = new DelegateCommand(() => GoBack());
        }

        public IBillingAddressUserControlViewModel BillingAddressViewModel
        {
            get { return _billingAddressViewModel; } 
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
            var addressId = parameter as string;

            _successAction = successAction;
            HeaderLabel = string.IsNullOrWhiteSpace(addressId) 
                              ? _resourceLoader.GetString("AddBillingAddressTitle")
                              : _resourceLoader.GetString("EditBillingAddressTitle");;

            await BillingAddressViewModel.PopulateStatesAsync();

            if (addressId != null)
            {
                BillingAddressViewModel.Address = _checkoutDataRepository.GetBillingAddress(addressId);
            }
        }

        private async Task SaveAsync()
        {
            if (BillingAddressViewModel.ValidateForm())
            {
                string errorMessage = string.Empty;

                try
                {
                    bool result = await _validationService.ValidateAddressAsync(BillingAddressViewModel.Address);

                    if (result)
                    {
                        BillingAddressViewModel.ProcessForm();
                        CloseFlyout();

                        if (_successAction != null)
                        {
                            _successAction();
                            _successAction = null;
                        }
                    }
                }
                catch (ModelValidationException mvex)
                {
                    DisplayValidationErrors(mvex.ValidationResult);
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

        private void DisplayValidationErrors(ModelValidationResult modelValidationResults)
        {
            var errors = new Dictionary<string, ReadOnlyCollection<string>>();

            // Property keys format: address.{Propertyname}
            foreach (var propkey in modelValidationResults.ModelState.Keys)
            {
                string propertyName = propkey.Substring(propkey.IndexOf('.') + 1); // strip off order. prefix

                errors.Add(propertyName, new ReadOnlyCollection<string>(modelValidationResults.ModelState[propkey]));
            }

            if (errors.Count > 0) BillingAddressViewModel.Address.Errors.SetAllErrors(errors);
        }
    }
}
