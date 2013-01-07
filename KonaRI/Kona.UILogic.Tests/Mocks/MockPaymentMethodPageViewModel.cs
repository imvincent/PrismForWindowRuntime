// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Kona.Infrastructure;
using Kona.UILogic.Models;
using Kona.UILogic.ViewModels;
using Windows.UI.Xaml.Navigation;

namespace Kona.UILogic.Tests.Mocks
{
    public class MockPaymentMethodPageViewModel : IPaymentMethodUserControlViewModel
    {
        public string CardNumber { get; set; }
        public string CardholderName { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string Phone { get; set; }
        public string CardVerificationCode { get; set; }
        public bool SaveInformation { get; set; }
        public bool SetAsDefault { get; set; }
        public string FirstError { get; set; }
        public int CurrentFormStatus { get; set; }
        public EntityValidator Validator { get; private set; }
        public ICommand GoBackCommand { get; private set; }
        public ICommand RegisterCommand { get; private set; }
        public string EntityId { get; set; }

        public Func<Task<bool>> ValidateFormAsyncDelegate { get; set; }

        public Action ProcessFormDelegate { get; set; }

        public void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState)
        {
            throw new System.NotImplementedException();
        }

        public void OnNavigatedFrom(Dictionary<string, object> viewState, bool suspending)
        {
            throw new System.NotImplementedException();
        }

        public void Register()
        {
            throw new System.NotImplementedException();
        }

        public void ProcessForm()
        {
            ProcessFormDelegate();
        }

        public Task<bool> ValidateFormAsync()
        {
            return ValidateFormAsyncDelegate();
        }

        public PaymentInfo GetPaymentInfo()
        {
            return GetPaymentInfoDelegate;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public PaymentInfo GetPaymentInfoDelegate { get; set; }
    }
}
