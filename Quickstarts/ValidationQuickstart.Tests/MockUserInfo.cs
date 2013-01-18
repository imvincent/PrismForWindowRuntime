// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ValidationQuickStart.Models;

namespace ValidationQuickstart.Tests
{
    public class MockUserInfo : IUserInfo, INotifyPropertyChanged
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
