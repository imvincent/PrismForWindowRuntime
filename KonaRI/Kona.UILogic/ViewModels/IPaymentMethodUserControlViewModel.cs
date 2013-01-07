// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Kona.Infrastructure;
using Kona.UILogic.Models;
using Kona.UILogic.Services;
using Windows.UI.Xaml.Navigation;

namespace Kona.UILogic.ViewModels
{
    public interface IPaymentMethodUserControlViewModel
    {
        [RestorableState]
        [Required(ErrorMessageResourceType = typeof (ErrorMessagesHelper), ErrorMessageResourceName = "CardNumberRequired")]
        string CardNumber { get; set; }

        [RestorableState]
        [Required(ErrorMessageResourceType = typeof (ErrorMessagesHelper), ErrorMessageResourceName = "CardholderNameRequired")]
        string CardholderName { get; set; }

        [RestorableState]
        [Required(ErrorMessageResourceType = typeof (ErrorMessagesHelper), ErrorMessageResourceName = "ExpirationMonthRequired")]
        string ExpirationMonth { get; set; }

        [RestorableState]
        [Required(ErrorMessageResourceType = typeof (ErrorMessagesHelper), ErrorMessageResourceName = "ExpirationYearRequired")]
        string ExpirationYear { get; set; }

        [RestorableState]
        string Phone { get; set; }

        [RestorableState]
        [Required(ErrorMessageResourceType = typeof (ErrorMessagesHelper), ErrorMessageResourceName = "CardVerificationCodeRequired")]
        string CardVerificationCode { get; set; }

        [RestorableState]
        bool SaveInformation { get; set; }

        [RestorableState]
        bool SetAsDefault { get; set; }

        [RestorableState]
        int CurrentFormStatus { get; set; }

        EntityValidator Validator { get; }
        string EntityId { get; set; }
        void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState);
        void OnNavigatedFrom(Dictionary<string, object> viewState, bool suspending);
        void ProcessForm();
        Task<bool> ValidateFormAsync();
        PaymentInfo GetPaymentInfo();
        event PropertyChangedEventHandler PropertyChanged;
    }
}