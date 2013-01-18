// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.Threading.Tasks;
using Kona.UILogic.Events;
using Kona.UILogic.Models;
using Kona.UILogic.Tests.Mocks;
using Kona.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI.Xaml.Navigation;

namespace Kona.UILogic.Tests.ViewModels
{
    [TestClass]
    public class ShoppingCartTabUserControlViewModelFixture
    {
        [TestMethod]
        public void NavigatedTo_CalculatesTotalNumberOfItemsInCart()
        {
            var shoppingCart = new ShoppingCart(new List<ShoppingCartItem>()
                                               {
                                                   new ShoppingCartItem {Quantity = 1},
                                                   new ShoppingCartItem {Quantity = 2}
                                               });
            var shoppingCartRepository = new MockShoppingCartRepository();
            var eventAggregator = new MockEventAggregator();
            eventAggregator.GetEventDelegate = type => new ShoppingCartUpdatedEvent();
            shoppingCartRepository.GetShoppingCartAsyncDelegate = () => Task.FromResult(shoppingCart);
            var target = new ShoppingCartTabUserControlViewModel(shoppingCartRepository, eventAggregator, null);

            Assert.AreEqual(3, target.ItemCount);
        }

        [TestMethod]
        public void VMListensToShoppingCartUpdatedEvent_ThenCalculatesTotalNumberOfItemsInCart()
        {
            var shoppingCart = new ShoppingCart(new List<ShoppingCartItem>());
            var shoppingCartRepository = new MockShoppingCartRepository();

            shoppingCartRepository.GetShoppingCartAsyncDelegate = () => Task.FromResult(shoppingCart);
            var shoppingCartUpdatedEvent = new ShoppingCartUpdatedEvent();
            var eventAggregator = new MockEventAggregator();
            eventAggregator.GetEventDelegate = type => shoppingCartUpdatedEvent;
            shoppingCartRepository.GetShoppingCartAsyncDelegate = () => Task.FromResult(shoppingCart);
            var target = new ShoppingCartTabUserControlViewModel(shoppingCartRepository, eventAggregator, null);

            shoppingCart = new ShoppingCart(new List<ShoppingCartItem>()
                                               {
                                                   new ShoppingCartItem {Quantity = 1},
                                                   new ShoppingCartItem {Quantity = 2}
                                               });

            Assert.AreEqual(0, target.ItemCount);

            shoppingCartUpdatedEvent.Publish("payload");

            Assert.AreEqual(3, target.ItemCount);

        }

        [TestMethod]
        public void ExecutingShoppingCartCommand_NavigatesToShoppingCart()
        {
            var navigateCalled = false;
            var navigationService = new MockNavigationService();
            navigationService.NavigateDelegate = (s, o) =>
                                                     {
                                                         Assert.AreEqual("ShoppingCart", s);
                                                         navigateCalled = true;
                                                         return true;
                                                     };
            var eventAggregator = new MockEventAggregator();
            eventAggregator.GetEventDelegate = type => { return new ShoppingCartUpdatedEvent(); };
            var shoppingCartRepository = new MockShoppingCartRepository();
            shoppingCartRepository.GetShoppingCartAsyncDelegate = () => Task.FromResult(new ShoppingCart(null));
            var target = new ShoppingCartTabUserControlViewModel(shoppingCartRepository, eventAggregator, navigationService);
            target.ShoppingCartTabCommand.Execute();

            Assert.IsTrue(navigateCalled);
        }
    }
}
