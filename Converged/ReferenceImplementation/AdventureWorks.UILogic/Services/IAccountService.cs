// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.Services
{
    public interface IAccountService
    {
        UserInfo SignedInUser { get; }

        Task<UserInfo> VerifyUserAuthenticationAsync();
        Task<UserInfo> VerifySavedCredentialsAsync();

        Task<bool> SignInUserAsync(string userName, string password, bool useCredentialStore);
        
        void SignOut();
        
        event EventHandler<UserChangedEventArgs> UserChanged;
    }
}
