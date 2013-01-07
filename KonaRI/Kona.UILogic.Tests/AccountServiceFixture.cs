// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Threading.Tasks;
using Kona.UILogic.Services;
using Kona.UILogic.Tests.Mocks;
using Kona.UILogic.Tests.TestableClasses;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Net;
using Kona.UILogic.Models;
using Windows.Security.Credentials;

namespace Kona.UILogic.Tests
{
    [TestClass]
    public class AccountServiceFixture
    {
        [TestMethod]
        public async Task SuccessfullSignIn_RaisesUserChangedEvent()
        {
            bool userChangedFired = false;

            var identityService = new MockIdentityService();
            var stateService = new MockRestorableStateService();
            identityService.LogOnAsyncDelegate = (userId, password) =>
                {
                    var userValidationResult =
                        new UserValidationResult
                            {
                                IsValid = true,
                                UserInfo = new UserInfo{UserName = userId}
                            };
                    return Task.FromResult(new LogOnResult { ServerCookieHeader = string.Empty, UserValidationResult = userValidationResult });
                };

            var target = new AccountService(identityService, null, stateService, null);
            target.UserChanged += (sender, userInfo) => { userChangedFired = true; }; 

            var retVal = await target.SignInUserAsync("TestUserName", "TestPassword");
            Assert.IsTrue(retVal);
            Assert.IsTrue(userChangedFired);
        }

        [TestMethod]
        public async Task FailedSignIn_DoesNotRaiseUserChangedEvent()
        {
            bool userChangedFired = false;

            var identityService = new MockIdentityService();
            var stateService = new MockRestorableStateService();
            identityService.LogOnAsyncDelegate = (userId, password) =>
            {
                var userValidationResult =
                    new UserValidationResult
                    {
                        IsValid = false
                    };
                return Task.FromResult(new LogOnResult { ServerCookieHeader = string.Empty, UserValidationResult = userValidationResult });
            };

            var target = new AccountService(identityService, null, stateService, null);
            target.UserChanged += (sender, userInfo) => { userChangedFired = true; };

            var retVal = await target.SignInUserAsync("TestUserName", "TestPassword");
            Assert.IsFalse(retVal);
            Assert.IsFalse(userChangedFired);
        }

        [TestMethod]
        public async Task CheckIfUserSignedIn_ReturnsUserInfo_IfSessionIsStillLive()
        {
            var identityService = new MockIdentityService();
            identityService.VerifyActiveSessionDelegate = (userName, cookieHeader) => Task.FromResult(true);
            var target = new TestableAccountService(identityService, null, null, null);
            target.SignedInUser = new UserInfo {UserName = "TestUsername"};

            var userInfo = await target.CheckIfUserSignedIn();

            Assert.IsNotNull(userInfo);
        }

        [TestMethod]
        public async Task CheckIfUserSignedIn_ReturnsNull_IfSessionIsStillInactiveAndNoSavedCredentials()
        {
            var identityService = new MockIdentityService();
            identityService.VerifyActiveSessionDelegate = (userName, cookieHeader) => Task.FromResult(false);
            var credentialStore = new MockCredentialStore();
            credentialStore.GetSavedCredentialsDelegate = s => null;

            var target = new AccountService(identityService, null, null, credentialStore);

            var userInfo = await target.CheckIfUserSignedIn(); 
            
            Assert.IsNull(userInfo);
        }

        [TestMethod]
        public async Task CheckIfUserSignedIn_ReturnsUserInfo_IfSessionIsStillInactiveButHasSavedCredentials()
        {
            var identityService = new MockIdentityService();
            identityService.VerifyActiveSessionDelegate = (userName, cookieHeader) => Task.FromResult(false);
            identityService.LogOnAsyncDelegate =
                (userName, password) =>
                    {
                        Assert.AreEqual("TestUserName", userName);
                        Assert.AreEqual("TestPassword", password);
                        return Task.FromResult(new LogOnResult()
                                            {
                                                UserValidationResult =
                                                    new UserValidationResult() { IsValid = true, UserInfo = new UserInfo(){UserName = "ReturnedUserName"}}
                                            });
                    };
            var credentialStore = new MockCredentialStore();
            credentialStore.GetSavedCredentialsDelegate = s => new PasswordCredential("KonaRI", "TestUserName", "TestPassword");
            var target = new AccountService(identityService, null, null, credentialStore);

            var userInfo = await target.CheckIfUserSignedIn();

            Assert.IsNotNull(userInfo);
            Assert.AreEqual("ReturnedUserName", userInfo.UserName);
        }

        [TestMethod]
        public async Task CheckIfUserSignedIn_ReturnsNull_IfSessionIsStillInactiveAndHasInvalidSavedCredentials()
        {
            var identityService = new MockIdentityService();
            identityService.VerifyActiveSessionDelegate = (userName, cookieHeader) => Task.FromResult(false);
            identityService.LogOnAsyncDelegate =
                (userName, password) =>
                {
                    Assert.AreEqual("TestUserName", userName);
                    Assert.AreEqual("TestPassword", password);
                    return Task.FromResult(new LogOnResult()
                    {
                        UserValidationResult =
                            new UserValidationResult() { IsValid = false }
                    });
                };
            var credentialStore = new MockCredentialStore();
            credentialStore.GetSavedCredentialsDelegate = s => new PasswordCredential("KonaRI", "TestUserName", "TestPassword");
            var target = new AccountService(identityService, null, null, credentialStore);

            var userInfo = await target.CheckIfUserSignedIn();

            Assert.IsNull(userInfo);
        }
    }
}
