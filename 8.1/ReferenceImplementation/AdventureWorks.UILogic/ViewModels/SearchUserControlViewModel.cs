// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Repositories;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Windows.UI.Xaml.Navigation;

namespace AdventureWorks.UILogic.ViewModels
{
    public class SearchUserControlViewModel : ViewModel
    {
        private const uint MaxNumberOfSuggestions = 5;
        private readonly INavigationService _navigationService;
        private readonly IProductCatalogRepository _productCatalogRepository;
        private readonly IEventAggregator _eventAggregator;
        private readonly IResourceLoader _resourceLoader;

        public SearchUserControlViewModel(INavigationService navigationService, IProductCatalogRepository productCatalogRepository, IEventAggregator eventAggregator, IResourceLoader resourceLoader)
        {
            _navigationService = navigationService;
            _productCatalogRepository = productCatalogRepository;
            _eventAggregator = eventAggregator;
            _resourceLoader = resourceLoader;
            this.SearchCommand = new DelegateCommand<SearchBoxQuerySubmittedEventArgs>(SearchBoxQuerySubmitted);
            this.SearchSuggestionsCommand = new DelegateCommand<SearchBoxSuggestionsRequestedEventArgs>(async (eventArgs) =>
            {
                await SearchBoxSuggestionsRequested(eventArgs);
            });

        }

        public DelegateCommand<SearchBoxQuerySubmittedEventArgs> SearchCommand { get; set; }

        public DelegateCommand<SearchBoxSuggestionsRequestedEventArgs> SearchSuggestionsCommand { get; set; }

        private void SearchBoxQuerySubmitted(SearchBoxQuerySubmittedEventArgs eventArgs)
        {
            var searchTerm = eventArgs.QueryText != null ? eventArgs.QueryText.Trim() : null;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                _navigationService.Navigate("SearchResults", searchTerm);
            }
        }

        private async Task SearchBoxSuggestionsRequested(SearchBoxSuggestionsRequestedEventArgs args)
        {
            var queryText = args.QueryText != null ? args.QueryText.Trim() : null;
            if (string.IsNullOrEmpty(queryText)) return;
            var deferral = args.Request.GetDeferral();

            try
            {
                var suggestionCollection = args.Request.SearchSuggestionCollection;
                             
                var querySuggestions = await _productCatalogRepository.GetSearchSuggestionsAsync(queryText);
                if (querySuggestions != null && querySuggestions.Count > 0)
                {
                    var querySuggestionCount = 0;
                    foreach (string suggestion in querySuggestions)
                    {
                        querySuggestionCount++;

                        suggestionCollection.AppendQuerySuggestion(suggestion);

                        if (querySuggestionCount >= MaxNumberOfSuggestions)
                        {
                            break;
                        }
                    }
                }
                
            }
            catch (Exception)
            {
                //Ignore any exceptions that occur trying to find search suggestions.
            }
            
            deferral.Complete();
        }
    }
}