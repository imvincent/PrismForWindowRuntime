// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Globalization;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Services;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Navigation;

namespace AdventureWorks.UILogic.ViewModels
{
    public class CategoryPageViewModel : ViewModel
    {
        private IProductCatalogRepository _productCatalogRepository;
        private INavigationService _navigationService;
        private readonly IAlertMessageService _alertMessageService;
        private readonly IResourceLoader _resourceLoader;
        private IReadOnlyCollection<CategoryViewModel> _subcategories;
        private string _title;

        public CategoryPageViewModel(IProductCatalogRepository productCatalogRepository, INavigationService navigationService, IAlertMessageService alertMessageService, IResourceLoader resourceLoader)
        {
            _productCatalogRepository = productCatalogRepository;
            _navigationService = navigationService;
            _alertMessageService = alertMessageService;
            _resourceLoader = resourceLoader;
        }

        public IReadOnlyCollection<CategoryViewModel> Subcategories
        {
            get { return _subcategories; }
            private set { SetProperty(ref _subcategories, value); }
        }

        [RestorableState]
        public string Title
        {
            get { return _title; }
            private set { SetProperty(ref _title, value); }
        }

        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            int parentCategoryId = int.Parse(navigationParameter.ToString());
            ReadOnlyCollection<Category> subCategories = null;
            string errorMessage = string.Empty;
            try
            {
                subCategories = await _productCatalogRepository.GetSubcategoriesAsync(parentCategoryId, 5);
            }
            catch (Exception ex)
            {
                errorMessage = string.Format(CultureInfo.CurrentCulture, _resourceLoader.GetString("GeneralServiceErrorMessage"), Environment.NewLine, ex.Message);

            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                await _alertMessageService.ShowAsync(errorMessage, _resourceLoader.GetString("ErrorServiceUnreachable"));
                return;
            }

            if (string.IsNullOrEmpty(Title))
            {
                Title = _productCatalogRepository.GetCategoryName(parentCategoryId);
            }

            var subCategoryViewModels = new List<CategoryViewModel>();
            foreach (var subCategory in subCategories)
            {
                subCategoryViewModels.Add(new CategoryViewModel(subCategory, _navigationService));
            }
            Subcategories = new ReadOnlyCollection<CategoryViewModel>(subCategoryViewModels);
        }

    }
}
