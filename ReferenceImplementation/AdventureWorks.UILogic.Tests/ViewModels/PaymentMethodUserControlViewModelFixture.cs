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
    public class PaymentMethodUserControlViewModelFixture
    {
        [TestMethod]
        public void OnNavigateTo_LoadsDefault_IfTryLoadDefaultTrue()
        {
            var defaultPaymentMethod = new PaymentMethod
            {
                CardholderName = "CardHolderName",
                CardNumber = "32323232",
                CardVerificationCode = "222",
                Phone = "22224232",
                ExpirationMonth = "12",
                ExpirationYear = "2016"
            };
            var checkoutDataRepository = new MockCheckoutDataRepository();
            checkoutDataRepository.GetDefaultPaymentMethodAsyncDelegate = () => Task.FromResult(defaultPaymentMethod);
            var target = new PaymentMethodUserControlViewModel(checkoutDataRepository);

            target.OnNavigatedTo(null, NavigationMode.New, new Dictionary<string, object>());
            Assert.IsNull(target.PaymentMethod.CardholderName);

            target.SetLoadDefault(true);
            target.OnNavigatedTo(null, NavigationMode.New, new Dictionary<string, object>());
            Assert.AreEqual("CardHolderName", target.PaymentMethod.CardholderName);
        }
    }
}
