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
        private IReadOnlyCollection<ProductViewModel> _items;
        private ProductViewModel _selectedProduct;
        private string _title;

        public ItemDetailPageViewModel(IProductCatalogRepository productCatalogRepository, INavigationService navigationService, IShoppingCartRepository shoppingCartRepository)
        {
            _productCatalogRepository = productCatalogRepository;
            _shoppingCartRepository = shoppingCartRepository;
            GoBackCommand = new DelegateCommand(() => navigationService.GoBack(), () => navigationService.CanGoBack());
            AddToCartCommand = new DelegateCommand(()=> AddToCart(), () => CanAddToCart());
        }

        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand AddToCartCommand { get; private set; }

        public ProductViewModel SelectedProduct
        {
            get { return _selectedProduct; }
            set { SetProperty(ref _selectedProduct, value); }
        }

        public IReadOnlyCollection<ProductViewModel> Items
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
            var matchingproduct = await _productCatalogRepository.GetProductAsync(productNumber);
            var products = await _productCatalogRepository.GetProductsAsync(matchingproduct.SubcategoryId);
            var productViewModels = new List<ProductViewModel>();
            foreach (var product in products)
            {
                productViewModels.Add(new ProductViewModel(product, _shoppingCartRepository));   
            }
            Items = new ReadOnlyCollection<ProductViewModel>(productViewModels);
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
