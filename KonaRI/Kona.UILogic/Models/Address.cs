// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Kona.Infrastructure;
using Kona.UILogic.Services;

namespace Kona.UILogic.Models
{
    public class Address : BindableBase
    {
        private string _id;
        private string _firstName;
        private string _middleInitial;
        private string _lastName;
        private string _streetAddress;
        private string _optionalAddress;
        private string _city;
        private string _state;
        private string _zipCode;
        private string _phone;

        public static ILocationService LocationService;

        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "FirstNameRequired")]
        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }

        public string MiddleInitial
        {
            get { return _middleInitial; }
            set { SetProperty(ref _middleInitial, value); }
        }

        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "LastNameRequired")]
        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }

        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "AddressRequired")]
        public string StreetAddress
        {
            get { return _streetAddress; }
            set { SetProperty(ref _streetAddress, value); }
        }

        public string OptionalAddress
        {
            get { return _optionalAddress; }
            set { SetProperty(ref _optionalAddress, value); }
        }

        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "CityRequired")]
        public string City
        {
            get { return _city; }
            set { SetProperty(ref _city, value); }
        }

        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "StateRequired")]
        public string State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "ZipCodeRequired")]
        [AsyncValidation(typeof(Address), "ValidateZipCodeAsync")]
        public string ZipCode
        {
            get { return _zipCode; }
            set { SetProperty(ref _zipCode, value); }
        }

        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "PhoneRequired")]
        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value); }
        }

        public static async Task<ValidationResult> ValidateZipCodeAsync(object value, ValidationContext validationContext)
        {
            string state = ((Address)validationContext.ObjectInstance).State;
            string zipCode = (string)value;

            bool isValid = await LocationService.GetIsZipCodeValidAsync(state, zipCode);

            if (isValid)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessagesHelper.ZipCodeState);
            }
        }
    }
}
