// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Kona.Infrastructure;

namespace ValidationQuickStart.Models
{
    public interface IUserInfo
    {
        string FirstName { get; set; }

        string MiddleName { get; set; }

        string LastName { get; set; }

        BindableValidator Errors { get; }

        bool ValidateProperties();

        ReadOnlyDictionary<string, ReadOnlyCollection<string>> GetAllErrors();

        event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    }
}
