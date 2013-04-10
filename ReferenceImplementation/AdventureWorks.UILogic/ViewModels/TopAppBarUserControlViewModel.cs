// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;

namespace AdventureWorks.UILogic.ViewModels
{
    public class TopAppBarUserControlViewModel : BindableBase
    {
        public TopAppBarUserControlViewModel(INavigationService navigationService)
        {
            HomeNavigationCommand = new DelegateCommand(() => navigationService.Navigate("Hub", null));
            ShoppingCartNavigationCommand = new DelegateCommand(() => navigationService.Navigate("ShoppingCart", null));
        }

        public DelegateCommand HomeNavigationCommand { get; private set; }
        public DelegateCommand ShoppingCartNavigationCommand { get; private set; }
    }
}
