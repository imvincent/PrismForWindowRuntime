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

namespace AdventureWorks.UILogic.ViewModels
{
    public class SearchUserControlViewModel : ViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IProductCatalogRepository _productCatalogRepository;
        private readonly IEventAggregator _eventAggregator;
        private bool _focusOnKeyBoardInput;

        public SearchUserControlViewModel(INavigationService navigationService, IProductCatalogRepository productCatalogRepository, IEventAggregator eventAggregator)
        {
            _navigationService = navigationService;
            _productCatalogRepository = productCatalogRepository;
            _eventAggregator = eventAggregator;
            this.MaxNumberOfSuggestions = 5;
            this.FocusOnKeyBoardInput = true;
            this.SearchCommand = new DelegateCommand<SearchBoxQuerySubmittedEventArgs>(SearchBoxQuerySubmitted);
            this.SearchSuggestionsCommand = new DelegateCommand<SearchBoxSuggestionsRequestedEventArgs>(async (eventArgs) =>
            {
                await SearchBoxSuggestionsRequested(eventArgs);
            });
            if (_eventAggregator != null)
            {
                eventAggregator.GetEvent<FocusOnKeyboardInputChangedEvent>().Subscribe(FocusOnKeybordInputToggle);
            }
        }

        public bool FocusOnKeyBoardInput
        {
            get
            {
                return this._focusOnKeyBoardInput;
            }

            private set { SetProperty(ref _focusOnKeyBoardInput, value); }
        }

        public IList<string> SuggestionsList { get; set; }

        public uint MaxNumberOfSuggestions { get; set; }

        public DelegateCommand<SearchBoxQuerySubmittedEventArgs> SearchCommand { get; set; }

        public DelegateCommand<SearchBoxSuggestionsRequestedEventArgs> SearchSuggestionsCommand { get; set; }

        private void FocusOnKeybordInputToggle(bool value)
        {
            this.FocusOnKeyBoardInput = value;
        }

        private void SearchBoxQuerySubmitted(SearchBoxQuerySubmittedEventArgs eventArgs)
        {
            var searchTerm = eventArgs.QueryText;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                _navigationService.Navigate("SearchResults", searchTerm);
            }
            else
            {
                _navigationService.Navigate("Hub", null);
            }
        }

        private async Task SearchBoxSuggestionsRequested(SearchBoxSuggestionsRequestedEventArgs args)
        {
            var queryText = args.QueryText;
            if (string.IsNullOrEmpty(queryText)) return;
            var deferral = args.Request.GetDeferral();
            this.SuggestionsList = await _productCatalogRepository.GetSearchSuggestionsAsync(queryText);
            if (this.SuggestionsList != null && !string.IsNullOrEmpty(queryText))
            {
                foreach (string suggestion in this.SuggestionsList)
                {
                    // Add suggestion to Search Pane
                    args.Request.SearchSuggestionCollection.AppendQuerySuggestion(suggestion);

                    // Break since the Search Pane can show at most 25 suggestions
                    if (args.Request.SearchSuggestionCollection.Size >= this.MaxNumberOfSuggestions)
                    {
                        break;
                    }
                }
            }
            deferral.Complete();
        }
    }
}