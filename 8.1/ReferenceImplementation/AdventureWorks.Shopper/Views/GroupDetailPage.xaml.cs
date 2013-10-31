// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Globalization;
using Microsoft.Practices.Prism.StoreApps;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using AdventureWorks.Shopper.Common;
using Windows.UI.Xaml.Controls.Primitives;

// The Group Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234229

namespace AdventureWorks.Shopper.Views
{
    /// <summary>
    /// A page that displays an overview of a single group, including a preview of the items
    /// within the group.
    /// </summary>
    public sealed partial class GroupDetailPage : VisualStateAwarePage
    {
        private double _scrollViewerHorizontalOffset;

        public GroupDetailPage()
        {
            InitializeComponent();
        }

        private void itemsGridView_Loaded(object sender, RoutedEventArgs e)
        {
            // Find the ScrollViewer inside the GridView
            var scrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(itemsGridView);

            if (scrollViewer != null)
            {
                if (scrollViewer.ComputedHorizontalScrollBarVisibility == Visibility.Visible)
                {
                    scrollViewer.ChangeView(_scrollViewerHorizontalOffset, null, null);
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

            if (((Visibility)e.NewValue) == Visibility.Visible)
            {
                // Update the Horizontal offset
                scrollViewer.ChangeView(_scrollViewerHorizontalOffset, null, null);
                helper.PropertyChanged -= ScrollBarHorizontalVisibilityChanged;
            };
        }

        protected override void SaveState(System.Collections.Generic.Dictionary<string, object> pageState)
        {
            if (pageState == null) return;

            base.SaveState(pageState);

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

            if (pageState.ContainsKey("scrollViewerHorizontalOffset"))
            {
                _scrollViewerHorizontalOffset = double.Parse(pageState["scrollViewerHorizontalOffset"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
            }
        }
    }
}