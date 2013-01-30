// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.Infrastructure;
using Kona.UILogic.ViewModels;
using Windows.UI.Xaml;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Kona.AWShopper.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class CheckoutSummaryPage : VisualStateAwarePage
    {
        public CheckoutSummaryPage()
        {
            this.InitializeComponent();
            pop.Opened += pop_Opened;
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            var viewModel = this.DataContext as IHandleWindowSizeChanged;
            viewModel.WindowCurrentSizeChanged();
        }

        void pop_Opened(object sender, object e)
        {
            const int margin = 10;
            const int appbarHeight = 280;
            pop.HorizontalOffset = margin;
            pop.VerticalOffset = Window.Current.CoreWindow.Bounds.Bottom - appbarHeight - margin - 100;
        }

        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            Window.Current.SizeChanged -= Current_SizeChanged;
            base.OnNavigatedFrom(e);
        }
    }
}
