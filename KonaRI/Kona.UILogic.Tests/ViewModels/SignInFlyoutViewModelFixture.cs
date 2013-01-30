// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.Tests.Mocks;
using Kona.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Kona.UILogic.Tests.ViewModels
{
    [TestClass]
    public class SignInFlyoutViewModelFixture
    {
        [TestMethod]
        public void FiringSignInCommand_Persists_Credentials_And_Turns_Invisible()
        {
            var accountService = new MockAccountService();
            bool accountServiceSignInCalled = false;
            bool credentialStoreSaveCalled = false;

            accountService.SignInUserAsyncDelegate = (username, password) =>
            {
                Assert.AreEqual("TestUsername", username);
                Assert.AreEqual("TestPassword", password);
                accountServiceSignInCalled = true;
                return Task.FromResult(true);
            };

            var credentialStore = new MockCredentialStore();
            credentialStore.SaveCredentialsDelegate = (resource, username, password) =>
            {
                Assert.AreEqual("KonaRI", resource);
                Assert.AreEqual("TestUsername", username);
                Assert.AreEqual("TestPassword", password);
                credentialStoreSaveCalled = true;
            };

            var target = new SignInFlyoutViewModel(accountService, credentialStore);
            target.CloseFlyout = () => { Assert.IsTrue(true); };

            target.UserName = "TestUsername";
            target.Password = "TestPassword";
            target.SaveCredentials = true;
            target.SignInCommand.Execute();

            Assert.IsTrue(accountServiceSignInCalled);
            Assert.IsTrue(credentialStoreSaveCalled);
        }

        [TestMethod]
        public void FiringSignInCommand_WithNotRememberPassword_DoesNotSave()
        {
            var credentialStoreSaveCalled = false;
            var accountService = new MockAccountService();
            accountService.SignInUserAsyncDelegate = (username, password) =>
            {
                return Task.FromResult(true);
            };

            var credentialStore = new MockCredentialStore();
            credentialStore.SaveCredentialsDelegate = (resource, username, password) =>
                                                                          {
                                                                              credentialStoreSaveCalled = true;
                                                                              Assert.Fail();
                                                                          };
            var target = new SignInFlyoutViewModel(accountService, credentialStore);
            target.CloseFlyout = () => { Assert.IsTrue(true); };
            target.SaveCredentials = false;

            target.SignInCommand.Execute();

            Assert.IsFalse(credentialStoreSaveCalled);
        }

        [TestMethod]
        public void SuccessfulSignIn_CallsSuccessAction()
        {
            var successActionCalled = false;
            var accountService = new MockAccountService();
            accountService.SignInUserAsyncDelegate = (username, password) => Task.FromResult(true);
            var credentialStore = new MockCredentialStore();
            credentialStore.SaveCredentialsDelegate = (resource, username, password) => { };
            var target = new SignInFlyoutViewModel(accountService, credentialStore);

            target.Open(null, () => { successActionCalled = true; });

            target.SignInCommand.Execute();

            Assert.IsTrue(successActionCalled);
        }

        [TestMethod]
        public void UserName_ReturnsLastSignedInUser_IfAvailable()
        {
            var accountService = new MockAccountService();
            accountService.LastSignedInUser = new UserInfo { UserName = "TestUserName" };

            var target = new SignInFlyoutViewModel(accountService, null);

            Assert.AreEqual("TestUserName", target.UserName);
            Assert.IsFalse(target.IsNewSignIn);

        }
    }
}
