// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Microsoft.Practices.Prism.StoreApps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace AdventureWorks.Shopper.Views
{
    /// <summary>
    /// A page that displays details for a single item within a group while allowing gestures to
    /// flip through other items belonging to the same group.
    /// </summary>
    public sealed partial class ItemDetailPage : VisualStateAwarePage
    {
        public ItemDetailPage()
        {
            this.InitializeComponent();
#if WINDOWS_APP
            this.TopAppBar = new AppBar
            {
                Style = (Style)App.Current.Resources["AppBarStyle"],
                Content = new TopAppBarUserControl()
            };
            // x:Uid="TopAppBar"
#endif
        }

        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // It is important to call EnableFocusOnKeyboardInput here in the OnNavigatedTo method to
            // give the previous page's SearchUserControl time to tear down.
            this.searchUserControl.EnableFocusOnKeyboardInput();
        }

        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            var adventureWorksApp = App.Current as App;
            if (adventureWorksApp != null && !adventureWorksApp.IsSuspending)
            {
                // It is important to call DisableFocusOnKeyboardInput here in the OnNavigatedFrom method 
                // to ensure that this page's SearchUserControl.FocusOnKeyboardInput is set to false 
                // prior to the next page's SearchUserControl.FocusOnKeyboardInput value is set to true
                this.searchUserControl.DisableFocusOnKeyboardInput();
            }
        }
    }
}
