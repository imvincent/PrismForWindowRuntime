// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Kona.Infrastructure;
using Kona.UILogic.Repositories;
using Windows.UI.Xaml.Navigation;

namespace Kona.UILogic.ViewModels
{
    public class SearchResultsPageViewModel : ViewModel, INavigationAware
    {
        private IProductCatalogRepository _productCatalogRepository;
        private INavigationService _navigationService;
        private string _searchTerm;
        private string _queryString;
        private  ObservableCollection<CategoryViewModel> _results = new ObservableCollection<CategoryViewModel>();

        public SearchResultsPageViewModel(IProductCatalogRepository productCatalogRepository, INavigationService navigationService)
        {
            _productCatalogRepository = productCatalogRepository;
            _navigationService = navigationService;
            ProductNavigationAction = NavigateToItem;
            GoBackCommand = new DelegateCommand(_navigationService.GoBack, _navigationService.CanGoBack);
        }

        public string QueryText
        {
            get { return _queryString; }
            set { SetProperty(ref this._queryString, value); }
        }

        public string SearchTerm
        {
            get { return _searchTerm; }
            set { SetProperty(ref this._searchTerm, value); }
        }

        public ObservableCollection<CategoryViewModel> Results
        {
            get { return _results; }
            set { SetProperty(ref _results, value); }
        } 

        public Action<object> ProductNavigationAction { get; private set; }

        public ICommand GoBackCommand { get; private set; }

        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState)
        {
            var queryText = navigationParameter as String;

            var rootCategories = await _productCatalogRepository.GetFilteredProductsAsync(queryText);
            var rootCategoryViewModels = new ObservableCollection<CategoryViewModel>();
            foreach (var rootCategory in rootCategories)
            {
                rootCategoryViewModels.Add(new CategoryViewModel(rootCategory, _navigationService));
            }

            // Communicate results through the view model
            this.SearchTerm = queryText;
            this.QueryText = '\u201c' + queryText + '\u201d';
            this.Results = rootCategoryViewModels;
        }

        private void NavigateToItem(object parameter)
        {
            var product = parameter as ProductViewModel;
            if (product != null)
            {
                _navigationService.Navigate("ItemDetail", product.ProductNumber);
            }
        }
    }
}


