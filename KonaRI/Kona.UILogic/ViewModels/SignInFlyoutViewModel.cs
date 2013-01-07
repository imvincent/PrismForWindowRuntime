// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Threading.Tasks;
using Kona.Infrastructure;
using Kona.Infrastructure.Flyouts;
using Kona.UILogic.Services;

namespace Kona.UILogic.ViewModels
{
    public class SignInFlyoutViewModel : BindableBase, IFlyoutViewModel
    {
        private string _userName;
        private string _password;
        private bool _isSignInInvalid;
        private readonly IAccountService _accountService;
        private readonly ICredentialStore _credentialStore;
        private bool _saveCredentials;

        public SignInFlyoutViewModel(IAccountService accountService, ICredentialStore credentialStore)
        {
            _accountService = accountService;
            _credentialStore = credentialStore;
            SignInCommand = new DelegateCommand(async () => await SignInAsync(), () => CanSignIn());
            GoBackCommand = new DelegateCommand(() => GoBack(), () => true);
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                if (SetProperty(ref _userName, value))
                {
                    SignInCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (SetProperty(ref _password, value))
                {
                    SignInCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public bool SaveCredentials
        {
            get { return _saveCredentials; }
            set { SetProperty(ref _saveCredentials, value); }
        }

        public bool IsSignInInvalid
        {
            get { return _isSignInInvalid; }
            set { SetProperty(ref _isSignInInvalid, value); }
        }

        public Action CloseFlyout { get; set; }
        

        public Action GoBack { get; set; }
        
        public DelegateCommand GoBackCommand { get; private set; }
        
        public DelegateCommand SignInCommand { get; private set; }

        public void Open() {}

        public bool CanSignIn()
        {
            return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);
        }

        public async Task SignInAsync()
        {
            var result = await _accountService.SignInUserAsync(UserName, Password);

            if (result)
            {
                IsSignInInvalid = false;

                if (SaveCredentials)
                {
                    _credentialStore.SaveCredentials("KonaRI", UserName, Password);
                }
                CloseFlyout();
            }
            else
            {
                IsSignInInvalid = true;
            }
        }
    }
}
