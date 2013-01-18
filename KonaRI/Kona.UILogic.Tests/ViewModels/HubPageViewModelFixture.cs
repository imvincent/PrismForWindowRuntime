// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.Tests.Mocks;
using Kona.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI.Xaml.Navigation;
using System.Collections.Generic;

namespace Kona.UILogic.Tests.ViewModels
{
    [TestClass]
    public class HubPageModelFixture
    {
        [TestMethod]
        public void OnNavigatedTo_Fill_RootCategories()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();

            repository.GetCategoriesAsyncDelegate = () =>
            {
                var categories = new ReadOnlyCollection<Category>(new List<Category>{
                    new Category(),
                    new Category(),
                    new Category()
                });

                return Task.FromResult(categories);
            };

            var viewModel = new HubPageViewModel(repository, navigationService);
            viewModel.OnNavigatedTo(null, NavigationMode.New, null);

            Assert.IsNotNull(viewModel.RootCategories);
            Assert.AreEqual(((ICollection<CategoryViewModel>)viewModel.RootCategories).Count, 3);
        }

        [TestMethod]
        public void CategoryNav_With_Valid_Parameter()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();

            navigationService.NavigateDelegate = (pageName, categoryId) =>
            {
                Assert.AreEqual("GroupDetailPage", pageName);
                Assert.AreEqual(1, categoryId);
                return true;
            };

            var viewModel = new HubPageViewModel(repository, navigationService);
            viewModel.CategoryNavigationAction.Invoke(1);
        }

        [TestMethod]
        public void CategoryNav_With_Null_Parameter()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();

            navigationService.NavigateDelegate = (pageName, categoryId) =>
            {
                Assert.Fail();
                return false;
            };
                
            var viewModel = new HubPageViewModel(repository, navigationService);
            viewModel.CategoryNavigationAction.Invoke(null);
        }
    }
}
