// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Tests.Mocks;
using AdventureWorks.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI.Xaml.Navigation;

namespace AdventureWorks.UILogic.Tests.ViewModels
{
    [TestClass]
    public class SearchResultsPageViewModelFixture
    {
        [TestMethod]
        public void OnNavigatingTo_Search_Results_Page_With_Search_Term()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();
            repository.GetFilteredProductsAsyncDelegate = (queryString) =>
                {
                    ReadOnlyCollection<Product> products;
                    if (queryString == "bike")
                        products = new ReadOnlyCollection<Product>(new List<Product>
                        {
                            new Product(){Title = "bike1", ProductNumber = "1", ImageUri = new Uri("http://image")},
                            new Product(){Title = "bike2", ProductNumber = "2", ImageUri = new Uri("http://image")}
                        });
                    else
                    {
                        products = new ReadOnlyCollection<Product>(new List<Product>
                        {
                            new Product(){Title = "bike1", ProductNumber = "1", ImageUri = new Uri("http://image")},
                            new Product(){Title = "bike2", ProductNumber = "2", ImageUri = new Uri("http://image")},
                            new Product(){Title = "product3", ProductNumber = "3", ImageUri = new Uri("http://image")}
                        });
                    }

                    return Task.FromResult(new SearchResult(3, products));
                };

            var target = new SearchResultsPageViewModel(repository, navigationService, new MockSearchPaneService(), new MockResourceLoader(), new MockAlertMessageService());
            const string searchTerm = "bike";
            target.OnNavigatedTo(searchTerm, NavigationMode.New, null);
            Assert.AreEqual("bike", target.SearchTerm);
            Assert.IsNotNull(target.Results);
            Assert.AreEqual(2, target.Results.Count);
            var resultsThatDontMatch = target.Results.Any(p => !p.Title.Contains(searchTerm));
            Assert.IsFalse(resultsThatDontMatch);
        }

        [TestMethod]
        public void OnNavigatingTo_Search_Results_Page_Without_Search_Term()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();
            repository.GetFilteredProductsAsyncDelegate = (queryString) =>
            {
                ReadOnlyCollection<Product> products;
                if (queryString == "bike")
                    products = new ReadOnlyCollection<Product>(new List<Product>
                        {
                            new Product(){Title = "bike1", ProductNumber = "1", ImageUri = new Uri("http://image")},
                            new Product(){Title = "bike2", ProductNumber = "2", ImageUri = new Uri("http://image")}
                        });
                else
                {
                    products = new ReadOnlyCollection<Product>(new List<Product>
                        {
                            new Product(){Title = "bike1", ProductNumber = "1", ImageUri = new Uri("http://image")},
                            new Product(){Title = "bike2", ProductNumber = "2", ImageUri = new Uri("http://image")},
                            new Product(){Title = "product3", ProductNumber = "3", ImageUri = new Uri("http://image")}
                        });
                }

                return Task.FromResult(new SearchResult(3, products));
            };

            var target = new SearchResultsPageViewModel(repository, navigationService, new MockSearchPaneService(), new MockResourceLoader(), new MockAlertMessageService());
            var searchTerm = string.Empty;
            target.OnNavigatedTo(searchTerm, NavigationMode.New, null);
            Assert.AreEqual(string.Empty, target.SearchTerm);
            Assert.IsNotNull(target.Results);
            Assert.AreEqual(3, target.Results.Count);
        }

        [TestMethod]
        public void ProductNav_With_Valid_Parameter_Does_Navigate()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();
            var productToNavigate = new ProductViewModel(new Product() { ListPrice = 100, DiscountPercentage = 50, ProductNumber = "p1", ImageUri = new Uri("http://image"), Currency = "USD", Title = "My Title", Description = "My Description", });
            navigationService.NavigateDelegate = (pageName, productId) =>
            {
                Assert.AreEqual("ItemDetail", pageName);
                Assert.AreEqual(productToNavigate.ProductNumber, productId);
                return true;
            };

            var viewModel = new SearchResultsPageViewModel(repository, navigationService, null, new MockResourceLoader(), new MockAlertMessageService());
            viewModel.ProductNavigationAction.Invoke(productToNavigate);
        }

        [TestMethod]
        public void ProductNav_With_Null_Parameter_Does_Not_Navigate()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();

            navigationService.NavigateDelegate = (pageName, categoryId) =>
            {
                Assert.Fail();
                return false;
            };

            var viewModel = new SearchResultsPageViewModel(repository, navigationService, null, new MockResourceLoader(), new MockAlertMessageService());
            viewModel.ProductNavigationAction.Invoke(null);
        }
    }
}
