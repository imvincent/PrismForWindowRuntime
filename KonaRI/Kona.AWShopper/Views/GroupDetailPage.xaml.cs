// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.Infrastructure;
using Windows.UI.Xaml.Controls;

// The Group Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234229

namespace Kona.AWShopper.Views
{
    /// <summary>
    /// A page that displays an overview of a single group, including a preview of the items
    /// within the group.
    /// </summary>
    public sealed partial class GroupDetailPage : VisualStateAwarePage
    {
        double itemGridViewScrollViewerHorizontalOffset;
        private ScrollViewer itemGridViewScrollViewer = null;

        public GroupDetailPage()
        {
            this.InitializeComponent();
        }

        void itemGridView_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            itemGridViewScrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(itemGridView);
            ScrollToSavedHorizontalOffset();

            itemGridViewScrollViewer.Loaded += itemGridViewScrollViewer_Loaded;

        }

        private void ScrollToSavedHorizontalOffset()
        {
            itemGridViewScrollViewer.ScrollToHorizontalOffset(itemGridViewScrollViewerHorizontalOffset);
        }

        void itemGridViewScrollViewer_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ScrollToSavedHorizontalOffset();
        }

        protected override void SaveState(System.Collections.Generic.Dictionary<string, object> pageState)
        {
            base.SaveState(pageState);

            if (itemGridViewScrollViewer != null && pageState != null)
            {
                pageState["itemGridViewScrollViewerHorizontalOffset"] = itemGridViewScrollViewer.HorizontalOffset;
            }
        }

        protected override void LoadState(object navigationParameter, System.Collections.Generic.Dictionary<string, object> pageState)
        {
            base.LoadState(navigationParameter, pageState);

            if (pageState != null && pageState.ContainsKey("itemGridViewScrollViewerHorizontalOffset"))
            {
                double.TryParse(pageState["itemGridViewScrollViewerHorizontalOffset"].ToString(),
                                out itemGridViewScrollViewerHorizontalOffset);
            }
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ScrollToSavedHorizontalOffset();
        }
    }
}
