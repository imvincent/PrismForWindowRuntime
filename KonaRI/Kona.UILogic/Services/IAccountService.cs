// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Kona.UILogic.Models;

namespace Kona.UILogic.Services
{
    public interface IAccountService
    {
        string UserId { get; }

        Task<UserInfo> CheckIfUserSignedIn();

        void DisplaySignIn(Action signInSuccessful, Action signInFailed);

        Task<bool> SignInUserAsync(string userName, string password);

        event EventHandler<UserChangedEventArgs> UserChanged;

        string ServerCookieHeader { get; }
        void SignOut();
    }
}
