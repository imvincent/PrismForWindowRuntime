// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using HelloWorld.Services;
using System.Collections.Generic;
using Microsoft.Practices.StoreApps.Infrastructure;
using Microsoft.Practices.StoreApps.Infrastructure.Interfaces;

namespace HelloWorld.ViewModels
{
    public class MainPageViewModel : ViewModel
    {
        IDataRepository _dataRepository;

        public MainPageViewModel(IDataRepository dataRepository, INavigationService navService)
        {
            _dataRepository = dataRepository;
            NavigateCommand = new DelegateCommand(() => navService.Navigate("UserInput", null));
        }

        public DelegateCommand NavigateCommand { get; set; }

        public List<string> DisplayItems
        {
            get { return _dataRepository.GetFeatures(); }
        }
    }
}
