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
    public sealed partial class ShoppingCartPage : VisualStateAwarePage
    {
        public ShoppingCartPage()
        {
            this.InitializeComponent();
            pop.Opened += pop_Opened;
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            var viewModel = this.DataContext as IHandleWindowSizeChanged;
            if (viewModel != null)
            {
                viewModel.WindowCurrentSizeChanged();
            }
        }

        void pop_Opened(object sender, object e)
        {
            const int margin = 10;
            const int appbarHeight = 280;
            pop.HorizontalOffset = margin;
            pop.VerticalOffset = Window.Current.CoreWindow.Bounds.Bottom - appbarHeight - margin;
        }

        protected override void OnNavigatingFrom(Windows.UI.Xaml.Navigation.NavigatingCancelEventArgs e)
        {
            Window.Current.SizeChanged -= Current_SizeChanged;
        }
    }
}
