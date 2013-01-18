// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Threading.Tasks;
using Kona.UILogic.Events;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Kona.UILogic.Tests.Mocks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Kona.UILogic.Tests.Repositories
{
    [TestClass]
    public class ShoppingCartRepositoryFixture
    {
        [TestMethod]
        public void CartUpdatedEventRaised_WhenProductAdded()
        {
            var shoppingCartUpdatedRaised = false;
            var shoppingCartService = new MockShoppingCartService();
            shoppingCartService.AddProductToShoppingCartAsyncDelegate =
                (s, s1) => Task.FromResult(new ShoppingCartItem());
            var shoppingCartUpdatedEvent = new ShoppingCartUpdatedEvent();
            shoppingCartUpdatedEvent.Subscribe((args) =>
                                                   {
                                                       shoppingCartUpdatedRaised = true;
                                                   });
            var eventAggregator = new MockEventAggregator();
            eventAggregator.GetEventDelegate = type => shoppingCartUpdatedEvent;
            var target = new ShoppingCartRepository(shoppingCartService, new MockAccountService(), eventAggregator, new MockProductCatalogRepository());

            target.AddProductToShoppingCartAsync("TestProductId");

            Assert.IsTrue(shoppingCartUpdatedRaised);
        }

        [TestMethod]
        public async Task CartUpdatedEventRaised_WhenProductRemoved()
        {
            var shoppingCartUpdatedRaised = false;
            var shoppingCartService = new MockShoppingCartService();
            shoppingCartService.RemoveShoppingCartItemDelegate =
                (s, s1) =>
                    {
                        Assert.AreEqual(123, s1);
                        return;
                    };
            var shoppingCartUpdatedEvent = new MockShoppingCartUpdatedEvent();
            shoppingCartUpdatedEvent.PublishDelegate =((args) =>
            {
                shoppingCartUpdatedRaised = true;
            });
            var eventAggregator = new MockEventAggregator();
            eventAggregator.GetEventDelegate = type => shoppingCartUpdatedEvent;
            var target = new ShoppingCartRepository(shoppingCartService, new MockAccountService(), eventAggregator, new MockProductCatalogRepository());
            
            target.RemoveShoppingCartItemAsync(123);

            Assert.IsTrue(shoppingCartUpdatedRaised);
        }

        //[TestMethod]
        //public void GetShoppingCartAsync_Offline_Gets_Valid_Cart()
        //{
        //    MockShoppingCartService shoppingCartService = new MockShoppingCartService
        //    {
        //        GetShoppingCartAsyncDelegate = (user) => { throw new Exception(); }
        //    };
        //    IShoppingCartRepository repository = new ShoppingCartRepository(shoppingCartService);
        //    var cart = repository.GetShoppingCartAsync().Result;
        //    Assert.IsNotNull(cart);
        //}

        //[TestMethod]
        //public void GetShoppingCartAsync_Online_Gets_Cart_From_Server()
        //{
        //}

        //[TestMethod]
        //public void AddProductToShoppingCartAsync_Offline_Adds_To_Client_Side_Cart()
        //{
        //}

        //[TestMethod]
        //public void AddProductToShoppingCartAsync_Online_Not_Logged_In_Adds_To_Server_Side_Temp_Cart()
        //{
        //}

        //[TestMethod]
        //public void AddProductToShoppingCartAsync_Online_Logged_In_Adds_To_Server_Side_User_Cart()
        //{
        //}

        //[TestMethod]
        //public void RemoveShoppingCartItemAsync_Offline_Adds_To_Client_Side_Cart()
        //{
        //}

        //[TestMethod]
        //public void RemoveShoppingCartItemAsync_Online_Not_Logged_In_Adds_To_Server_Side_Temp_Cart()
        //{
        //}

        //[TestMethod]
        //public void RemoveShoppingCartItemAsync_Online_Logged_In_Adds_To_Server_Side_User_Cart()
        //{
        //}
    }
}
