// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.Infrastructure;
using Kona.UILogic.Models;
using Kona.UILogic.Services;

namespace Kona.UILogic.Tests.TestableClasses
{
    public class TestableAccountService : AccountService
    {
        public TestableAccountService(IIdentityService identityService, ISettingsCharmService settingsCharmService, IRestorableStateService stateService, ICredentialStore credentialStore) : base(identityService, settingsCharmService, stateService, credentialStore)
        {
        }

        public UserInfo SignedInUser
        {
            set { base.SignedInUser = value; }
        }
    }
}
