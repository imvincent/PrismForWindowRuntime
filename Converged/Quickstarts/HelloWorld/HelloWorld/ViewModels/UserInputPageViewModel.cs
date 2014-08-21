// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using HelloWorld.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;

namespace HelloWorld.ViewModels
{
    // This QuickStart is documented at http://go.microsoft.com/fwlink/?LinkID=288829&clcid=0x409

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
