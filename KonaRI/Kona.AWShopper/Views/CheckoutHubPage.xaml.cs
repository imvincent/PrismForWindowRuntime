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
    public sealed partial class CheckoutHubPage : VisualStateAwarePage
    {
        public CheckoutHubPage()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            var viewModel = this.DataContext as IHandleWindowSizeChanged;
            viewModel.WindowCurrentSizeChanged();
        }

        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            Window.Current.SizeChanged -= Current_SizeChanged;
            base.OnNavigatedFrom(e);
        }
    }
}
