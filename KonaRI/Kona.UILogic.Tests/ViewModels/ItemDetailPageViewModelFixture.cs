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
                ObservableCollection<Product> products = null;

                if (subCategoryId == 1)
                {
                    products = new ObservableCollection<Product>
                    {
                        new Product(){ ProductNumber = "1"},
                        new Product(){ ProductNumber = "2"},
                        new Product(){ ProductNumber = "3"}
                    };
                }

                return Task.FromResult(products);
            };

            var viewModel = new ItemDetailPageViewModel(repository, navigationService, new MockShoppingCartRepository());
            viewModel.OnNavigatedTo("1", NavigationMode.New, null);

            Assert.IsNotNull(viewModel.Items);
            Assert.AreEqual(3, ((ICollection<Product>)viewModel.Items).Count);
            Assert.AreEqual(viewModel.Items.First(), viewModel.SelectedProduct);
        }

        [TestMethod]
        public void GoBack_When_CanGoBack_Is_Not_True()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();

            navigationService.CanGoBackDelegate = () => false;
            navigationService.GoBackDelegate = () => Assert.Fail();

            var viewModel = new ItemDetailPageViewModel(repository, navigationService, new MockShoppingCartRepository());
            bool canExecute = viewModel.GoBackCommand.CanExecute();
            
            if (canExecute) viewModel.GoBackCommand.Execute();
        }

        [TestMethod]
        public void GoBack_When_CanGoBack_Is_True()
        {
            var repository = new MockProductCatalogRepository();
            var navigationService = new MockNavigationService();

            navigationService.CanGoBackDelegate = () => true;
            navigationService.GoBackDelegate = () => Assert.IsTrue(true, "I can go back");

            var viewModel = new ItemDetailPageViewModel(repository, navigationService, new MockShoppingCartRepository());
            bool canExecute = viewModel.GoBackCommand.CanExecute();

            if (canExecute) viewModel.GoBackCommand.Execute();
        }
    }
}
