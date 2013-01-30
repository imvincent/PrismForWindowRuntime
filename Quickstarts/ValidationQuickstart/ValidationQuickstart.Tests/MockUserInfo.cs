// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Kona.Infrastructure;
using ValidationQuickStart.Models;

namespace ValidationQuickstart.Tests
{
    public class MockUserInfo : IUserInfo, INotifyPropertyChanged
    {
        public Func<bool> ValidatePropertiesDelegate { get; set; }
        public Func<ReadOnlyDictionary<string, ReadOnlyCollection<string>>> GetAllErrorsDelegate { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        public BindableValidator Errors { get; private set; }

        public bool ValidateProperties()
        {
            return ValidatePropertiesDelegate();
        }

        public ReadOnlyDictionary<string, ReadOnlyCollection<string>> GetAllErrors()
        {
            return GetAllErrorsDelegate();
        }

        public void RaiseErrorsChanged()
        {
            ErrorsChanged(this, null);
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
