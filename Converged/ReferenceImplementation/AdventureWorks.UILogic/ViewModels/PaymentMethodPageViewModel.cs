// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Windows.Input;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Services;
using System;
using System.Globalization;
using AdventureWorks.UILogic.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AdventureWorks.UILogic.ViewModels
{
    public class PaymentMethodPageViewModel : ViewModel
    {
        private readonly IPaymentMethodUserControlViewModel _paymentMethodViewModel;
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private readonly IAlertMessageService _alertMessageService;
        private readonly IResourceLoader _resourceLoader;
        private readonly IAccountService _accountService;
        private readonly INavigationService _navigationService;
        private string _headerLabel;

        public PaymentMethodPageViewModel(IPaymentMethodUserControlViewModel paymentMethodViewModel, ICheckoutDataRepository checkoutDataRepository,
                                            IResourceLoader resourceLoader, IAlertMessageService alertMessageService, IAccountService accountService, INavigationService navigationService)
        {
            _paymentMethodViewModel = paymentMethodViewModel;
            _checkoutDataRepository = checkoutDataRepository;
            _resourceLoader = resourceLoader;
            _alertMessageService = alertMessageService;
            _accountService = accountService;
            _navigationService = navigationService;

            SaveCommand = DelegateCommand.FromAsyncHandler(SaveAsync);
        }

        public IPaymentMethodUserControlViewModel PaymentMethodViewModel
        { 
            get { return _paymentMethodViewModel; } 
        }

        public string HeaderLabel
        {
            get { return _headerLabel; }
            private set { SetProperty(ref _headerLabel, value); }
        }

        public ICommand SaveCommand { get; private set; }

        public override async void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, System.Collections.Generic.Dictionary<string, object> viewModelState)
        {
            if (await _accountService.VerifyUserAuthenticationAsync() == null) return;

            var paymentMethodId = navigationParameter as string;

            HeaderLabel = string.IsNullOrWhiteSpace(paymentMethodId)
                  ? _resourceLoader.GetString("AddPaymentMethodTitle")
                  : _resourceLoader.GetString("EditPaymentMethodTitle");

            if (!string.IsNullOrWhiteSpace(paymentMethodId))
            {
                // Update PaymentMethod information
                PaymentMethodViewModel.PaymentMethod = await _checkoutDataRepository.GetPaymentMethodAsync(paymentMethodId);
            }
            PaymentMethodViewModel.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

        public override void OnNavigatedFrom(System.Collections.Generic.Dictionary<string, object> viewModelState, bool suspending)
        {
            PaymentMethodViewModel.OnNavigatedFrom(viewModelState, suspending);
        }

        private async Task SaveAsync()
        {
            if (PaymentMethodViewModel.ValidateForm())
            {
                string errorMessage = string.Empty;

                try
                {
                    await PaymentMethodViewModel.ProcessFormAsync();
                    _navigationService.GoBack();
                }
                catch (ModelValidationException mvex)
                {
                    DisplayValidationErrors(mvex.ValidationResult);
                }
                catch (Exception ex)
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
            var errors = new Dictionary<string, Collection<string>>();

            // Property keys format: address.{Propertyname}
            foreach (var propkey in modelValidationResults.ModelState.Keys)
            {
                string propertyName = propkey.Substring(propkey.IndexOf('.') + 1); // strip off order. prefix

                errors.Add(propertyName, new Collection<string>(modelValidationResults.ModelState[propkey]));
            }

            if (errors.Count > 0) PaymentMethodViewModel.PaymentMethod.Errors.SetAllErrors(errors);
        }
    }
}