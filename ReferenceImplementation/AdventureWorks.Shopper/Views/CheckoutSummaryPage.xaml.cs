// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Microsoft.Practices.Prism.StoreApps;
using Windows.UI.Xaml;

namespace AdventureWorks.Shopper.Views
{
    public sealed partial class CheckoutSummaryPage : VisualStateAwarePage
    {
        public CheckoutSummaryPage()
        {
            this.InitializeComponent();
            Popup.Opened += Popup_Opened;
        }

        private void Popup_Opened(object sender, object e)
        {
            int margin = 10;
            int appbarHeight = 90;
            Popup.HorizontalOffset = margin;
            Popup.VerticalOffset = Window.Current.CoreWindow.Bounds.Bottom - appbarHeight - PopupPanel.Height - margin;
        }
    }
}
