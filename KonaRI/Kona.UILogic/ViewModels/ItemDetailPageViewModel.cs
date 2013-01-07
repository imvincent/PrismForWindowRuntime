// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Kona.Infrastructure;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Windows.UI.Xaml.Navigation;
using Kona.UILogic.Services;

namespace Kona.UILogic.ViewModels
{
    public class ItemDetailPageViewModel : ViewModel, INavigationAware
    {
        private readonly IProductCatalogRepository _productCatalogRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private IEnumerable<Product> _items;
        private Product _selectedProduct;
        private string _title;

        public ItemDetailPageViewModel(IProductCatalogRepository productCatalogRepository, INavigationService navigationService, IShoppingCartRepository shoppingCartRepository)
        {
            _productCatalogRepository = productCatalogRepository;
            _shoppingCartRepository = shoppingCartRepository;
            GoBackCommand = new DelegateCommand(() => navigationService.GoBack(), () => navigationService.CanGoBack());
            // <snippet402>
            ShoppingCartCommand = new DelegateCommand(() => navigationService.Navigate("ShoppingCart", null));
            // </snippet402>
            AddToCartCommand = new DelegateCommand(()=> AddToCart(), () => CanAddToCart());
        }

        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand ShoppingCartCommand { get; private set; }
        public DelegateCommand AddToCartCommand { get; private set; }

        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set { SetProperty(ref _selectedProduct, value); }
        }

        public IEnumerable<Product> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState)
        {
            var productNumber = navigationParameter as string;
            var product = await _productCatalogRepository.GetProductAsync(productNumber);
            Items = await _productCatalogRepository.GetProductsAsync(product.SubcategoryId);
            SelectedProduct = Items.First(p => p.ProductNumber == productNumber);
            Title = SelectedProduct.Title;
        }

        public void AddToCart()
        {
            _shoppingCartRepository.AddProductToShoppingCartAsync(SelectedProduct.ProductNumber);
        }

        public bool CanAddToCart()
        {
            //TODO: Check Inventory
            return true;
        }
    }
}
