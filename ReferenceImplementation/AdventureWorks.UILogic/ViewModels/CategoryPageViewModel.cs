// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Services;
using Microsoft.Practices.StoreApps.Infrastructure;
using Microsoft.Practices.StoreApps.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Input;
using Windows.UI.Xaml.Navigation;

namespace AdventureWorks.UILogic.ViewModels
{
    public class CategoryPageViewModel : ViewModel
    {
        private IProductCatalogRepository _productCatalogRepository;
        private INavigationService _navigationService;
        private readonly IAlertMessageService _alertMessageService;
        private readonly IResourceLoader _resourceLoader;
        private readonly ISearchPaneService _searchPaneService;
        private IReadOnlyCollection<CategoryViewModel> _subcategories;

        public CategoryPageViewModel(IProductCatalogRepository productCatalogRepository, INavigationService navigationService, IAlertMessageService alertMessageService, IResourceLoader resourceLoader, ISearchPaneService searchPaneService)
        {
            _productCatalogRepository = productCatalogRepository;
            _navigationService = navigationService;
            _alertMessageService = alertMessageService;
            _resourceLoader = resourceLoader;
            _searchPaneService = searchPaneService;
            ProductNavigationAction = NavigateToItem;
            GoBackCommand = new DelegateCommand(navigationService.GoBack, navigationService.CanGoBack);
        }

        public IReadOnlyCollection<CategoryViewModel> Subcategories
        {
            get { return _subcategories; }
            private set { SetProperty(ref _subcategories, value); }
        }

        public ICommand GoBackCommand { get; private set; }

        public Action<object> ProductNavigationAction { get; private set; }

        private void NavigateToItem(object parameter)
        {
            var product = parameter as ProductViewModel;
            if (product != null)
            {
                _navigationService.Navigate("ItemDetail", product.ProductNumber);
            }
        }

        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            int parentCategoryId = int.Parse(navigationParameter.ToString());
            ReadOnlyCollection<Category> subCategories = null;
            var getCategoriesCallFailed = false;
            try
            {
                subCategories = await _productCatalogRepository.GetSubcategoriesAsync(parentCategoryId, 5);
            }
            catch (HttpRequestException)
            {
                getCategoriesCallFailed = true;
            }

            if (getCategoriesCallFailed)
            {
                await _alertMessageService.ShowAsync(_resourceLoader.GetString("ErrorServiceUnreachable"), _resourceLoader.GetString("Error"));
                return;
            }

            var subCategoryViewModels = new List<CategoryViewModel>();
            foreach (var subCategory in subCategories)
            {
                subCategoryViewModels.Add(new CategoryViewModel(subCategory, _navigationService));
            }
            Subcategories = new ReadOnlyCollection<CategoryViewModel>(subCategoryViewModels);
            _searchPaneService.ShowOnKeyboardInput(true);
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatedFrom(viewModelState, suspending);
            if (!suspending)
            {
                _searchPaneService.ShowOnKeyboardInput(false);
            }
        }
    }
}
