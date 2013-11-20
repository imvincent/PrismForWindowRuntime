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

// The Group Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234229

namespace AdventureWorks.Shopper.Views
{
    /// <summary>
    /// A page that displays an overview of a single group, including a preview of the items
    /// within the group.
    /// </summary>
    public sealed partial class GroupDetailPage : VisualStateAwarePage
    {
        private double _scrollViewerOffsetProportion;
        private bool _isPageLoading = true;

        public GroupDetailPage()
        {
            InitializeComponent();
            this.SizeChanged += Page_SizeChanged;
        }

        private void ScrollBarVisibilityChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var helper = (DependencyPropertyChangedHelper)sender;

            var scrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(itemsGridView);

            if (((Visibility)e.NewValue) == Visibility.Visible)
            {
                ScrollViewerUtilities.ScrollToProportion(scrollViewer, _scrollViewerOffsetProportion);
                helper.PropertyChanged -= ScrollBarVisibilityChanged;
            };

            if (_isPageLoading)
            {
                itemsGridView.LayoutUpdated += itemsGridView_LayoutUpdated;
                _isPageLoading = false;
            }
        }

        protected override void SaveState(System.Collections.Generic.Dictionary<string, object> pageState)
        {
            if (pageState == null) return;

            base.SaveState(pageState);

            pageState["scrollViewerOffsetProportion"] = ScrollViewerUtilities.GetScrollViewerOffsetProportion(itemsGridView);
        }

        protected override void LoadState(object navigationParameter, System.Collections.Generic.Dictionary<string, object> pageState)
        {
            if (pageState == null) return;

            base.LoadState(navigationParameter, pageState);

            if (pageState.ContainsKey("scrollViewerOffsetProportion"))
            {
                _scrollViewerOffsetProportion = double.Parse(pageState["scrollViewerOffsetProportion"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
            }
        }

        void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var scrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(itemsGridView);

            if (scrollViewer != null)
            {
                if (scrollViewer.ComputedHorizontalScrollBarVisibility == Visibility.Visible && scrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible)
                {
                    ScrollViewerUtilities.ScrollToProportion(scrollViewer, _scrollViewerOffsetProportion);
                }
                else
                {
                    DependencyPropertyChangedHelper horizontalHelper = new DependencyPropertyChangedHelper(scrollViewer, "ComputedHorizontalScrollBarVisibility");
                    horizontalHelper.PropertyChanged += ScrollBarVisibilityChanged;

                    DependencyPropertyChangedHelper verticalHelper = new DependencyPropertyChangedHelper(scrollViewer, "ComputedVerticalScrollBarVisibility");
                    verticalHelper.PropertyChanged += ScrollBarVisibilityChanged;
                }
            }
        }

        private void itemsGridView_LayoutUpdated(object sender, object e)
        {
            _scrollViewerOffsetProportion = ScrollViewerUtilities.GetScrollViewerOffsetProportion(itemsGridView);
        }
    }
}