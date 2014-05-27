// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using HelloWorldWithContainer.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;

namespace HelloWorldWithContainer.ViewModels
{
    // This QuickStart is documented at http://go.microsoft.com/fwlink/?LinkID=288830&clcid=0x409

    public class UserInputPageViewModel : ViewModel
    {
        private readonly IDataRepository _dataRepository;
        private readonly INavigationService _navService;
        private string _VMState;

        public UserInputPageViewModel(IDataRepository dataRepository, INavigationService navService)
        {
            _navService = navService;
            _dataRepository = dataRepository;
            GoBackCommand = new DelegateCommand(_navService.GoBack);
        }

        public DelegateCommand GoBackCommand { get; set; }

        [RestorableState]
        public string VMState
        {
            get { return _VMState; }
            set { SetProperty(ref _VMState, value); }
        }

        public string ServiceState
        {
            get { return _dataRepository.GetUserEnteredData(); }
            set { _dataRepository.SetUserEnteredData(value); }
        }
    }
}
