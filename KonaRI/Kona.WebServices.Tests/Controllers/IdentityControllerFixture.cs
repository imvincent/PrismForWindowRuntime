// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Net.Http;
using System.Threading.Tasks;
using Kona.WebServices.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kona.WebServices.Tests.Controllers
{
    [TestClass]
    public class IdentityControllerFixture
    {
        [TestMethod]
        public void Validate_User_Name()
        {
            var controller = new IdentityController();
            var result = controller.GetIsValid("JohnDoe", "pwd");
            Assert.IsNotNull(result);
            Assert.IsTrue(result != null);
            Assert.AreEqual(result.UserInfo.UserName, "JohnDoe");
        }

        [TestMethod]
        public void Do_Not_Validate_Non_Existent_User()
        {
            var controller = new IdentityController();
            var result = controller.GetIsValid("UnknownUser", string.Empty);
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsValid);
            Assert.IsNull(result.UserInfo);
        }
    }
}
