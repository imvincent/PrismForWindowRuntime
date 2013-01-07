// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ValidationQuickStart.Models;

namespace ValidationQuickstart.Tests
{
    [TestClass]
    public class UserInfoFixture
    {
        #region RequiredField validation

        [TestMethod]
        public void FirstName_Is_Valid_If_Not_Empty()
        {
            var target = new UserInfo() {FirstName = "This is a valid value"};
            bool isValid = TryValidateProperty(target, "FirstName", target.FirstName);
            
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void FirstName_Is_Not_Valid_If_Empty()
        {
            var target = new UserInfo();
            bool isValid = TryValidateProperty(target, "FirstName", target.FirstName);

            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void LastName_Is_Valid_If_Not_Empty()
        {
            var target = new UserInfo() { LastName = "This is a valid value" };
            bool isValid = TryValidateProperty(target, "LastName", target.LastName);

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void LastName_Is_Not_Valid_If_Empty()
        {
            var target = new UserInfo();
            bool isValid = TryValidateProperty(target, "LastName", target.LastName);

            Assert.IsFalse(isValid);
        }

        #endregion

        #region RegularExpression Validation

        [TestMethod]
        public void FirstName_Is_Valid_If_Contains_Valid_Characters()
        {
            // We allow unicode characters
            // as well as internal spaces and hypens, as long as these do not occur in sequences
            var target = new UserInfo() { FirstName = "This is-a-valid value" };
            bool isValid = TryValidateProperty(target, "FirstName", target.FirstName);

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void FirstName_Is_InValid_If_Contains_InValid_Characters()
        {
            // We allow unicode characters
            // as well as internal spaces and hypens, as long as these do not occur in sequences
            var target1 = new UserInfo() { FirstName = "-Invalid value" };
            var target2 = new UserInfo() { FirstName = "Invalid value-" };
            var target3 = new UserInfo() { FirstName = "Invalid  value" };
            var target4 = new UserInfo() { FirstName = "Invalid--value" };
            var target5 = new UserInfo() { FirstName = "?Invalid value" };
            var target6 = new UserInfo() { FirstName = "Invalid value*" };
            var target7 = new UserInfo() { FirstName = "invalid !#$%i&/()= value" };

            Assert.IsFalse(TryValidateProperty(target1, "FirstName", target1.FirstName));
            Assert.IsFalse(TryValidateProperty(target2, "FirstName", target2.FirstName));
            Assert.IsFalse(TryValidateProperty(target3, "FirstName", target3.FirstName));
            Assert.IsFalse(TryValidateProperty(target4, "FirstName", target4.FirstName));
            Assert.IsFalse(TryValidateProperty(target5, "FirstName", target5.FirstName));
            Assert.IsFalse(TryValidateProperty(target6, "FirstName", target6.FirstName));
            Assert.IsFalse(TryValidateProperty(target7, "FirstName", target7.FirstName));
        }

        [TestMethod]
        public void MiddleName_Is_Valid_If_Contains_Valid_Characters()
        {
            // We allow unicode characters
            // as well as internal spaces and hypens, as long as these do not occur in sequences
            var target = new UserInfo() { MiddleName = "This is-a-valid value" };
            bool isValid = TryValidateProperty(target, "MiddleName", target.MiddleName);

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void MiddleName_Is_InValid_If_Contains_InValid_Characters()
        {
            // We allow unicode characters
            // as well as internal spaces and hypens, as long as these do not occur in sequences
            var target1 = new UserInfo() { MiddleName = "-Invalid value" };
            var target2 = new UserInfo() { MiddleName = "Invalid value-" };
            var target3 = new UserInfo() { MiddleName = "Invalid  value" };
            var target4 = new UserInfo() { MiddleName = "Invalid--value" };
            var target5 = new UserInfo() { MiddleName = "?Invalid value" };
            var target6 = new UserInfo() { MiddleName = "Invalid value*" };
            var target7 = new UserInfo() { MiddleName = "invalid !#$%i&/()= value" };

            Assert.IsFalse(TryValidateProperty(target1, "MiddleName", target1.MiddleName));
            Assert.IsFalse(TryValidateProperty(target2, "MiddleName", target2.MiddleName));
            Assert.IsFalse(TryValidateProperty(target3, "MiddleName", target3.MiddleName));
            Assert.IsFalse(TryValidateProperty(target4, "MiddleName", target4.MiddleName));
            Assert.IsFalse(TryValidateProperty(target5, "MiddleName", target5.MiddleName));
            Assert.IsFalse(TryValidateProperty(target6, "MiddleName", target6.MiddleName));
            Assert.IsFalse(TryValidateProperty(target7, "MiddleName", target7.MiddleName));
        }

        [TestMethod]
        public void LastName_Is_Valid_If_Contains_Valid_Characters()
        {
            // We allow unicode characters
            // as well as internal spaces and hypens, as long as these do not occur in sequences
            var target = new UserInfo() { LastName = "This is-a-valid value" };
            bool isValid = TryValidateProperty(target, "LastName", target.LastName);

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void LastName_Is_InValid_If_Contains_InValid_Characters()
        {
            // We allow unicode characters
            // as well as internal spaces and hypens, as long as these do not occur in sequences
            var target1 = new UserInfo() { LastName = "-Invalid value" };
            var target2 = new UserInfo() { LastName = "Invalid value-" };
            var target3 = new UserInfo() { LastName = "Invalid  value" };
            var target4 = new UserInfo() { LastName = "Invalid--value" };
            var target5 = new UserInfo() { LastName = "?Invalid value" };
            var target6 = new UserInfo() { LastName = "Invalid value*" };
            var target7 = new UserInfo() { LastName = "invalid !#$%i&/()= value" };

            Assert.IsFalse(TryValidateProperty(target1, "LastName", target1.LastName));
            Assert.IsFalse(TryValidateProperty(target2, "LastName", target2.LastName));
            Assert.IsFalse(TryValidateProperty(target3, "LastName", target3.LastName));
            Assert.IsFalse(TryValidateProperty(target4, "LastName", target4.LastName));
            Assert.IsFalse(TryValidateProperty(target5, "LastName", target5.LastName));
            Assert.IsFalse(TryValidateProperty(target6, "LastName", target6.LastName));
            Assert.IsFalse(TryValidateProperty(target7, "LastName", target7.LastName));
        }

        #endregion

        #region Cross-Property Validation

        [TestMethod]
        public void Empty_MiddleName_Is_Valid_If_FirstName_Is_Not_An_Initial()
        {
            var target = new UserInfo() { FirstName = "Not an initial" };
            bool isValid = TryValidateProperty(target, "MiddleName", target.MiddleName);

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void Empty_MiddleName_Is_Not_Valid_If_FirstName_Is_An_Initial()
        {
            var target = new UserInfo() { FirstName = "A" };
            bool isValid = TryValidateProperty(target, "MiddleName", target.MiddleName);

            Assert.IsFalse(isValid);
        }

        #endregion

        private bool TryValidateProperty(object entity, string propertyName, object propertyValue)
        {
            var context = new ValidationContext(entity) { MemberName = propertyName };
            var result = new List<ValidationResult>();

            return Validator.TryValidateProperty(propertyValue, context, result);
        }
    }
}
