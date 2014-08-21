// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Services;

namespace AdventureWorks.UILogic.ViewModels
{
    public class HubPageViewModel : ViewModel
    {
        private IProductCatalogRepository _productCatalogRepository;
        private INavigationService _navigationService;
        private readonly IAlertMessageService _alertMessageService;
        private readonly IResourceLoader _resourceLoader;
        private IReadOnlyCollection<CategoryViewModel> _rootCategories;
        private bool _loadingData;

        public HubPageViewModel(IProductCatalogRepository productCatalogRepository, INavigationService navigationService, IAlertMessageService alertMessageService, IResourceLoader resourceLoader)
        {
            _productCatalogRepository = productCatalogRepository;
            _navigationService = navigationService;
            _alertMessageService = alertMessageService;
            _resourceLoader = resourceLoader;
        }

        public bool LoadingData
        {
            get { return _loadingData; }
            private set { SetProperty(ref _loadingData, value); }
        }

        public IReadOnlyCollection<CategoryViewModel> RootCategories
        {
            get { return _rootCategories; }
            private set { SetProperty(ref _rootCategories, value); }
        }
        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            string errorMessage = string.Empty;
            ICollection<Category> rootCategories = null;
            try
            {
                LoadingData = true;
                rootCategories = await _productCatalogRepository.GetRootCategoriesAsync(5);
            }
            catch (Exception ex)
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
        }  
    }
}
