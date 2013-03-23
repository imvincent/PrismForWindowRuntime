// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Tests.Mocks;
using AdventureWorks.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI.Xaml.Navigation;

namespace AdventureWorks.UILogic.Tests.ViewModels
{
    [TestClass]
    public class ShippingAddressUserControlViewModelFixture
    {
        [TestMethod]
        public void OnNavigateTo_LoadsDefault_IfTryLoadDefaultTrue()
        {
            var defaultAddress = new Address
                                     {
                                         FirstName = "FirstName"
                                     };
            var checkoutDataRepository = new MockCheckoutDataRepository();
            checkoutDataRepository.GetDefaultShippingAddressAsyncDelegate = () => Task.FromResult(defaultAddress);
            var locationService = new MockLocationService();
            var resourceLoader = new MockResourceLoader();
            var target = new ShippingAddressUserControlViewModel(checkoutDataRepository, locationService, resourceLoader, null);

            target.OnNavigatedTo(null, NavigationMode.New, new Dictionary<string, object>());
            Assert.IsNull(target.Address.FirstName);

            target.SetLoadDefault(true);
            target.OnNavigatedTo(null, NavigationMode.New, new Dictionary<string, object>());
            Assert.AreEqual("FirstName", target.Address.FirstName);
        }
    }
}
