// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.Infrastructure;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;

namespace Kona.UILogic.ViewModels
{
    public class GroupDetailPageViewModel : ViewModel, INavigationAware
    {
        private readonly IProductCatalogRepository _productCatalogRepository;
        private readonly INavigationService _navigationService;
        private IEnumerable<Product> _items;
        private string _title;

        public GroupDetailPageViewModel(IProductCatalogRepository productCatalogRepository, INavigationService navigationService)
        {
            _productCatalogRepository = productCatalogRepository;
            _navigationService = navigationService;
            ProductNavigationAction = NavigateToProduct;
            GoBackCommand = new DelegateCommand(() => navigationService.GoBack(), () => navigationService.CanGoBack());
        }

        public IEnumerable<Product> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        public Action<object> ProductNavigationAction { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }

        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState)
        {
            var categoryId = navigationParameter is int ? (int)navigationParameter : 0;

            Items = await _productCatalogRepository.GetProductsAsync(categoryId);
            var category = await _productCatalogRepository.GetCategoryAsync(categoryId);
            Title = category.Title;
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private void NavigateToProduct(object parameter)
        {
            var product = parameter as Product;
            if (product != null)
            {
                _navigationService.Navigate("ItemDetail", product.ProductNumber);
            }
        }
    }
}
