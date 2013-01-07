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
    public class SignOutFlyoutViewModelFixture
    {
        [TestMethod]
        public async Task SignOut_CallsSignOutinAccountServiceAndRemovesSavedCredentials()
        {
            var closeFlyoutCalled = false;
            var accountServiceSignOutCalled = false;
            var accountService = new MockAccountService();
            accountService.SignOutDelegate = () =>
                                                 {
                                                     accountServiceSignOutCalled = true;
                                                 };
            accountService.CheckIfUserSignedInDelegate = () => new UserInfo();
            var credentialStoreRemoveCredsCalled = false;
            var credentialStore = new MockCredentialStore();
            credentialStore.RemovedSavedCredentialsDelegate = s =>
                                                                  {
                                                                      credentialStoreRemoveCredsCalled = true;
                                                                      Assert.AreEqual("KonaRI", s);
                                                                  };
            var target = new SignOutFlyoutViewModel(accountService, credentialStore);
            target.CloseFlyout = () =>
                                     {
                                         closeFlyoutCalled = true;
                                     };

            target.Open();
            target.SignOutCommand.Execute();

            Assert.IsTrue(accountServiceSignOutCalled);
            Assert.IsTrue(credentialStoreRemoveCredsCalled);
            Assert.IsTrue(closeFlyoutCalled);
        }
    }
}
