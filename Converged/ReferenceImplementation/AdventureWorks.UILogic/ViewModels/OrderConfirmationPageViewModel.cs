// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Globalization;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Microsoft.Practices.Prism.Mvvm.Interfaces;

namespace AdventureWorks.UILogic.ViewModels
{
    public class OrderConfirmationPageViewModel : ViewModel
    {
        private readonly IResourceLoader _resourceLoader;
        private readonly INavigationService _navigationService;

        public OrderConfirmationPageViewModel(IResourceLoader resourceLoader, INavigationService navigationService)
        {
            _resourceLoader = resourceLoader;
            _navigationService = navigationService;
        }

        public string OrderConfirmationContent { get; set; }

        public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, System.Collections.Generic.Dictionary<string, object> viewModelState)
        {
            OrderConfirmationContent = string.Format(CultureInfo.InvariantCulture, _resourceLoader.GetString("OrderConfirmationContent"), navigationParameter);
            _navigationService.ClearHistory();
        }
    }
}