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
using System.Security;
using System.Net.Http;

namespace Kona.UILogic.Services
{
    public class AccountService : IAccountService
    {
        private const string STATE_TOKEN = "AccountService_ServerCookieHeader";

        private readonly IIdentityService _identityService;
        private readonly IRestorableStateService _stateService;
        private readonly ICredentialStore _credentialStore;
        private string _serverCookieHeader;
        private UserInfo _signedInUser;

        public AccountService(IIdentityService identityService, IRestorableStateService stateService, ICredentialStore credentialStore)
        {
            _identityService = identityService;
            _stateService = stateService;
            _credentialStore = credentialStore;
            if (_stateService != null) _stateService.AppRestored += (s, e) => { if (_stateService != null) _serverCookieHeader = _stateService.GetState(STATE_TOKEN) as string; };
        }

        public string ServerCookieHeader
        {
            get { return _serverCookieHeader; }
        }

        public UserInfo LastSignedInUser { get { return _signedInUser; } }

        /// <summary>
        /// Gets the current active user signed in the app.
        /// </summary>
        /// <returns>A Task that, when complete, stores an active user that is ready to be used for any operation agains the service.</returns>
        public async Task<UserInfo> GetSignedInUserAsync()
        {
            try
            {
                // If user is logged in, verify that the session in the service is still active
                if (_signedInUser != null && _serverCookieHeader != null && await _identityService.VerifyActiveSession(_signedInUser.UserName, _serverCookieHeader))
                {
                    return _signedInUser;
                }
            }
            catch (SecurityException){}
            {
                //User's session has expired.
            }

            // Attempt to sign in using credentials stored locally
            // If succeeds, ask for a new active session
            var savedCredentials = _credentialStore.GetSavedCredentials("KonaRI");
            if (savedCredentials != null && await SignInUserAsync(savedCredentials.UserName, savedCredentials.Password))
            {
                return _signedInUser;
            }

            return null;
        }

        // <snippet507>
        public async Task<bool> SignInUserAsync(string userName, string password)
        {
            try
            {
                var result = await _identityService.LogOnAsync(userName, password);
                _serverCookieHeader = result.ServerCookieHeader;
                _stateService.SaveState(STATE_TOKEN, _serverCookieHeader);
                var previousSignedInUser = _signedInUser;
                _signedInUser = result.UserInfo;
                RaiseUserChanged(result.UserInfo, previousSignedInUser);
                return true;
            }
            catch (HttpRequestException)
            {
                _serverCookieHeader = string.Empty;
                _stateService.SaveState(STATE_TOKEN, _serverCookieHeader);
            }

            return false;
        }
        // </snippet507>

        public event EventHandler<UserChangedEventArgs> UserChanged;

        private void RaiseUserChanged(UserInfo newUserInfo, UserInfo oldUserInfo)
        {
            var handler = UserChanged;
            if (handler != null)
            {
                handler(this, new UserChangedEventArgs(newUserInfo, oldUserInfo));
            }
        }

        public void SignOut()
        {
            _serverCookieHeader = null;
            var previousSignedInUser = _signedInUser;
            _signedInUser = null;
            RaiseUserChanged(null, previousSignedInUser);
        }
    }
}
