// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Kona.Infrastructure;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Windows.UI.Xaml.Navigation;

namespace Kona.UILogic.ViewModels
{
    public class HubPageViewModel :ViewModel, INavigationAware
    {
        private IProductCatalogRepository _productCatalogRepository;
        private INavigationService _navigationService;
        private ObservableCollection<Category> _rootCategories;

        public ObservableCollection<Category> RootCategories
        {
            get { return _rootCategories; }
            private set { SetProperty(ref _rootCategories, value); }
        }

        public Action<object> CategoryNavigationAction { get; private set; }

        public HubPageViewModel(IProductCatalogRepository productCatalogRepository, INavigationService navigationService)
        {
            _productCatalogRepository = productCatalogRepository; 
            _navigationService = navigationService;
            CategoryNavigationAction = NavigateToCategory;
        }

        private void NavigateToCategory(object parameter)
        {
            var category = parameter as Category;
            if (category != null)
            {
                _navigationService.Navigate("GroupDetail", category.CategoryId);                
            }
        }

        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState)
        {
            RootCategories = await _productCatalogRepository.GetCategoriesAsync();
        }
    }
}
