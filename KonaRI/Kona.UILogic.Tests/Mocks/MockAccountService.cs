// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Threading.Tasks;
using Kona.UILogic.Services;
using System.Net;
using Kona.UILogic.Models;

namespace Kona.UILogic.Tests.Mocks
{
    public class MockAccountService : IAccountService
    {
        public Func<string> GetUserIdDelegate { get; set; }

        public Func<string, string, Task<bool>> SignInUserAsyncDelegate { get; set; }

        public Func<UserInfo> CheckIfUserSignedInDelegate { get; set; } 

        public Action DisplaySignInDelegate { get; set; }

        public Action SignOutDelegate { get; set; }

        public Action signInSuccessful { get; set; }

        public Action signInFailed { get; set; }

        public string UserId
        {
            get
            {
                return GetUserIdDelegate();
            }
        }

        public async Task<bool> SignInUserAsync(string userName, string password)
        {
            var result = await this.SignInUserAsyncDelegate(userName, password);
            if (result)
                signInSuccessful();
            else
                signInFailed();
            return result;
        }

        public async Task<UserInfo> CheckIfUserSignedIn()
        {
            return CheckIfUserSignedInDelegate();
        }

        public void DisplaySignIn(Action signInSuccessful = null, Action signInFailed = null)
        {
            this.signInSuccessful = signInSuccessful;
            this.signInFailed = signInFailed;
            this.DisplaySignInDelegate();
        }

        public void RaiseUserChanged(UserInfo userInfo)
        {
            UserChanged(this, new UserChangedEventArgs(userInfo));
        }

        public event EventHandler<UserChangedEventArgs> UserChanged;


        public string ServerCookieHeader
        {
            get { return string.Empty; }
        }

        public void SignOut()
        {
            SignOutDelegate();
        }
    }
}
