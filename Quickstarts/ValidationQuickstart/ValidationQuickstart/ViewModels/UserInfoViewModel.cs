// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Kona.Infrastructure;
using ValidationQuickStart.Models;

namespace ValidationQuickStart.ViewModels
{
    public class UserInfoViewModel : ViewModel
    {
        private IUserInfo _userInfo;
        private ReadOnlyCollection<string> _allErrors;

        // <snippet1306> 
        public UserInfoViewModel()
            : this(new UserInfo())
        {
        }

        public UserInfoViewModel(IUserInfo userInfo)
        {
            _userInfo = userInfo;
            _userInfo.ErrorsChanged += OnErrorsChanged;
            _allErrors = BindableValidator.EmptyErrorsCollection;
            ValidateCommand = new DelegateCommand(Validate);
        }
        // </snippet1306>

        public IUserInfo UserInfo
        {
            get { return _userInfo; }
            set { SetProperty(ref _userInfo, value); }
        }

        public ReadOnlyCollection<string> AllErrors
        {
            get { return _allErrors; }
            private set { SetProperty(ref _allErrors, value); }
        }

        public ICommand ValidateCommand { get; set; }

        private void Validate()
        {
            _userInfo.ValidateProperties();
        }

        // <snippet1308>
        private void OnErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            AllErrors = new ReadOnlyCollection<string>(_userInfo.GetAllErrors().Values.SelectMany(c => c).ToList());
        }
        // </snippet1308>
    }
}
