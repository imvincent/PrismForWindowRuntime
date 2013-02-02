// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.Infrastructure;
using Kona.Infrastructure.Interfaces;
using Windows.UI.Xaml;

namespace Kona.AWShopper.Views
{
    public sealed partial class CheckoutSummaryPage : VisualStateAwarePage
    {
        public CheckoutSummaryPage()
        {
            this.InitializeComponent();
            Popup.Opened += Popup_Opened;
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            var viewModel = this.DataContext as IHandleWindowSizeChanged;
            viewModel.WindowCurrentSizeChanged();
        }

        private void Popup_Opened(object sender, object e)
        {
            int margin = 10;
            int appbarHeight = 90;
            Popup.HorizontalOffset = margin;
            Popup.VerticalOffset = Window.Current.CoreWindow.Bounds.Bottom - appbarHeight - PopupPanel.ActualHeight - margin;
        }

        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            Window.Current.SizeChanged -= Current_SizeChanged;
            base.OnNavigatedFrom(e);
        }
    }
}
