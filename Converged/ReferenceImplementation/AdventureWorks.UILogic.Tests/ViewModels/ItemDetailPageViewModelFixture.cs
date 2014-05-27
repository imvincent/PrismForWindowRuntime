// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Tests.Mocks;
using AdventureWorks.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI.Xaml.Navigation;
using System.Collections.Generic;

namespace AdventureWorks.UILogic.Tests.ViewModels
{
    [TestClass]
    public class ItemDetailPageViewModelFixture
    {
        [TestMethod]
        public void OnNavigatedTo_Fill_Items_And_SelectedProduct()
        {
            var repository = new MockProductCatalogRepository();

            repository.GetProductAsyncDelegate = (productNumber) =>
                {
                    Product product = null;

                    if (productNumber == "1")
                    {
                        product = new Product { ProductNumber = productNumber, SubcategoryId = 1 };
                    }

                    return Task.FromResult(product);
                };

            repository.GetProductsAsyncDelegate = (subCategoryId) =>
                {
                    ReadOnlyCollection<Product> products = null;

                    if (subCategoryId == 1)
                    {
                        products = new ReadOnlyCollection<Product>(new List<Product>
                        {
                            new Product(){ ProductNumber = "1", ImageUri = new Uri("http://image") },
                            new Product(){ ProductNumber = "2", ImageUri = new Uri("http://image") },
                            new Product(){ ProductNumber = "3", ImageUri = new Uri("http://image") }
                        });
                    }

                    return Task.FromResult(products);
                };

            var secondaryTileService = new MockSecondaryTileService() { SecondaryTileExistsDelegate = s => false};

            var target = new ItemDetailPageViewModel(repository, new MockShoppingCartRepository(), null, null, secondaryTileService);
            target.OnNavigatedTo("1", NavigationMode.New, null);

            Assert.IsNotNull(target.Items);
            Assert.AreEqual(3, ((IReadOnlyCollection<ProductViewModel>)target.Items).Count);
            Assert.AreEqual(target.Items.First(), target.SelectedProduct);
        }

        [TestMethod]
        public void OnNavigatedTo_When_Service_Not_Available_Then_Pops_Alert()
        {
            var repository = new MockProductCatalogRepository();
            var alertService = new MockAlertMessageService();
            var resourceLoader = new MockResourceLoader();

            bool alertCalled = false;
            repository.GetProductAsyncDelegate = (productNumber) =>
                {
                    throw new Exception();
                };

            repository.GetProductsAsyncDelegate = (subCategoryId) =>
                {
                    throw new Exception();
                };

            alertService.ShowAsyncDelegate = (msg, title) =>
                {
                    alertCalled = true;
                    return Task.FromResult(string.Empty);
                };

            var target = new ItemDetailPageViewModel(repository, new MockShoppingCartRepository(), alertService, resourceLoader, null);
            target.OnNavigatedTo("1", NavigationMode.New, null);

            Assert.IsTrue(alertCalled);
        }

        [TestMethod]
        public async Task PinToStart_FiresOnly_IfProductIsSelected_And_SecondaryTileDoesNotExist()
        {
            bool fired = false;
            var secondaryTileService = new MockSecondaryTileService() { ActivateTileNotificationsDelegate = (a, b, c) => Task.Delay(0) };
            var target = new ItemDetailPageViewModel(null, null, null, null, secondaryTileService);

            // Case 1: Item not selected --> should not be fired
            secondaryTileService.SecondaryTileExistsDelegate = (a) => false;
            secondaryTileService.PinSquareSecondaryTileDelegate = (a, b, c) =>
                {
                    fired = true;
                    return Task.FromResult(true);
                };
            secondaryTileService.PinWideSecondaryTileDelegate = (a, b, c) =>
                {
                    fired = true;
                    return Task.FromResult(true);
                };

            await target.PinProductCommand.Execute();
            Assert.IsFalse(fired);

            // Case 2: Item selected but SecondaryTile exists --> should not be fired
            secondaryTileService.SecondaryTileExistsDelegate = (a) => true;
            target.SelectedProduct = new ProductViewModel(new Product() { ImageUri = new Uri("http://dummy-image-uri.com") });
            
            await target.PinProductCommand.Execute();
            Assert.IsFalse(fired);

            // Case 3: Item selected and SecondaryTile does not exist --> should be fired
            secondaryTileService.SecondaryTileExistsDelegate = (a) => false;

            await target.PinProductCommand.Execute();
            Assert.IsTrue(fired);
        }

        [TestMethod]
        public async Task PinToStart_Changes_IsBottomAppBarSticky()
        {
            var secondaryTileService = new MockSecondaryTileService() 
            { 
                SecondaryTileExistsDelegate = (a) => false ,
                ActivateTileNotificationsDelegate = (a, b, c) => Task.Delay(0)
            };
            var target = new ItemDetailPageViewModel(null, null, null, null, secondaryTileService);
            target.SelectedProduct = new ProductViewModel(new Product() { ImageUri = new Uri("http://dummy-image-uri.com") });

            // The AppBar should be sticky when the item is being pinned
            secondaryTileService.PinSquareSecondaryTileDelegate = (a, b, c) =>
                {
                    Assert.IsTrue(target.IsBottomAppBarSticky);
                    return Task.FromResult(true);
                };
            secondaryTileService.PinWideSecondaryTileDelegate = (a, b, c) =>
                {
                    Assert.IsTrue(target.IsBottomAppBarSticky);
                    return Task.FromResult(true);
                };

            // Check if the AppBar is Sticky before pinning
            Assert.IsFalse(target.IsBottomAppBarSticky);

            await target.PinProductCommand.Execute();

            // Check if the AppBar is Sticky after pinning
            Assert.IsFalse(target.IsBottomAppBarSticky);
        }

        [TestMethod]
        public async Task PinToStart_ActivatesLiveTile()
        {
            var tileService = new MockSecondaryTileService()
            {
                SecondaryTileExistsDelegate = (a) => false,
                PinSquareSecondaryTileDelegate = (a, b, c) => Task.FromResult(true),
                PinWideSecondaryTileDelegate = (a, b, c) => Task.FromResult(true),
                ActivateTileNotificationsDelegate = (tileId, tileContentUri, recurrence) =>
                    {
                        Assert.IsTrue(tileId == "MyProduct");
                        Assert.IsNotNull(tileContentUri);
                        Assert.IsNotNull(recurrence);
                    }
            };
            var target = new ItemDetailPageViewModel(null, null, null, null, tileService);
            target.SelectedProduct = new ProductViewModel(new Product() { ProductNumber = "MyProduct", ImageUri = new Uri("http://dummy-image-uri.com") });

            await target.PinProductCommand.Execute();
        }

        [TestMethod]
        public async Task UnpinFromStart_FiresOnly_IfProductIsSelected_And_SecondaryTileDoesNotExist()
        {
            bool fired = false;
            var secondaryTileService = new MockSecondaryTileService();
            var target = new ItemDetailPageViewModel(null, null, null, null, secondaryTileService);

            // Case 1: Item not selected --> should not be fired
            secondaryTileService.SecondaryTileExistsDelegate = (a) => true;
            secondaryTileService.UnpinTileDelegate = (a) =>
            {
                fired = true;
                return Task.FromResult(true);
            };

            await target.UnpinProductCommand.Execute();
            Assert.IsFalse(fired);

            // Case 2: Item selected but SecondaryTile does not exist --> should not be fired
            secondaryTileService.SecondaryTileExistsDelegate = (a) => false;
            target.SelectedProduct = new ProductViewModel(new Product() { ImageUri = new Uri("http://dummy-image-uri.com") });

            await target.UnpinProductCommand.Execute();
            Assert.IsFalse(fired);

            // Case 3: Item selected and SecondaryTile exists --> should be fired
            secondaryTileService.SecondaryTileExistsDelegate = (a) => true;

            await target.UnpinProductCommand.Execute();
            Assert.IsTrue(fired);
        }

        [TestMethod]
        public async Task UnpinFromStart_Changes_IsBottomAppBarSticky()
        {
            var tileService = new MockSecondaryTileService() { SecondaryTileExistsDelegate = (a) => false };
            var target = new ItemDetailPageViewModel(null, null, null, null, tileService);
            target.SelectedProduct = new ProductViewModel(new Product() { ImageUri = new Uri("http://dummy-image-uri.com") });

            // The AppBar should be sticky when the item is being unpinned
            tileService.UnpinTileDelegate = (a) =>
                {
                    Assert.IsTrue(target.IsBottomAppBarSticky);
                    return Task.FromResult(true);
                };

            // Check if the AppBar is Sticky before unpinning
            Assert.IsFalse(target.IsBottomAppBarSticky);

            await target.UnpinProductCommand.Execute();

            // Check if the AppBar is Sticky after unpinning
            Assert.IsFalse(target.IsBottomAppBarSticky);
        }
    }
}
