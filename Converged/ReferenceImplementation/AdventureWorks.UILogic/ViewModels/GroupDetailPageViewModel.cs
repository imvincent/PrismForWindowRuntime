// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Services;

namespace AdventureWorks.UILogic.ViewModels
{
    public class GroupDetailPageViewModel : ViewModel
    {
        private readonly IProductCatalogRepository _productCatalogRepository;
        private readonly IAlertMessageService _alertMessageService;
        private readonly IResourceLoader _resourceLoader;
        private string _title;
        private IReadOnlyCollection<ProductViewModel> _items;

        public GroupDetailPageViewModel(IProductCatalogRepository productCatalogRepository, IAlertMessageService alertMessageService, IResourceLoader resourceLoader)
        {
            _productCatalogRepository = productCatalogRepository;
            _alertMessageService = alertMessageService;
            _resourceLoader = resourceLoader;
        }

        public string Title
        {
            get { return _title; }
            private set { SetProperty(ref _title, value); }
        }

        public IReadOnlyCollection<ProductViewModel> Items
        {
            get { return _items; }
            private set { SetProperty(ref _items, value); }
        }

        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var categoryId = Convert.ToInt32(navigationParameter);

            string errorMessage = string.Empty;
            try
            {
                var category = await _productCatalogRepository.GetCategoryAsync(categoryId);

                Title = category.Title;

                var products = await _productCatalogRepository.GetProductsAsync(categoryId);
                Items = new ReadOnlyCollection<ProductViewModel>(products
                                                                         .Select(product => new ProductViewModel(product))
                                                                         .ToList());
            }
            catch (Exception ex)
            {
            errorMessage = string.Format(CultureInfo.CurrentCulture, _resourceLoader.GetString("GeneralServiceErrorMessage"), Environment.NewLine, ex.Message);
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                await _alertMessageService.ShowAsync(errorMessage, _resourceLoader.GetString("ErrorServiceUnreachable"));
            }
        }
    }
}
