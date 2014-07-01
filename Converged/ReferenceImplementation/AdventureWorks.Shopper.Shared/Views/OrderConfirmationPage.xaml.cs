// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Microsoft.Practices.Prism.StoreApps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AdventureWorks.Shopper.Views
{
    public sealed partial class OrderConfirmationPage : VisualStateAwarePage
    {
        public OrderConfirmationPage()
        {
#if WINDOWS_APP
            this.TopAppBar = new AppBar
            {
                Style = (Style)App.Current.Resources["AppBarStyle"],
                Content = new TopAppBarUserControl()
            };
            // x:Uid="TopAppBar"
#endif
            this.InitializeComponent();
        }
    }
}
