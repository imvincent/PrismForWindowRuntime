// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using AdventureWorks.UILogic.Tests.Mocks;
using AdventureWorks.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureWorks.UILogic.Tests.ViewModels
{
    [TestClass]
    public class OrderConfirmationPageViewModelFixture
    {
        [TestMethod]
        public async Task OnNavigatedTo_ClearsNavigationHistory()
        {
            bool clearHistoryCalled = false;
            var navigationService = new MockNavigationService();
            navigationService.ClearHistoryDelegate = () =>
            {
                clearHistoryCalled = true;
            };
            var resourcesService = new MockResourceLoader()
            {
                GetStringDelegate = (key) => key
            };
            var target = new OrderConfirmationPageViewModel(resourcesService, navigationService);
            target.OnNavigatedTo(null, Windows.UI.Xaml.Navigation.NavigationMode.Forward, null);

            Assert.IsTrue(clearHistoryCalled);
        }
    }
}
