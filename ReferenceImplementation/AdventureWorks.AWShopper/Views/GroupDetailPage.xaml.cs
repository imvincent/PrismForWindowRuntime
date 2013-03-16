// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Globalization;
using Microsoft.Practices.StoreApps.Infrastructure;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using AdventureWorks.AWShopper.Common;

// The Group Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234229

namespace AdventureWorks.AWShopper.Views
{
    /// <summary>
    /// A page that displays an overview of a single group, including a preview of the items
    /// within the group.
    /// </summary>
    public sealed partial class GroupDetailPage : VisualStateAwarePage
    {
        private double _wrapPanelHorizontalOffset;
        private double _scrollViewerHorizontalOffset;

        public GroupDetailPage()
        {
            InitializeComponent();
        }

        private void wrapGrid_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var wrapGrid = (WrapGrid)sender;

            // Find the ScrollViewer inside the GridView
            var scrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(itemsGridView);

            if (scrollViewer != null)
            {
                if (scrollViewer.ComputedHorizontalScrollBarVisibility == Visibility.Visible)
                {
                    wrapGrid.SetHorizontalOffset(_wrapPanelHorizontalOffset);
                    scrollViewer.ScrollToHorizontalOffset(_scrollViewerHorizontalOffset);
                }
                else
                {
                    DependencyPropertyChangedHelper helper = new DependencyPropertyChangedHelper(scrollViewer, "ComputedHorizontalScrollBarVisibility");
                    helper.PropertyChanged += ScrollBarHorizontalVisibilityChanged;
                }
            }
        }

        private void ScrollBarHorizontalVisibilityChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var helper = (DependencyPropertyChangedHelper)sender;

            var scrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(itemsGridView);
            var wrapGrid = VisualTreeUtilities.GetVisualChild<WrapGrid>(itemsGridView);

            if (((Visibility)e.NewValue) == Visibility.Visible)
            {
                // Update the Horizontal offset
                wrapGrid.SetHorizontalOffset(_wrapPanelHorizontalOffset);
                scrollViewer.ScrollToHorizontalOffset(_scrollViewerHorizontalOffset);
                helper.PropertyChanged -= ScrollBarHorizontalVisibilityChanged;
            };
        }

        protected override void SaveState(System.Collections.Generic.Dictionary<string, object> pageState)
        {
            if (pageState == null) return;

            base.SaveState(pageState);

            var wrapGrid = VisualTreeUtilities.GetVisualChild<WrapGrid>(itemsGridView);
            if (wrapGrid != null)
            {
                pageState["wrapGridHorizontalOffset"] = wrapGrid.HorizontalOffset;
            }

            var scrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(itemsGridView);
            if (scrollViewer != null)
            {
                pageState["scrollViewerHorizontalOffset"] = scrollViewer.HorizontalOffset;
            }
        }

        protected override void LoadState(object navigationParameter, System.Collections.Generic.Dictionary<string, object> pageState)
        {
            if (pageState == null) return;

            base.LoadState(navigationParameter, pageState);

            if (pageState.ContainsKey("wrapGridHorizontalOffset"))
            {
                _wrapPanelHorizontalOffset = double.Parse(pageState["wrapGridHorizontalOffset"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
            }

            if (pageState.ContainsKey("scrollViewerHorizontalOffset"))
            {
                _scrollViewerHorizontalOffset = double.Parse(pageState["scrollViewerHorizontalOffset"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
            }
        }
    }
}