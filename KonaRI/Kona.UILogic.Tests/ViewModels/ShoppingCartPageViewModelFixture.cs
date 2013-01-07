// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.Services;
using Kona.UILogic.Tests.Mocks;
using Kona.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI.Xaml.Navigation;

namespace Kona.UILogic.Tests.ViewModels
{
    [TestClass]
    public class ShoppingCartPageViewModelFixture
    {
        [TestMethod]
        public void OnNavigatedTo_Fill_Properties_No_Shopping_Cart_Items()
        {
            var shoppingCartRepository = new MockShoppingCartRepository();
            var navigationService = new MockNavigationService();

            shoppingCartRepository.GetShoppingCartAsyncDelegate = () =>
            {
                ShoppingCart shoppingCart = new ShoppingCart(new ObservableCollection<ShoppingCartItem>()) { FullPrice = 200, TotalDiscount = 100, Currency = "USD"};
                return Task.FromResult(shoppingCart);
            };

            var target = new ShoppingCartPageViewModel(shoppingCartRepository, navigationService, new MockAccountService());
            target.OnNavigatedTo(null, NavigationMode.New, null);

            Assert.AreEqual("$200.00", target.FullPrice);
            Assert.AreEqual("$100.00", target.TotalDiscount);
            Assert.AreEqual(0, target.ShoppingCartItemViewModels.Count);
        }

        [TestMethod]
        public void OnNavigatedTo_Fill_Properties_With_Shopping_Cart_Items()
        {
            var shoppingCartRepository = new MockShoppingCartRepository();
            var navigationService = new MockNavigationService();

            shoppingCartRepository.GetShoppingCartAsyncDelegate = () =>
            {
                ShoppingCart shoppingCart = null;
                var shoppingCartItems = new ObservableCollection<ShoppingCartItem>
                                            {
                                                new ShoppingCartItem() { Product = new Product { ImageName = "image" }, Currency = "USD"}, 
                                                new ShoppingCartItem() { Product = new Product { ImageName = "image" }, Currency = "USD"}
                                            };
                shoppingCart = new ShoppingCart(shoppingCartItems) { FullPrice = 200, TotalDiscount = 100, Currency = "USD"};

                return Task.FromResult(shoppingCart);
            };

            var target = new ShoppingCartPageViewModel(shoppingCartRepository, navigationService, new MockAccountService());
            target.OnNavigatedTo(null, NavigationMode.New, null);

            Assert.AreEqual("$200.00", target.FullPrice);
            Assert.AreEqual("$100.00", target.TotalDiscount);
            Assert.AreEqual(2, target.ShoppingCartItemViewModels.Count);
        }

        [TestMethod]
        public void ShoppingCartUpdated_WhenUserChanged()
        {
            var shoppingCartRepository = new MockShoppingCartRepository();
            var navigationService = new MockNavigationService();
            var accountService = new MockAccountService();

            accountService.GetUserIdDelegate = () => "User1";

            shoppingCartRepository.GetShoppingCartAsyncDelegate = () =>
            {
                ShoppingCart shoppingCart = new ShoppingCart(new ObservableCollection<ShoppingCartItem>()) { FullPrice = 100, TotalDiscount = 100, Currency = "USD"};
                return Task.FromResult(shoppingCart);
            };

            var target = new ShoppingCartPageViewModel(shoppingCartRepository, navigationService, accountService);
            target.OnNavigatedTo(null, NavigationMode.New, null);  

            Assert.AreEqual("$100.00", target.FullPrice);

            shoppingCartRepository.GetShoppingCartAsyncDelegate = () =>
            {
                ShoppingCart shoppingCart = new ShoppingCart(new ObservableCollection<ShoppingCartItem>()) { FullPrice = 200, TotalDiscount = 200, Currency = "USD"};
                return Task.FromResult(shoppingCart);
            };

            accountService.RaiseUserChanged(new UserInfo {UserName = "User2"});

            Assert.AreEqual("$200.00", target.FullPrice);

        }

        // Note: The remove method is a WIP
        //[TestMethod]
        //public void Remove_Item_From_Collection()
        //{
        //    var shoppingCartRepository = new MockShoppingCartRepository();
        //    var navigationService = new MockNavigationService();
        //    var accountManager = new MockAccountManager();

        //    shoppingCartRepository.GetShoppingCartAsyncDelegate = (userId) =>
        //    {
        //        ShoppingCart shoppingCart = null;
        //        var shoppingCartItems = new ObservableCollection<ShoppingCartItem> { new ShoppingCartItem() { Item = new Item() }, new ShoppingCartItem() { Item = new Item() } };
        //        shoppingCart = new ShoppingCart(shoppingCartItems) { FullPrice = 200, TotalDiscount = 100 };

        //        return Task.FromResult(shoppingCart);
        //    };

        //    shoppingCartRepository.RemoveItemFromShoppingCartAsyncDelegate = (userId, itemId)  =>
        //    {
        //        return Task.FromResult(true);
        //    };

        //    var viewModel = new ShoppingCartPageViewModel(shoppingCartRepository, navigationService, accountManager);
        //    viewModel.OnNavigatedTo("JohnDoe", NavigationMode.New, null);
        //    Assert.AreEqual(2, viewModel.ShoppingCartItemViewModels.Count);
        //    var cartItem = viewModel.ShoppingCartItemViewModels.First();
        //    viewModel.Remove(cartItem);
        //    Assert.AreEqual(1, viewModel.ShoppingCartItemViewModels.Count);
        //}

    }
}
