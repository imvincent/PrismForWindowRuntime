// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AdventureWorks.Shopper.Tests
{
    [TestClass]
    public class SampleFixture
    {
        [TestMethod]
        public void ShopperTestRunnerCheck()
        {
            //This is a simple test to validate that the test runner can execute unit tests
            //against the Shopper assembly.
            Assert.IsTrue(true);
        }
    }
}
