// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Microsoft.Practices.StoreApps.Infrastructure;
using Microsoft.Practices.StoreApps.Infrastructure.Interfaces;

namespace AdventureWorks.UILogic.ViewModels
{
    public class SearchUserControlViewModel
    {
        private readonly ISearchPaneService _searchPaneService;

        public SearchUserControlViewModel(ISearchPaneService searchPaneService)
        {
            _searchPaneService = searchPaneService;
            SearchCommand = new DelegateCommand(ShowSearchPane);
        }

        public DelegateCommand SearchCommand { get; set; }

        private void ShowSearchPane()
        {
            _searchPaneService.Show();
        }
    }
}
