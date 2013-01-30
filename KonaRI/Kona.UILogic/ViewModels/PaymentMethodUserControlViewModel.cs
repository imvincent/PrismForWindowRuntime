// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Kona.Infrastructure;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Windows.UI.Xaml.Navigation;

namespace Kona.UILogic.ViewModels
{
    public class PaymentMethodUserControlViewModel : ViewModel, INavigationAware, IPaymentMethodUserControlViewModel
    {
        private bool _setAsDefault;
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private int _currentFormStatus;
        private PaymentInfo _paymentInfo;

        public PaymentMethodUserControlViewModel(ICheckoutDataRepository checkoutDataRepository)
        {
            _paymentInfo = new PaymentInfo() { Id = Guid.NewGuid().ToString() };
            _checkoutDataRepository = checkoutDataRepository;
        }
        
        [RestorableState]
        public PaymentInfo PaymentInfo
        {
            get { return _paymentInfo; }
            set { SetProperty(ref _paymentInfo, value); }
        }

        [RestorableState]
        public bool SetAsDefault
        {
            get { return _setAsDefault; }
            set { SetProperty(ref _setAsDefault, value); }
        }

        [RestorableState]
        public int CurrentFormStatus
        {
            get { return _currentFormStatus; }
            set { SetProperty(ref _currentFormStatus, value); }
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState)
        {
            if (viewState != null)
            {
                base.OnNavigatedTo(navigationParameter, navigationMode, viewState);

                if (navigationMode == NavigationMode.Refresh)
                {
                    // Restore the errors collection manually
                    var errorsCollection = RetrieveEntityStateValue<IDictionary<string, ReadOnlyCollection<string>>>("errorsCollection", viewState);

                    if (errorsCollection != null)
                    {
                        _paymentInfo.SetAllErrors(errorsCollection);
                    }
                }
            }

            if (navigationMode == NavigationMode.New)
            {
                if (_checkoutDataRepository.ContainsDefaultValue("PaymentInfo"))
                {
                    PaymentInfo = _checkoutDataRepository.GetDefaultPaymentInfoValue();

                    // Validate form fields
                    ValidateForm();
                }
            }
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewState, bool suspending)
        {
            base.OnNavigatedFrom(viewState, suspending);

            // Store the errors collection manually
            if (viewState != null)
            {
                AddEntityStateValue("errorsCollection", _paymentInfo.GetAllErrors(), viewState);
            }
        }

        public void ProcessForm()
        {
            var savedPaymentInformation =  _checkoutDataRepository.SavePaymentInfo(PaymentInfo);

            //If matching saved payment information found, use saved payment information
            if (savedPaymentInformation.Id != PaymentInfo.Id)
            {
                PaymentInfo = savedPaymentInformation;
            }

            if (SetAsDefault)
            {
                _checkoutDataRepository.SetAsDefaultPaymentInfo(savedPaymentInformation.Id);
            }
        }

        public bool ValidateForm()
        {
            return _paymentInfo.ValidateProperties();
        }
    }
}
