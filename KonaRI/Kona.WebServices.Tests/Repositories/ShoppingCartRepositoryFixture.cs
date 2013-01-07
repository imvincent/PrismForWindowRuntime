// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Linq;
using Kona.WebServices.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Kona.WebServices.Tests.Repositories
{
    [TestClass]
    public class ShoppingCartRepositoryFixture
    {
        [TestMethod]
        public void AddProductToCart_AddsNewShoppingCartItem()
        {
            var target = new ShoppingCartRepository();
            var preAddcart = target.GetShoppingCart("TestUser");
            Assert.IsNull(preAddcart);

            var shoppingCartItem = target.AddProductToCart("TestUser", "BB-7421");
            
            var postAddcart = target.GetShoppingCart("TestUser");
            Assert.IsNotNull(postAddcart);
            Assert.AreEqual(1, postAddcart.ShoppingCartItems.Count);
            Assert.AreEqual("BB-7421", postAddcart.ShoppingCartItems.First().Product.ProductNumber);
            Assert.AreSame(shoppingCartItem, postAddcart.ShoppingCartItems.First());
        }

        [TestMethod]
        public void AddProductToCart_AddsNewShoppingCartItemToExistingCart()
        {
            var target = new ShoppingCartRepository();
            target.AddProductToCart("TestUser", "BB-7421");
            target.AddProductToCart("TestUser", "BB-8107");
            var cart = target.GetShoppingCart("TestUser");
            Assert.IsNotNull(cart);
            Assert.AreEqual(2, cart.ShoppingCartItems.Count);
            Assert.IsNotNull(cart.ShoppingCartItems.First(item => item.Product.ProductNumber == "BB-7421"));
            Assert.IsNotNull(cart.ShoppingCartItems.First(item => item.Product.ProductNumber == "BB-8107"));
        }

        [TestMethod]
        public void AddProductToCart_AddsNewShoppingCartItemToExistingCart_WithSameProduct()
        {
            var target = new ShoppingCartRepository();
            target.AddProductToCart("TestUser", "BB-7421");
            target.AddProductToCart("TestUser", "BB-7421");
            var cart = target.GetShoppingCart("TestUser");
            Assert.IsNotNull(cart);
            Assert.AreEqual(1, cart.ShoppingCartItems.Count);
            Assert.IsNotNull(cart.ShoppingCartItems.First(item => item.Product.ProductNumber == "BB-7421"));
            Assert.AreEqual(2, cart.ShoppingCartItems.First(item => item.Product.ProductNumber == "BB-7421").Quantity);
        }
    }
}
