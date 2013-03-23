// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Globalization;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Input;
using Microsoft.Practices.StoreApps.Infrastructure;
using Microsoft.Practices.StoreApps.Infrastructure.Interfaces;
using Windows.UI.Xaml.Navigation;

namespace AdventureWorks.UILogic.ViewModels
{
    public class HubPageViewModel : ViewModel
    {
        private IProductCatalogRepository _productCatalogRepository;
        private INavigationService _navigationService;
        private readonly IAlertMessageService _alertMessageService;
        private readonly IResourceLoader _resourceLoader;
        private readonly ISearchPaneService _searchPaneService;
        private IReadOnlyCollection<CategoryViewModel> _rootCategories;
        private bool _loadingData;

        public HubPageViewModel(IProductCatalogRepository productCatalogRepository, INavigationService navigationService, IAlertMessageService alertMessageService, IResourceLoader resourceLoader, ISearchPaneService searchPaneService)
        {
            _productCatalogRepository = productCatalogRepository;
            _navigationService = navigationService;
            _alertMessageService = alertMessageService;
            _resourceLoader = resourceLoader;
            _searchPaneService = searchPaneService;
            ProductNavigationAction = NavigateToItem;
            GoBackCommand = new DelegateCommand(navigationService.GoBack, navigationService.CanGoBack);
        }

        public bool LoadingData
        {
            get { return _loadingData; }
            private set { SetProperty(ref _loadingData, value); }
        }

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

        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            string errorMessage = string.Empty;
            ReadOnlyCollection<Category> rootCategories = null;
            try
            {
                LoadingData = true;
                // <snippet511>
                // <snippet1100>
                rootCategories = await _productCatalogRepository.GetRootCategoriesAsync(5);
                // </snippet1100>
                // </snippet511>
            }
            catch (HttpRequestException ex)
            {
                errorMessage = string.Format(CultureInfo.CurrentCulture,
                                             _resourceLoader.GetString("GeneralServiceErrorMessage"),
                                             Environment.NewLine, ex.Message);
            }
            finally
            {
                LoadingData = false;    
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                await _alertMessageService.ShowAsync(errorMessage, _resourceLoader.GetString("ErrorServiceUnreachable"));
                return;
            }

            var rootCategoryViewModels = new List<CategoryViewModel>();
            foreach (var rootCategory in rootCategories)
            {
                rootCategoryViewModels.Add(new CategoryViewModel(rootCategory, _navigationService));
            }
            RootCategories = new ReadOnlyCollection<CategoryViewModel>(rootCategoryViewModels);
            // <snippet1005>
            _searchPaneService.ShowOnKeyboardInput(true);
            // </snippet1005>
        }

        // <snippet1006>
        // <snippet709>
        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatedFrom(viewModelState, suspending);
            if (!suspending)
            {
                _searchPaneService.ShowOnKeyboardInput(false);
            }
        }
        // </snippet709>
        // </snippet1006>
    }
}
