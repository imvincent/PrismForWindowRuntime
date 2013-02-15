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
using System.Net.Http;

namespace Kona.UILogic.Tests.ViewModels
{
    [TestClass]
    public class GroupDetailPageViewModelFixture
    {
        [TestMethod]
        public void OnNavigatedTo_Fill_Items_And_Title()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();

            repository.GetCategoryAsyncDelegate = (categoryId) =>
            {
                Category category = null;

                if (categoryId == 1)
                {
                    category = new Category { Id = categoryId, Title = "CategoryTitle" };
                }

                return Task.FromResult(category);
            };

            repository.GetSubcategoriesAsyncDelegate = (categoryId) =>
            {
                ReadOnlyCollection<Category> categories = null;

                if (categoryId == 1)
                {
                    categories = new ReadOnlyCollection<Category>(new List<Category>
                    {
                        new Category(),
                        new Category(),
                        new Category()
                    });
                }

                return Task.FromResult(categories);
            };

            var viewModel = new GroupDetailPageViewModel(repository, navigationService, null, null);
            viewModel.OnNavigatedTo(1, NavigationMode.New, null);

            Assert.IsNotNull(viewModel.Items);
            Assert.AreEqual(3, ((ICollection<CategoryViewModel>)viewModel.Items).Count);
            Assert.AreEqual("CategoryTitle", viewModel.Title);
        }

        [TestMethod]
        public void OnNavigatedTo_When_Service_Not_Available_Then_Pops_Alert()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();
            var alertService = new MockAlertMessageService();
            var resourceLoader = new MockResourceLoader();

            bool alertCalled = false;
            repository.GetSubcategoriesAsyncDelegate = (categoryId) =>
            {
                throw new HttpRequestException();
            };

            repository.GetCategoriesAsyncDelegate = (categoryId) =>
            {
                throw new HttpRequestException();
            };

            alertService.ShowAsyncDelegate = (msg, title) =>
            {
                alertCalled = true;
                return Task.FromResult(string.Empty);
            };

            var viewModel = new GroupDetailPageViewModel(repository, navigationService, alertService, resourceLoader);
            viewModel.OnNavigatedTo("1", NavigationMode.New, null);

            Assert.IsTrue(alertCalled);
        }


        [TestMethod]
        public void ProductNav_With_Valid_Parameter()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();

            navigationService.NavigateDelegate = (pageName, productNumber) =>
            {
                Assert.AreEqual("ItemDetailPage", pageName);
                Assert.AreEqual(1, productNumber);
                return true;
            };

            var viewModel = new GroupDetailPageViewModel(repository, navigationService, null, null);
            viewModel.ProductNavigationAction.Invoke(1);
        }

        [TestMethod]
        public void ProductNav_With_Null_Parameter()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();

            navigationService.NavigateDelegate = (pageName, categoryId) =>
            {
                Assert.Fail();
                return false;
            };

            var viewModel = new GroupDetailPageViewModel(repository, navigationService, null, null);
            viewModel.ProductNavigationAction.Invoke(null);
        }

        [TestMethod]
        public void GoBack_When_CanGoBack_Is_Not_True()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();

            navigationService.CanGoBackDelegate = () => false;
            navigationService.GoBackDelegate = () => Assert.Fail();

            var viewModel = new GroupDetailPageViewModel(repository, navigationService, null, null);
            bool canExecute = viewModel.GoBackCommand.CanExecute();

            if (canExecute) viewModel.GoBackCommand.Execute();
        }

        [TestMethod]
        public void GoBack_When_CanGoBack_Is_True()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();

            navigationService.CanGoBackDelegate = () => true;
            navigationService.GoBackDelegate = () => Assert.IsTrue(true);

            var viewModel = new GroupDetailPageViewModel(repository, navigationService, null, null);
            bool canExecute = viewModel.GoBackCommand.CanExecute();

            if (canExecute) viewModel.GoBackCommand.Execute();
        }
    }
}
