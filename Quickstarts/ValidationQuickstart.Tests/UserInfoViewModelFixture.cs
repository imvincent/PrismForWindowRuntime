// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ValidationQuickStart.ViewModels;

namespace ValidationQuickstart.Tests
{
    [TestClass]
    public class UserInfoViewModelFixture
    {
        #region PropertyChanged event Test Cases

        [TestMethod]
        public void FirstName_Fires_PropertyChanged_When_Updated()
        {
            bool propertyChangedFired = false;
            var target = new UserInfoViewModel();
            target.PropertyChanged += (s, e) => propertyChangedFired = (e.PropertyName == "FirstName");

            target.FirstName = "PropertyChanged should be fired";

            Assert.IsTrue(propertyChangedFired);
        }

        [TestMethod]
        public void FirstName_Does_Not_Fire_PropertyChanged_If_Not_Updated()
        {
            bool propertyChangedFired = false;
            var target = new UserInfoViewModel { FirstName = "Initial value" };
            target.PropertyChanged += (s, e) => propertyChangedFired = (e.PropertyName == "FirstName");

            target.FirstName = "Initial value";

            Assert.IsFalse(propertyChangedFired);
        }

        [TestMethod]
        public void MiddleName_Fires_PropertyChanged_When_Updated()
        {
            bool propertyChangedFired = false;
            var target = new UserInfoViewModel();
            target.PropertyChanged += (s, e) => propertyChangedFired = (e.PropertyName == "MiddleName");

            target.MiddleName = "PropertyChanged should be fired";

            Assert.IsTrue(propertyChangedFired);
        }

        [TestMethod]
        public void MiddleName_Does_Not_Fire_PropertyChanged_If_Not_Updated()
        {
            bool propertyChangedFired = false;
            var target = new UserInfoViewModel { MiddleName = "Initial value" };
            target.PropertyChanged += (s, e) => propertyChangedFired = (e.PropertyName == "MiddleName");

            target.MiddleName = "Initial value";

            Assert.IsFalse(propertyChangedFired);
        }

        [TestMethod]
        public void LastName_Fires_PropertyChanged_When_Updated()
        {
            bool propertyChangedFired = false;
            var target = new UserInfoViewModel();
            target.PropertyChanged += (s, e) => propertyChangedFired = (e.PropertyName == "LastName");

            target.LastName = "PropertyChanged should be fired";

            Assert.IsTrue(propertyChangedFired);
        }

        [TestMethod]
        public void LastName_Does_Not_Fire_PropertyChanged_If_Not_Updated()
        {
            bool propertyChangedFired = false;
            var target = new UserInfoViewModel { LastName = "Initial value" };
            target.PropertyChanged += (s, e) => propertyChangedFired = (e.PropertyName == "LastName");

            target.LastName = "Initial value";

            Assert.IsFalse(propertyChangedFired);
        }

        #endregion

        #region Validation Test Cases

        [TestMethod]
        public void FirstName_Updates_AllErrors_If_Invalid()
        {
            bool propertyChangedFired = false;
            var target = new UserInfoViewModel();
            target.PropertyChanged += (s, e) => propertyChangedFired = (e.PropertyName == "AllErrors");

            // We update the AllErrors collection by setting a property with invalid values
            target.FirstName = "123 <-- This is invalid";

            Assert.IsTrue(propertyChangedFired);
        }

        [TestMethod]
        public void FirstName_Does_Not_Update_AllErrors_If_Valid()
        {
            bool propertyChangedFired = false;
            var target = new UserInfoViewModel();
            target.PropertyChanged += (s, e) => propertyChangedFired = (e.PropertyName == "AllErrors");

            // We update the AllErrors collection by setting a property with invalid values
            target.FirstName = "This is valid";

            Assert.IsFalse(propertyChangedFired);
        }

        [TestMethod]
        public void MiddleName_Updates_AllErrors_If_Invalid()
        {
            bool propertyChangedFired = false;
            var target = new UserInfoViewModel();
            target.PropertyChanged += (s, e) => propertyChangedFired = (e.PropertyName == "AllErrors");

            // We update the AllErrors collection by setting a property with invalid values
            target.MiddleName = "123 <-- This is invalid";

            Assert.IsTrue(propertyChangedFired);
        }

        [TestMethod]
        public void MiddleName_Does_Not_Update_AllErrors_If_Valid()
        {
            bool propertyChangedFired = false;
            var target = new UserInfoViewModel();
            target.PropertyChanged += (s, e) => propertyChangedFired = (e.PropertyName == "AllErrors");

            // We update the AllErrors collection by setting a property with invalid values
            target.MiddleName = "This is valid";

            Assert.IsFalse(propertyChangedFired);
        }

        [TestMethod]
        public void LastName_Updates_AllErrors_If_Invalid()
        {
            bool propertyChangedFired = false;
            var target = new UserInfoViewModel();
            target.PropertyChanged += (s, e) => propertyChangedFired = (e.PropertyName == "AllErrors");

            // We update the AllErrors collection by setting a property with invalid values
            target.LastName = "123 <-- This is invalid";

            Assert.IsTrue(propertyChangedFired);
        }

        [TestMethod]
        public void LastName_Does_Not_Update_AllErrors_If_Valid()
        {
            bool propertyChangedFired = false;
            var target = new UserInfoViewModel();
            target.PropertyChanged += (s, e) => propertyChangedFired = (e.PropertyName == "AllErrors");

            // We update the AllErrors collection by setting a property with invalid values
            target.LastName = "This is valid";

            Assert.IsFalse(propertyChangedFired);
        }

        [TestMethod]
        public void MiddleName_Is_Validated_If_FirstName_Changes()
        {
            bool propertyChangedFired = false;
            var userInfoWithInvalidMiddleName = new MockUserInfo
            {
                FirstName = "Valid FirstName",
                MiddleName = string.Empty,
                LastName = "Valid LastName",
            };
            var target = new UserInfoViewModel(userInfoWithInvalidMiddleName);
            target.PropertyChanged += (s, e) => propertyChangedFired = (e.PropertyName == "AllErrors");

            // We need to check if MiddleName is valid
            // if the FirstName changes
            target.FirstName = "This is a valid value";

            Assert.IsTrue(propertyChangedFired);
        }

        [TestMethod]
        public async Task SubmitAsyncCommand_Fires_Validation_For_All_The_Properties()
        {
            bool propertyChangedFiredWithValidValues = false;
            bool propertyChangedFiredWithInvalidFirstName = false;
            bool propertyChangedFiredWithInvalidMiddleName = false;
            bool propertyChangedFiredWithInvalidLastName = false;

            var userInfoWithValidValues = new MockUserInfo
            {
                FirstName = "Valid FirstName",
                MiddleName = "Valid MiddleName",
                LastName = "Valid LastName",
            };
            var targetWithValidValues = new UserInfoViewModel(userInfoWithValidValues);
            targetWithValidValues.PropertyChanged += (s, e) => propertyChangedFiredWithValidValues = (e.PropertyName == "AllErrors");

            var userInfoWithInvalidFirstName = new MockUserInfo()
            {
                FirstName = string.Empty,
                MiddleName = "Valid MiddleName",
                LastName = "Valid LastName",
            };
            var targetWithInvalidFirstName = new UserInfoViewModel(userInfoWithInvalidFirstName);
            targetWithInvalidFirstName.PropertyChanged += (s, e) => propertyChangedFiredWithInvalidFirstName = (e.PropertyName == "AllErrors");

            var userInfoWithInvalidMiddleName = new MockUserInfo()
            {
                FirstName = "A",
                MiddleName =  string.Empty,
                LastName = "Valid LastName",
            };
            var targetWithInvalidMiddleName = new UserInfoViewModel(userInfoWithInvalidMiddleName);
            targetWithInvalidMiddleName.PropertyChanged += (s, e) => propertyChangedFiredWithInvalidMiddleName = (e.PropertyName == "AllErrors");

            var userInfoWithInvalidLastName = new MockUserInfo()
            {
                FirstName = "Valid FirstName",
                MiddleName = "Valid MiddleName",
                LastName = string.Empty,
            };
            var targetWithInvalidLastName = new UserInfoViewModel(userInfoWithInvalidLastName);
            targetWithInvalidLastName.PropertyChanged += (s, e) => propertyChangedFiredWithInvalidLastName = (e.PropertyName == "AllErrors");

            // For all the cases, call ValidateAsync
            // Then, check if the AllErrors collection has changed

            await targetWithValidValues.ValidateAsync();
            await targetWithInvalidFirstName.ValidateAsync();
            await targetWithInvalidMiddleName.ValidateAsync();
            await targetWithInvalidLastName.ValidateAsync();

            Assert.IsFalse(propertyChangedFiredWithValidValues);
            Assert.IsTrue(propertyChangedFiredWithInvalidFirstName);
            Assert.IsTrue(propertyChangedFiredWithInvalidMiddleName);
            Assert.IsTrue(propertyChangedFiredWithInvalidLastName);
        }

        #endregion
    }
}
