// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using HelloWorldWithContainer.Services;
using System.Collections.Generic;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Prism.Commands;

namespace HelloWorldWithContainer.ViewModels
{
    // This QuickStart is documented at http://go.microsoft.com/fwlink/?LinkID=288830&clcid=0x409

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
