// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Kona.UILogic.Tests.Mocks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Kona.UILogic.Tests.Repositories
{
    [TestClass]
    public class ProductCatalogRepositoryFixture
    {
        [TestMethod]
        public async Task GetCategories_Calls_Service_When_Cache_Miss()
        {
            var cacheService = new MockCacheService();
            cacheService.DataExistsAndIsValidAsyncDelegate = s => Task.FromResult(false);
            cacheService.SaveExternalDataAsyncDelegate = (uri, s) => Task.FromResult(new Uri("http://test.org"));
            cacheService.SaveDataAsyncDelegate = (s, c) => Task.FromResult(new Uri("http://test.org"));

            var productCatalogService = new MockProductCatalogService();
            var categories = new List<Category>
            {
                new Category{ CategoryId = 1},
                new Category{ CategoryId = 2}
            };

            productCatalogService.GetCategoriesAsyncDelegate = (depth) => Task.FromResult(new ObservableCollection<Category>(categories));
            productCatalogService.GetSubcategoriesAsyncDelegate = (i) => Task.FromResult(new ObservableCollection<Category>());
            
            var target = new ProductCatalogRepository(productCatalogService, cacheService);
            var returnedCategories = await target.GetCategoriesAsync();

            Assert.AreEqual(2, returnedCategories.Count);
            Assert.AreEqual(1, returnedCategories[0].CategoryId);
            Assert.AreEqual(2, returnedCategories[1].CategoryId);
        }

        [TestMethod]
        public async Task GetCategories_Uses_Cache_When_Data_Available()
        {
            var cacheService = new MockCacheService();
            cacheService.DataExistsAndIsValidAsyncDelegate = s => Task.FromResult(true);
            
            var categories = new List<Category>
            {
                new Category{ CategoryId = 1},
                new Category{ CategoryId = 2}
            };

            cacheService.GetDataDelegate = (string s) =>
            {
                if (s == "Categories")
                    return new ObservableCollection<Category>(categories);

                return new ObservableCollection<Category>(null);
            };

            var productCatalogService = new MockProductCatalogService();
            productCatalogService.GetCategoriesAsyncDelegate = (depth) => Task.FromResult(new ObservableCollection<Category>(null));

            var target = new ProductCatalogRepository(productCatalogService, cacheService);

            var returnedCategories = await target.GetCategoriesAsync();

            Assert.AreEqual(2, returnedCategories.Count);
            Assert.AreEqual(1, returnedCategories[0].CategoryId);
            Assert.AreEqual(2, returnedCategories[1].CategoryId);
        }

        [TestMethod]
        public async Task GetCategories_Saves_Data_To_Cache()
        {
            var cacheService = new MockCacheService();
            cacheService.DataExistsAndIsValidAsyncDelegate = s => Task.FromResult(false);
            cacheService.SaveExternalDataAsyncDelegate = (uri, s) => Task.FromResult(new Uri("http://test.org"));

            cacheService.SaveDataAsyncDelegate = (s, o) =>
            {
                ObservableCollection<Category> collection = (ObservableCollection<Category>)o;
                Assert.AreEqual("Categories", s);
                Assert.AreEqual(2, collection.Count);
                Assert.AreEqual(1, collection[0].CategoryId);
                Assert.AreEqual(2, collection[1].CategoryId);
                return Task.FromResult(new Uri("http://test.org"));
            };

            var productCatalogService = new MockProductCatalogService();
            var categories = new List<Category>
                                 {
                                     new Category{ CategoryId = 1},
                                     new Category{ CategoryId = 2}
                                 };
            productCatalogService.GetCategoriesAsyncDelegate = (depth) => Task.FromResult(new ObservableCollection<Category>(categories));
            productCatalogService.GetSubcategoriesAsyncDelegate =
                i => Task.FromResult(new ObservableCollection<Category>());

            var target = new ProductCatalogRepository(productCatalogService, cacheService);

            await target.GetCategoriesAsync();
        }

        [TestMethod]
        public async Task GetSubcategories_Calls_Service_When_Cache_Miss()
        {
            var cacheService = new MockCacheService();
            cacheService.DataExistsAndIsValidAsyncDelegate = s => Task.FromResult(false);
            cacheService.SaveExternalDataAsyncDelegate = (uri, s) => Task.FromResult(new Uri("http://test.org"));
            cacheService.SaveDataAsyncDelegate = (s, c) => Task.FromResult(new Uri("http://test.org"));

            var productCatalogService = new MockProductCatalogService();
            var subCategories = new List<Category>
                                 {
                                     new Category{ CategoryId = 10},
                                     new Category{ CategoryId = 11}
                                 };
            productCatalogService.GetSubcategoriesAsyncDelegate = (i) => Task.FromResult(new ObservableCollection<Category>(subCategories));

            var target = new ProductCatalogRepository(productCatalogService, cacheService);

            var returnedSubcategories = await target.GetSubcategoriesAsync(1);

            Assert.AreEqual(2, returnedSubcategories.Count);
            Assert.AreEqual(10, returnedSubcategories[0].CategoryId);
            Assert.AreEqual(11, returnedSubcategories[1].CategoryId);
        }

        [TestMethod]
        public async Task GetSubcategories_Uses_Cache_When_Data_Available()
        {
            var cacheService = new MockCacheService();
            cacheService.DataExistsAndIsValidAsyncDelegate = s => Task.FromResult(true);
            var categories = new List<Category>
                                 {
                                     new Category{ CategoryId = 10},
                                     new Category{ CategoryId = 11}
                                 };
            cacheService.GetDataDelegate = s =>
            {
                if (s == "SubCategoriesOfCategoryId1")
                    return new ObservableCollection<Category>(categories);

                return new ObservableCollection<Category>(null);
            };

            var productCatalogService = new MockProductCatalogService();
            productCatalogService.GetSubcategoriesAsyncDelegate = (i) => Task.FromResult(new ObservableCollection<Category>(null));

            var target = new ProductCatalogRepository(productCatalogService, cacheService);

            var returnedCategories = await target.GetSubcategoriesAsync(1);

            Assert.AreEqual(2, returnedCategories.Count);
            Assert.AreEqual(10, returnedCategories[0].CategoryId);
            Assert.AreEqual(11, returnedCategories[1].CategoryId);
        }

        [TestMethod]
        public async Task GetSubcategories_Saves_Data_To_Cache()
        {
            var cacheService = new MockCacheService();
            cacheService.DataExistsAndIsValidAsyncDelegate = s => Task.FromResult(false);
            cacheService.SaveExternalDataAsyncDelegate = (uri, s) => Task.FromResult(new Uri("http://test.org"));
            cacheService.SaveDataAsyncDelegate = (s, o) => 
            {
                ObservableCollection<Category> collection = (ObservableCollection<Category>)o;
                Assert.AreEqual("SubCategoriesOfCategoryId1", s);
                Assert.AreEqual(2, collection.Count);
                Assert.AreEqual(10, collection[0].CategoryId);
                Assert.AreEqual(11, collection[1].CategoryId);
                return Task.FromResult(new Uri("http://test.org"));
            };

            var productCatalogService = new MockProductCatalogService();
            var subCategories = new List<Category>
                                 {
                                     new Category{ CategoryId = 10},
                                     new Category{ CategoryId = 11}
                                 };
            productCatalogService.GetSubcategoriesAsyncDelegate = (i) => Task.FromResult(new ObservableCollection<Category>(subCategories));

            var target = new ProductCatalogRepository(productCatalogService, cacheService);

            await target.GetSubcategoriesAsync(1);
        }
    }
}
