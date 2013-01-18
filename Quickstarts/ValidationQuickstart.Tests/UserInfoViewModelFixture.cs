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
        [TestMethod]
        public void Call_To_Validate_Fires_Validation_For_All_The_Properties()
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

            // For all the cases, call Validate
            // Then, check if the AllErrors collection has changed

            targetWithValidValues.Validate();
            targetWithInvalidFirstName.Validate();
            targetWithInvalidMiddleName.Validate();
            targetWithInvalidLastName.Validate();

            targetWithInvalidFirstName.Validate();
            Assert.IsTrue(propertyChangedFiredWithInvalidFirstName);

            targetWithInvalidMiddleName.Validate();
            Assert.IsTrue(propertyChangedFiredWithInvalidMiddleName);

            targetWithInvalidLastName.Validate();
            Assert.IsTrue(propertyChangedFiredWithInvalidLastName);
        }
    }
}
