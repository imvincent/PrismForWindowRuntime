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
using Kona.Infrastructure;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Windows.Foundation;
using Windows.UI.Xaml.Navigation;
using Kona.UILogic.Services;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kona.UILogic.ViewModels
{
    public class ItemDetailPageViewModel : ViewModel, INavigationAware
    {
        private readonly IProductCatalogRepository _productCatalogRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IAlertMessageService _alertService;
        private readonly IResourceLoader _resourceLoader;
        private readonly ITileService _tileService;
        private IReadOnlyCollection<ProductViewModel> _items;
        private ProductViewModel _selectedProduct;
        private bool _isSelectedProductPinned;
        private string _title;
        private bool _isAppBarSticky;

        public ItemDetailPageViewModel(IProductCatalogRepository productCatalogRepository, INavigationService navigationService, IShoppingCartRepository shoppingCartRepository, IAlertMessageService alertMessageService, IResourceLoader resourceLoader, ITileService tileService)
        {
            _productCatalogRepository = productCatalogRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _alertService = alertMessageService;
            _resourceLoader = resourceLoader;
            _tileService = tileService;
            _isAppBarSticky = false;

            GoBackCommand = new DelegateCommand(navigationService.GoBack, navigationService.CanGoBack);
            AddToCartCommand = DelegateCommand.FromAsyncHandler(AddToCart, CanAddToCart);
            PinProductCommand = DelegateCommand.FromAsyncHandler(PinProduct, () => SelectedProduct != null);
            UnpinProductCommand = DelegateCommand.FromAsyncHandler(UnpinProduct, () => SelectedProduct != null);
        }

        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand AddToCartCommand { get; private set; }
        public DelegateCommand PinProductCommand { get; private set; }
        public DelegateCommand UnpinProductCommand { get; private set; }

        public ProductViewModel SelectedProduct
        {
            get { return _selectedProduct; }
            set { SetProperty(ref _selectedProduct, value); }
        }

        public bool IsSelectedProductPinned
        {
            get { return _isSelectedProductPinned; }
            set { SetProperty(ref _isSelectedProductPinned, value); }
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

        public bool IsAppBarSticky
        {
            get { return _isAppBarSticky; }
            set { SetProperty(ref _isAppBarSticky, value); }
        }

        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState)
        {
            try
            {
                var productNumber = navigationParameter as string;
                var selectedProduct = await _productCatalogRepository.GetProductAsync(productNumber);
                var productViewModels = (await _productCatalogRepository.GetProductsAsync(selectedProduct.SubcategoryId))
                                                                        .Select(product => new ProductViewModel(product, _shoppingCartRepository));
                
                Items = new ReadOnlyCollection<ProductViewModel>(productViewModels.ToList());
                
                SelectedProduct = Items.First(p => p.ProductNumber == productNumber);
                Title = SelectedProduct.Title;
                IsSelectedProductPinned = _tileService.SecondaryTileExists(SelectedProduct.ProductNumber);
            }
            catch (HttpRequestException)
            {
                var task = _alertService.ShowAsync(_resourceLoader.GetString("ErrorServiceUnreachable"), _resourceLoader.GetString("Error"));
            }
        }

        public async Task AddToCart()
        {
            await _shoppingCartRepository.AddProductToShoppingCartAsync(SelectedProduct.ProductNumber);
        }

        public bool CanAddToCart()
        {
            //TODO: Check Inventory
            return true;
        }

        private async Task PinProduct()
        {
            var product = SelectedProduct;
            bool isPinned = _tileService.SecondaryTileExists(product.ProductNumber);

            if (product != null && !isPinned)
            {
                IsAppBarSticky = true;

                isPinned = await _tileService.PinWideSecondaryTile(product.ProductNumber, product.Title, product.Description, product.ProductNumber);
                IsSelectedProductPinned = isPinned;

                IsAppBarSticky = false;
            }
        }

        private async Task UnpinProduct()
        {
            var product = SelectedProduct;
            bool isPinned = _tileService.SecondaryTileExists(product.ProductNumber);

            if (product != null && isPinned)
            {
                IsAppBarSticky = true;

                isPinned = (await _tileService.UnpinTile(product.ProductNumber)) == false;
                IsSelectedProductPinned = isPinned;

                IsAppBarSticky = false;
            }
        }
    }
}
