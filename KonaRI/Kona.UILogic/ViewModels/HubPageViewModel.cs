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
using System.Windows.Input;

namespace Kona.UILogic.ViewModels
{
    public class HubPageViewModel :ViewModel, INavigationAware
    {
        private IProductCatalogRepository _productCatalogRepository;
        private INavigationService _navigationService;
        private IReadOnlyCollection<CategoryViewModel> _rootCategories;

        // <snippet303>
        public HubPageViewModel(IProductCatalogRepository productCatalogRepository, INavigationService navigationService)
        {
            _productCatalogRepository = productCatalogRepository; 
            _navigationService = navigationService;
            CategoryNavigationAction = NavigateToCategory;
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

        public Action<object> CategoryNavigationAction { get; private set; }

        private void NavigateToCategory(object parameter)
        {
            var category = parameter as CategoryViewModel;
            if (category != null)
            {
                _navigationService.Navigate("GroupDetail", category.CategoryId);                
            }
        }

        // <snippet511>
        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState)
        {
            var rootCategories = await _productCatalogRepository.GetCategoriesAsync();
            var rootCategoryViewModels = new List<CategoryViewModel>();
            foreach (var rootCategory in rootCategories)
            {
                rootCategoryViewModels.Add(new CategoryViewModel(rootCategory));
            }
            RootCategories = new ReadOnlyCollection<CategoryViewModel>(rootCategoryViewModels);
        }
        // </snippet511>
    }
}
