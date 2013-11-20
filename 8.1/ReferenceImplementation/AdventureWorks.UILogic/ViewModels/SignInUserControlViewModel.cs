// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Net;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Services;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;

namespace AdventureWorks.UILogic.ViewModels
{
    public class SignInUserControlViewModel : ViewModel, ISignInUserControlViewModel
    {
        private string _userName;
        private string _password;
        private bool _saveCredentials;
        private bool _isSignInInvalid;
        private bool _isOpened;
        private readonly IAccountService _accountService;
        private readonly IAlertMessageService _alertMessageService;
        private readonly IResourceLoader _resourceLoader;
        private Action _successAction;

        public SignInUserControlViewModel(IAccountService accountService, IAlertMessageService alertMessageService, IResourceLoader resourceLoader)
        {
            _accountService = accountService;
            _alertMessageService = alertMessageService;
            _resourceLoader = resourceLoader;
            
            SignInCommand = DelegateCommand.FromAsyncHandler(SignInAsync, CanSignIn);
            GoBackCommand = new DelegateCommand(Close);

            if (_accountService.SignedInUser != null)
            {
                _userName = _accountService.SignedInUser.UserName;
                IsNewSignIn = false;
            }
            else
            {
                IsNewSignIn = true;
            }
        }

        [RestorableState]
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

        [RestorableState]
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

        [RestorableState]
        public bool SaveCredentials
        {
            get { return _saveCredentials; }
            set { SetProperty(ref _saveCredentials, value); }
        }

        [RestorableState]
        public bool IsSignInInvalid
        {
            get { return _isSignInInvalid; }
            private set { SetProperty(ref _isSignInInvalid, value); }
        }

        [RestorableState]
        public bool IsOpened
        {
            get { return _isOpened; }
            private set { SetProperty(ref _isOpened, value); }
        }

        public bool IsNewSignIn { get; set; }

        public DelegateCommand GoBackCommand { get; private set; }

        public DelegateCommand SignInCommand { get; private set; }

        public void Open(Action successAction)
        {
            IsOpened = true;
            _successAction = successAction;
        }

        private bool CanSignIn()
        {
            return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);
        }

        private async Task SignInAsync()
        {
            var signinCallFailed = false;
            var signinSuccessfull = false;
            try
            {
                signinSuccessfull = await _accountService.SignInUserAsync(UserName, Password, SaveCredentials);
            }
            catch (WebException)
            {
                signinCallFailed = true;
            }
            if (signinCallFailed)
            {
                await _alertMessageService.ShowAsync(_resourceLoader.GetString("ErrorServiceUnreachable"), _resourceLoader.GetString("Error"));
                return;
            }
            if (signinSuccessfull)
            {
                IsSignInInvalid = false;

                if (_successAction != null)
                {
                    _successAction();
                    _successAction = null;
                }

                Close();
            }
            else
            {
                IsSignInInvalid = true;
            }
        }

        private void Close()
        {
            IsOpened = false;
        }
    }
}
