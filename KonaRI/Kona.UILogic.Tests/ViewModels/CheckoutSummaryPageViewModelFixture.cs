// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.Tests.Mocks;
using Kona.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI.Xaml.Navigation;

namespace Kona.UILogic.Tests.ViewModels
{
    [TestClass]
    public class CheckoutSummaryPageViewModelFixture
    {

        [TestMethod]
        public void SubmitValidOrder_CallSuccessDialog()
        {
            // TODO
        }

        [TestMethod]
        public void SubmitValidOrder_CallErrorDialog()
        {
            // TODO
        }

        [TestMethod]
        public void Submit_WhenAnonymous_ShowsSignInFlyout()
        {
            var showFlyoutCalled = false;
            var navigationService = new MockNavigationService();
            var accountService = new MockAccountService();
            accountService.GetSignedInUserAsyncDelegate = () => Task.FromResult<UserInfo>(null);
            var settingsCharmService = new MockSettingsCharmService();
            settingsCharmService.ShowFlyoutDelegate = (s, o, arg3) =>
            {
                showFlyoutCalled = true;
                Assert.AreEqual("signIn", s);
            };
            var target = new CheckoutSummaryPageViewModel(navigationService, null, null, null, null, accountService, settingsCharmService, null, null);

            target.SubmitCommand.Execute();

            Assert.IsTrue(showFlyoutCalled);
        }

        [TestMethod]
        public void SelectShippingMethod_Recalculates_Order()
        {
            var shippingMethods = new List<ShippingMethod>() { new ShippingMethod() { Id = 1,  Cost = 0 } };
            var shoppingCartItems = new List<ShoppingCartItem>() { new ShoppingCartItem() {  Quantity = 1, Currency = "USD", Product = new Product() } };
            var mockOrder = new Order()
            {
                ShoppingCart = new ShoppingCart(shoppingCartItems) { Currency = "USD", TotalPrice = 100 },
                ShippingAddress = new Address(),
                BillingAddress = new Address(),
                PaymentMethod = new PaymentMethod() { CardNumber = "1234" },
                ShippingMethod = shippingMethods.First()
            };
            var mockShippingMethodService = new MockShippingMethodService() 
            {
                GetShippingMethodsAsyncDelegate = () => Task.FromResult<IEnumerable<ShippingMethod>>(shippingMethods) 
            };

            var target = new CheckoutSummaryPageViewModel(new MockNavigationService(), new MockOrderService(), mockShippingMethodService,
                                                          null, new MockShoppingCartRepository(),
                                                          new MockAccountService(), new MockSettingsCharmService(), new MockResourceLoader(), null);

            target.OnNavigatedTo(mockOrder, NavigationMode.New, null);

            Assert.AreEqual("$0.00", target.ShippingCost);
            Assert.AreEqual("$100.00", target.OrderSubtotal);
            Assert.AreEqual("$100.00", target.GrandTotal);

            target.SelectedShippingMethod = new ShippingMethod() { Cost = 10 };

            Assert.AreEqual("$10.00", target.ShippingCost);
            Assert.AreEqual("$100.00", target.OrderSubtotal);
            Assert.AreEqual("$110.00", target.GrandTotal);

        }

        [TestMethod]
        public void SelectCheckoutData_Opens_AppBar()
        {
            var shippingMethods = new List<ShippingMethod>() { new ShippingMethod() { Id = 1, Cost = 0 } };
            var shoppingCartItems = new List<ShoppingCartItem>() { new ShoppingCartItem() { Quantity = 1, Currency = "USD", Product = new Product() } };
            var mockOrder = new Order()
            {
                ShoppingCart = new ShoppingCart(shoppingCartItems) { Currency = "USD", FullPrice = 100 },
                ShippingAddress = new Address(),
                BillingAddress = new Address(),
                PaymentMethod = new PaymentMethod() { CardNumber = "1234" },
                ShippingMethod = shippingMethods.First()
            };
            var mockShippingMethodService = new MockShippingMethodService()
            {
                GetShippingMethodsAsyncDelegate = () => Task.FromResult<IEnumerable<ShippingMethod>>(shippingMethods)
            };
            
            var target = new CheckoutSummaryPageViewModel(new MockNavigationService(), new MockOrderService(), mockShippingMethodService,
                                                          null, new MockShoppingCartRepository(),
                                                          new MockAccountService(), new MockSettingsCharmService(), new MockResourceLoader(), null);

            target.OnNavigatedTo(mockOrder, NavigationMode.New, null);
            Assert.IsFalse(target.IsBottomAppBarOpened);

            target.SelectedCheckoutData = target.CheckoutDataViewModels.First();
            Assert.IsTrue(target.IsBottomAppBarOpened);
        }

        [TestMethod]
        public void EditCheckoutData_Calls_Proper_Flyout()
        {
            string requestedFlyoutName = string.Empty;
            var mockSettingsCharmService = new MockSettingsCharmService() { ShowFlyoutDelegate = (flyoutName, a, b) => Assert.IsTrue(flyoutName == requestedFlyoutName) };

            var target = new CheckoutSummaryPageViewModel(new MockNavigationService(), null, null, null, null, null, mockSettingsCharmService, null, null);

            requestedFlyoutName = "editShippingAddress";
            target.SelectedCheckoutData = new CheckoutDataViewModel(null, null, null, null, null, null, Constants.ShippingAddress, null);
            target.EditCheckoutDataCommand.Execute().Wait();

            requestedFlyoutName = "editBillingAddress";
            target.SelectedCheckoutData = new CheckoutDataViewModel(null, null, null, null, null, null, Constants.BillingAddress, null);
            target.EditCheckoutDataCommand.Execute().Wait();

            requestedFlyoutName = "editPaymentMethod";
            target.SelectedCheckoutData = new CheckoutDataViewModel(null, null, null, null, null, null, Constants.PaymentMethod, null);
            target.EditCheckoutDataCommand.Execute().Wait();
        }

        [TestMethod]
        public void EditCheckoutData_Updates_Order()
        {
            var shippingMethods = new List<ShippingMethod>() { new ShippingMethod() { Id = 1, Cost = 0 } };
            var shoppingCartItems = new List<ShoppingCartItem>() { new ShoppingCartItem() { Quantity = 1, Currency = "USD", Product = new Product() } };
            var mockOrder = new Order()
            {
                ShoppingCart = new ShoppingCart(shoppingCartItems) { Currency = "USD", FullPrice = 100 },
                ShippingAddress = new Address(),
                BillingAddress = new Address(),
                PaymentMethod = new PaymentMethod() { CardNumber = "1234" },
                ShippingMethod = shippingMethods.First()
            };
            var mockShippingMethodService = new MockShippingMethodService()
            {
                GetShippingMethodsAsyncDelegate = () => Task.FromResult<IEnumerable<ShippingMethod>>(shippingMethods)
            };
            var mockSettingsCharmService = new MockSettingsCharmService();
            mockSettingsCharmService.ShowFlyoutDelegate = (flyoutName, param, success) =>
            { 
                // Update CheckoutData information and call success
                var paymentMethod = param as PaymentMethod;
                paymentMethod.CardNumber = "5678";
                success.Invoke();
            };
            
            var target = new CheckoutSummaryPageViewModel(new MockNavigationService(), new MockOrderService(), mockShippingMethodService,
                                                          null, new MockShoppingCartRepository(),
                                                          new MockAccountService(), mockSettingsCharmService, new MockResourceLoader(), null);

            target.OnNavigatedTo(mockOrder, NavigationMode.New, null);
            target.SelectedCheckoutData = target.CheckoutDataViewModels[2];
            target.EditCheckoutDataCommand.Execute().Wait();

            // Check if order information has changed
            Assert.IsTrue(mockOrder.PaymentMethod.CardNumber == "5678");
            Assert.IsTrue(((PaymentMethod)target.CheckoutDataViewModels[2].Context).CardNumber == "5678");
        }

        [TestMethod]
        public void AddCheckoutData_Calls_Proper_Flyout()
        {
            string requestedFlyoutName = string.Empty;
            var mockSettingsCharmService = new MockSettingsCharmService()
            {
                ShowFlyoutDelegate = (flyoutName, action, b) =>
                {
                    Assert.IsTrue(flyoutName == requestedFlyoutName);
                }
            };

            var target = new CheckoutSummaryPageViewModel(new MockNavigationService(), null, null, null, null, null, mockSettingsCharmService, null, null);

            requestedFlyoutName = "addShippingAddress";
            target.SelectedCheckoutData = new CheckoutDataViewModel(null, null, null, null, null, null, Constants.ShippingAddress, null);
            target.AddCheckoutDataCommand.Execute().Wait();

            requestedFlyoutName = "addBillingAddress";
            target.SelectedCheckoutData = new CheckoutDataViewModel(null, null, null, null, null, null, Constants.BillingAddress, null);
            target.AddCheckoutDataCommand.Execute().Wait();

            requestedFlyoutName = "addPaymentMethod";
            target.SelectedCheckoutData = new CheckoutDataViewModel(null, null, null, null, null, null, Constants.PaymentMethod, null);
            target.AddCheckoutDataCommand.Execute().Wait();
        }
    }
}
