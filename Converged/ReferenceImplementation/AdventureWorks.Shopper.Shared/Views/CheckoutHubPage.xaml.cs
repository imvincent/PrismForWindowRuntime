// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Prism.StoreApps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AdventureWorks.Shopper.Views
{
    public sealed partial class CheckoutHubPage : VisualStateAwarePage
    {
        public CheckoutHubPage()
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
