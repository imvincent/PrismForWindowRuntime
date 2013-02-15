// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.Services;
using Kona.UILogic.Tests.Mocks;
using Kona.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI.Xaml.Navigation;
using System.Collections.Generic;
using System.Net.Http;

namespace Kona.UILogic.Tests.ViewModels
{
    [TestClass]
    public class ItemDetailPageViewModelFixture
    {
        [TestMethod]
        public void OnNavigatedTo_Fill_Items_And_SelectedProduct()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();

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
                        new Product(){ ProductNumber = "1", ImageName = "http://image" },
                        new Product(){ ProductNumber = "2", ImageName = "http://image" },
                        new Product(){ ProductNumber = "3", ImageName = "http://image" }
                    });
                }

                return Task.FromResult(products);
            };

            var viewModel = new ItemDetailPageViewModel(repository, navigationService, new MockShoppingCartRepository(), null, null, null);
            viewModel.OnNavigatedTo("1", NavigationMode.New, null);

            Assert.IsNotNull(viewModel.Items);
            Assert.AreEqual(3, ((IReadOnlyCollection<ProductViewModel>)viewModel.Items).Count);
            Assert.AreEqual(viewModel.Items.First(), viewModel.SelectedProduct);
        }

        [TestMethod]
        public void OnNavigatedTo_When_Service_Not_Available_Then_Pops_Alert()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();
            var alertService = new MockAlertMessageService();
            var resourceLoader = new MockResourceLoader();

            bool alertCalled = false;
            repository.GetProductAsyncDelegate = (productNumber) =>
            {
                throw new HttpRequestException();
            };

            repository.GetProductsAsyncDelegate = (subCategoryId) =>
            {
                throw new HttpRequestException();
            };

            alertService.ShowAsyncDelegate = (msg, title) =>
            {
                alertCalled = true;
                return Task.FromResult(string.Empty);
            };

            var viewModel = new ItemDetailPageViewModel(repository, navigationService, new MockShoppingCartRepository(), alertService, resourceLoader, null);
            viewModel.OnNavigatedTo("1", NavigationMode.New, null);

            Assert.IsTrue(alertCalled);
        }

        [TestMethod]
        public void GoBack_When_CanGoBack_Is_Not_True()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();
            var alertService = new MockAlertMessageService();

            navigationService.CanGoBackDelegate = () => false;
            navigationService.GoBackDelegate = () => Assert.Fail();

            var viewModel = new ItemDetailPageViewModel(repository, navigationService, new MockShoppingCartRepository(), null, null, null);
            bool canExecute = viewModel.GoBackCommand.CanExecute();
            
            if (canExecute) viewModel.GoBackCommand.Execute();
        }

        [TestMethod]
        public void GoBack_When_CanGoBack_Is_True()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();
            var alertService = new MockAlertMessageService();

            navigationService.CanGoBackDelegate = () => true;
            navigationService.GoBackDelegate = () => Assert.IsTrue(true, "I can go back");

            var viewModel = new ItemDetailPageViewModel(repository, navigationService, new MockShoppingCartRepository(), null, null, null);
            bool canExecute = viewModel.GoBackCommand.CanExecute();

            if (canExecute) viewModel.GoBackCommand.Execute();
        }

         [TestMethod]
         public void PinToDesktop_Changes_IsAppBarSticky()
         {
             // TODO
         }

         [TestMethod]
         public void PinToDesktop_CallsPin_OnlyIfNotPinned()
         {
             // TODO
         }

         [TestMethod]
         public void UnpToDesktop_Changes_IsAppBarSticky()
         {
             // TODO
         }

         [TestMethod]
         public void UnpinToDesktop_CallsUnpin_OnlyIfPinned()
         {
             // TODO
         }
    }
}
