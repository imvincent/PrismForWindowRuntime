// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.Tests.Mocks;
using Kona.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI.Xaml.Navigation;

namespace Kona.UILogic.Tests.ViewModels
{
    [TestClass]
    public class ShoppingCartTabViewModelFixture
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

            shoppingCartRepository.GetShoppingCartAsyncDelegate = () => Task.FromResult(shoppingCart);
            var target = new ShoppingCartTabViewModel(shoppingCartRepository);

            Assert.AreEqual(3, target.ItemCount);
        }

        [TestMethod]
        public void VMListensToShoppingCartUpdatedEvent_ThenCalculatesTotalNumberOfItemsInCart()
        {
            var shoppingCart = new ShoppingCart(new List<ShoppingCartItem>());
            var shoppingCartRepository = new MockShoppingCartRepository();

            shoppingCartRepository.GetShoppingCartAsyncDelegate = () => Task.FromResult(shoppingCart);
            var target = new ShoppingCartTabViewModel(shoppingCartRepository);

            shoppingCart = new ShoppingCart(new List<ShoppingCartItem>()
                                               {
                                                   new ShoppingCartItem {Quantity = 1},
                                                   new ShoppingCartItem {Quantity = 2}
                                               });

            shoppingCartRepository.RaiseShoppingCartUpdatedEvent();

            Assert.AreEqual(3, target.ItemCount);

        }
    }
}
