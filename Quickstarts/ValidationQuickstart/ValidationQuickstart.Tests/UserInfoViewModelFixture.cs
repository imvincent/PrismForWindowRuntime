// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ValidationQuickStart.ViewModels;

namespace ValidationQuickstart.Tests
{
    [TestClass]
    public class UserInfoViewModelFixture
    {
        [TestMethod]
        public void Validate_UpdatesAllErrors()
        {
            var validatePropertiesCalled = false;
            var userInfo = new MockUserInfo();
            var errorDictionary = new Dictionary<string, ReadOnlyCollection<string>>();
            errorDictionary.Add("firstfield", new ReadOnlyCollection<string>(new List<string>{"error message1"}));
            errorDictionary.Add("secondfield", new ReadOnlyCollection<string>(new List<string>{"error message2"}));
            userInfo.ValidatePropertiesDelegate = () =>
                                                      {
                                                          validatePropertiesCalled = true;
                                                          userInfo.RaiseErrorsChanged();
                                                          return false;
                                                      };
            userInfo.GetAllErrorsDelegate = 
                () => new ReadOnlyDictionary<string, ReadOnlyCollection<string>>(errorDictionary);

            var target = new UserInfoViewModel(userInfo);

            Assert.AreEqual(0, target.AllErrors.Count);

            target.ValidateCommand.Execute(null);

            Assert.AreEqual(2, target.AllErrors.Count);
            Assert.IsTrue(validatePropertiesCalled);
        }
    }
}
