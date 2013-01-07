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
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Kona.Infrastructure;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Kona.UILogic.Services;
using Windows.UI.Xaml.Navigation;

namespace Kona.UILogic.ViewModels
{
    public class PaymentMethodUserControlViewModel : ViewModel, INavigationAware, IPaymentMethodUserControlViewModel
    {
        private string _cardNumber;
        private string _cardholderName;
        private string _expirationMonth;
        private string _expirationYear;
        private string _phone;
        private string _cardVerificationCode;
        private bool _saveInformation;
        private bool _setAsDefault;
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private EntityValidator _validator;
        private int _currentFormStatus;

        public PaymentMethodUserControlViewModel(ICheckoutDataRepository checkoutDataRepository)
        {
            _checkoutDataRepository = checkoutDataRepository;
            _validator = new EntityValidator(this);
            _validator.ErrorsChanged += ValidatorErrorsChanged;
        }

        [RestorableState]
        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "CardNumberRequired")]
        [StringLength(20, MinimumLength = 4, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "CardNumberInvalidLength")]
        public string CardNumber
        {
            get { return _cardNumber; }
            set
            {
                if (SetProperty(ref _cardNumber, value))
                {
                    Validator.ValidateProperty("CardNumber");
                }
            }
        }

        [RestorableState]
        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "CardholderNameRequired")]
        public string CardholderName
        {
            get { return _cardholderName; }
            set
            {
                if (SetProperty(ref _cardholderName, value))
                {
                    Validator.ValidateProperty("CardholderName");
                }
            }
        }

        [RestorableState]
        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "ExpirationMonthRequired")]
        public string ExpirationMonth
        {
            get { return _expirationMonth; }
            set
            {
                if (SetProperty(ref _expirationMonth, value))
                {
                    Validator.ValidateProperty("ExpirationMonth");
                }
            }
        }

        [RestorableState]
        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "ExpirationYearRequired")]
        public string ExpirationYear
        {
            get { return _expirationYear; }
            set
            {
                if (SetProperty(ref _expirationYear, value))
                {
                    Validator.ValidateProperty("ExpirationYear");
                }
            }
        }

        [RestorableState]
        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value); }
        }

        [RestorableState]
        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "CardVerificationCodeRequired")]
        public string CardVerificationCode
        {
            get { return _cardVerificationCode; }
            set
            {
                if (SetProperty(ref _cardVerificationCode, value))
                {
                    Validator.ValidateProperty("CardVerificationCode");
                }
            }
        }

        [RestorableState]
        public bool SaveInformation
        {
            get { return _saveInformation; }
            set { SetProperty(ref _saveInformation, value); }
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

        public EntityValidator Validator
        {
            get { return _validator; }
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState)
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
                        _validator.SetAllErrors(errorsCollection);
                    }
                }
            }

            if (navigationMode == NavigationMode.New)
            {
                if (_checkoutDataRepository.ContainsDefaultValue("PaymentInfo"))
                {
                    var defaultValue = _checkoutDataRepository.GetDefaultValue<PaymentInfo>("PaymentInfo");
                    PopulateEntity(defaultValue);

                    // Validate form fields
                    bool isValid = await Validator.ValidatePropertiesAsync();
                    CurrentFormStatus = isValid ? FormStatus.Complete : FormStatus.Invalid;
                }
            }
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewState, bool suspending)
        {
            if (!suspending)
            {
                Validator.ErrorsChanged -= ValidatorErrorsChanged;
            }

            base.OnNavigatedFrom(viewState, suspending);

            // Store the errors collection manually
            if (viewState != null)
            {
                AddEntityStateValue("errorsCollection", _validator.GetAllErrors(), viewState);
            }
        }

        public void ProcessForm()
        {
            var model = GetPaymentInfo();

            if (SaveInformation)
            {
                _checkoutDataRepository.SavePaymentInfo(model);
            }

            if (SetAsDefault)
            {
                _checkoutDataRepository.SetAsDefaultValue("PaymentInfo", model.Id);
            }
        }

        public async Task<bool> ValidateFormAsync()
        {
            bool isValid = await Validator.ValidatePropertiesAsync();
            CurrentFormStatus = isValid ? FormStatus.Complete : FormStatus.Invalid;
            return isValid;
        }

        private void PopulateEntity(PaymentInfo entity)
        {
            CardholderName = entity.CardholderName;
            CardNumber = entity.CardNumber;
            CardVerificationCode = entity.CardVerificationCode;
            ExpirationMonth = entity.ExpirationMonth;
            ExpirationYear = entity.ExpirationYear;
            Phone = entity.Phone;
            EntityId = entity.Id;
        }

        public PaymentInfo GetPaymentInfo()
        {
            return new PaymentInfo
                {
                    Id = EntityId,
                    CardholderName = CardholderName,
                    CardNumber = CardNumber,
                    CardVerificationCode = CardVerificationCode,
                    ExpirationMonth = ExpirationMonth,
                    ExpirationYear = ExpirationYear,
                    Phone = Phone
                };
        }

        private void ValidatorErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            var allErrors = Validator.GetAllErrors();
            CurrentFormStatus = allErrors.Values.Count > 0 ? FormStatus.Invalid : FormStatus.Incomplete;
        }
    }
}
