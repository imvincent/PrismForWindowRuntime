// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using Kona.Infrastructure;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Kona.UILogic.Services;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;
using System.Windows.Input;

namespace Kona.UILogic.ViewModels
{
    public class HubPageViewModel : ViewModel, INavigationAware
    {
        private IProductCatalogRepository _productCatalogRepository;
        private INavigationService _navigationService;
        private readonly IAlertMessageService _alertMessageService;
        private readonly IResourceLoader _resourceLoader;
        private IReadOnlyCollection<CategoryViewModel> _rootCategories;

        // <snippet303>
        public HubPageViewModel(IProductCatalogRepository productCatalogRepository, INavigationService navigationService, IAlertMessageService alertMessageService, IResourceLoader resourceLoader)
        {
            _productCatalogRepository = productCatalogRepository; 
            _navigationService = navigationService;
            _alertMessageService = alertMessageService;
            _resourceLoader = resourceLoader;
            ProductNavigationAction = NavigateToItem;
            GoBackCommand = new DelegateCommand(() => navigationService.GoBack(), () => navigationService.CanGoBack());
        }
        // </snippet303>
        
        // <snippet305>
        public IReadOnlyCollection<CategoryViewModel> RootCategories
        {
            get { return _rootCategories; }
            private set { SetProperty(ref _rootCategories, value); }
        }
        // </snippet305>

        public ICommand GoBackCommand { get; private set; }

        public Action<object> ProductNavigationAction { get; private set; }

        // <snippet412>
        private void NavigateToItem(object parameter)
        {
            var product = parameter as ProductViewModel;
            if (product != null)
            {
                _navigationService.Navigate("ItemDetail", product.ProductNumber);
            }
        }
        // </snippet412>

        // <snippet511>
        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState)
        {
            ReadOnlyCollection<Category> rootCategories = null;
            var getCategoriesCallFailed = false;
            try
            {
                rootCategories = await _productCatalogRepository.GetCategoriesAsync(5);
            }
            catch (HttpRequestException)
            {
                getCategoriesCallFailed = true;
            }

            if (getCategoriesCallFailed)
            {
                await _alertMessageService.ShowAsync(_resourceLoader.GetString("ErrorProductCatalogServiceUnreachable"), _resourceLoader.GetString("Error"));
                return;
            }
            
            var rootCategoryViewModels = new List<CategoryViewModel>();
            foreach (var rootCategory in rootCategories)
            {
                rootCategoryViewModels.Add(new CategoryViewModel(rootCategory, _navigationService));
            }
            RootCategories = new ReadOnlyCollection<CategoryViewModel>(rootCategoryViewModels);
        }
        // </snippet511>
    }
}
