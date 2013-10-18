// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.ComponentModel.DataAnnotations;
using Microsoft.Practices.Prism.StoreApps;

namespace ValidationQuickStart.Models
{
    //This QuickStart is documented at http://go.microsoft.com/fwlink/?LinkID=288827&clcid=0x409

    public class UserInfo : ValidatableBindableBase
    {
        private string _firstName;
        private string _middleName;
        private string _lastName;

        // We allow all Unicode letter characters as well as internal spaces and hypens, as long as these do not occur in sequences
        private const string RegexPattern = @"\A\p{L}+([\p{Zs}\-][\p{L}]+)*\z";

        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "FirstNameRequired")]
        [RegularExpression(RegexPattern, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "FirstNameRegex")]
        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }

        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "MiddleNameRequired")]
        [RegularExpression(RegexPattern, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "MiddleNameRegex")]
        public string MiddleName
        {
            get { return _middleName; }
            set { SetProperty(ref _middleName, value); }
        }

        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "LastNameRequired")]
        [RegularExpression(RegexPattern, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "LastNameRegex")]
        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }
    }
}
