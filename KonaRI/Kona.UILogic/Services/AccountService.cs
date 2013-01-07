// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Threading.Tasks;
using Kona.Infrastructure;
using Kona.UILogic.Models;

namespace Kona.UILogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly ISettingsCharmService _settingsCharmService;
        private readonly IIdentityService _identityService;
        private readonly IRestorableStateService _stateService;
        private readonly ICredentialStore _credentialStore;
        private Action _signInSuccessful;
        private Action _signInFailed;
        private string _userId;
        private string _serverCookieHeader;
        const string _stateToken = "AccountService_ServerCookieHeader";
        private UserInfo _signedInUser;

        public AccountService(IIdentityService identityService, ISettingsCharmService settingsCharmService, IRestorableStateService stateService, ICredentialStore credentialStore)
        {
            _userId = Guid.NewGuid().ToString();
            _identityService = identityService;
            _settingsCharmService = settingsCharmService;
            _stateService = stateService;
            _credentialStore = credentialStore;
            if (_stateService != null) _stateService.AppRestored += (s, e) => { if (_stateService != null) _serverCookieHeader = _stateService.GetState(_stateToken) as string; };
        }

        public string UserId
        {
            get
            {
                if (_signedInUser != null)
                {
                    return _signedInUser.UserName;
                }
                return _userId;
            }
        }

        protected UserInfo SignedInUser
        {
            get { return _signedInUser; }
            set { _signedInUser = value; }
        }

        public async Task<UserInfo> CheckIfUserSignedIn()
        {
            //If user has logged in, verify session is still active
            if (_signedInUser != null && await _identityService.VerifyActiveSession(_signedInUser.UserName, _serverCookieHeader))
            {
                return _signedInUser;
            }
            
            //Attempt to sign in using saved credentials
            var savedCredentials = _credentialStore.GetSavedCredentials("KonaRI");
            if (savedCredentials != null)
            {
                var result = await _identityService.LogOnAsync(savedCredentials.UserName, savedCredentials.Password);
                if (result != null && result.UserValidationResult.IsValid)
                {
                    _signedInUser = result.UserValidationResult.UserInfo;
                    return _signedInUser;
                }
            }
            return null;
        }

        public void DisplaySignIn(Action signInSuccessful, Action signInFailed)
        {
            _signInSuccessful = signInSuccessful;
            _signInFailed = signInFailed;

            _settingsCharmService.ShowFlyout("signIn");
        }

        public async Task<bool> SignInUserAsync(string userName, string password)
        {
            var loginResult = await _identityService.LogOnAsync(userName, password);
            if (loginResult.UserValidationResult.IsValid)
            {
                _serverCookieHeader = loginResult.ServerCookieHeader;
                _stateService.SaveState(_stateToken, _serverCookieHeader);
                _signedInUser = loginResult.UserValidationResult.UserInfo;
                RaiseUserChanged(loginResult.UserValidationResult.UserInfo);
                if (_signInSuccessful != null) _signInSuccessful();
            }
            else
            {
                if (_signInFailed != null) _signInFailed();
                _serverCookieHeader = string.Empty;
                _stateService.SaveState(_stateToken, _serverCookieHeader);
            }

            return loginResult.UserValidationResult.IsValid;
        }

        public event EventHandler<UserChangedEventArgs> UserChanged;

        private void RaiseUserChanged(UserInfo userInfo)
        {
            var handler = UserChanged;
            if (handler != null)
            {
                handler(this, new UserChangedEventArgs(userInfo));
            }
        }

        public string ServerCookieHeader
        {
            get { return _serverCookieHeader; }
        }

        public void SignOut()
        {
            _serverCookieHeader = null;
            _signedInUser = null;
        }
    }
}
