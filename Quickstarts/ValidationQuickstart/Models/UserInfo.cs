// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.ComponentModel.DataAnnotations;

namespace ValidationQuickStart.Models
{
    public class UserInfo : IUserInfo
    {
        // We allow all Unicode letter characters as well as internal spaces and hypens, as long as these do not occur in sequences
        private const string REGEX_PATTERN = @"\A\p{L}+([\p{Zs}\-][\p{L}]+)*\z";

        // <snippet1310>
        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "FirstNameRequired")]
        [RegularExpression(REGEX_PATTERN, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "FirstNameRegex")]
        public string FirstName { get; set; }

        [RegularExpression(REGEX_PATTERN, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "MiddleNameRegex")]
        [CustomValidation(typeof(UserInfo), "ValidateMiddleName")]
        public string MiddleName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "LastNameRequired")]
        [RegularExpression(REGEX_PATTERN, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "LastNameRegex")]
        public string LastName { get; set; }
        // </snippet1310>

        // <snippet1311>
        public static ValidationResult ValidateMiddleName(object value, ValidationContext validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentException("validationContext cannot be null", "validationContext");
            }

            UserInfo user = (UserInfo)validationContext.ObjectInstance;

            // Rule: Middle Name is required if the First Name is an initial
            if (string.IsNullOrEmpty((string)value) && !string.IsNullOrEmpty(user.FirstName) && user.FirstName.Length == 1)
            {
                return new ValidationResult(ErrorMessagesHelper.MiddleNameFirstName);
            }
            else
            {
                return ValidationResult.Success;
            }
        }
        // </snippet1311>
    }
}
