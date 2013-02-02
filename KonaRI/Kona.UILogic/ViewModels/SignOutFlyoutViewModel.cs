// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using Kona.Infrastructure;
using Kona.Infrastructure.Flyouts;
using Kona.UILogic.Models;
using Kona.UILogic.Services;

namespace Kona.UILogic.ViewModels
{
    public class SignOutFlyoutViewModel : BindableBase, IFlyoutViewModel
    {
        private readonly IAccountService _accountService;
        private readonly ICredentialStore _credentialStore;
        private readonly INavigationService _navigationService;
        private UserInfo _userInfo;
        private DelegateCommand _signOutCommand;
        private string _userName;

        public SignOutFlyoutViewModel(IAccountService accountService, ICredentialStore credentialStore, INavigationService navigationService)
        {
            _accountService = accountService;
            _credentialStore = credentialStore;
            _navigationService = navigationService;
            GoBackCommand = new DelegateCommand(() => GoBack(), () => true);
        }


        public void Open(object parameter, Action successAction)
        {
            _userInfo = _accountService.LastSignedInUser;

            if (_userInfo != null)
            {
                UserName = _userInfo.UserName;
            }
            SignOutCommand = new DelegateCommand(SignOut, CanSignOut);
        }

        public Action CloseFlyout { get; set; }

        public Action GoBack { get; set; }

        public DelegateCommand GoBackCommand { get; private set; }

        public DelegateCommand SignOutCommand
        {
            get { return _signOutCommand; }
            private set { SetProperty(ref _signOutCommand, value); }
        }

        private bool CanSignOut()
        {
            return _userInfo != null;
        }

        private void SignOut()
        {
            _credentialStore.RemovedSavedCredentials("KonaRI");
            _accountService.SignOut();
            _navigationService.ClearHistory();
            _navigationService.Navigate("Hub", null);
            CloseFlyout();
        }

        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }
    }
}
