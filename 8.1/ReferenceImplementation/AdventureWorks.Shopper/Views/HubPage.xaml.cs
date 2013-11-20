// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.ComponentModel;
using System.Globalization;
using Microsoft.Practices.Prism.StoreApps;
using Windows.UI.Xaml.Controls;
using AdventureWorks.Shopper.Common;
using Windows.UI.Xaml;
using System;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace AdventureWorks.Shopper.Views
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class HubPage : VisualStateAwarePage
    {
        private double _scrollViewerOffsetProportion;
        private bool _isPageLoading = true;

        public HubPage()
        {
            this.InitializeComponent();
            this.Loaded += HubPage_Loaded;
            this.SizeChanged += Page_SizeChanged;
        }

        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var viewModel = this.DataContext as INotifyPropertyChanged;
            if (viewModel != null)
            {
                viewModel.PropertyChanged += viewModel_PropertyChanged;
            }
        }

        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            var viewModel = this.DataContext as INotifyPropertyChanged;
            var adventureWorksApp = App.Current as App;
            if (!adventureWorksApp.IsSuspending && viewModel != null)
            {
                viewModel.PropertyChanged -= viewModel_PropertyChanged;
            }
        }

        void viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RootCategories")
            {
                (semanticZoom.ZoomedOutView as ListViewBase).ItemsSource = groupedItemsViewSource.View.CollectionGroups;
            }
        }

        private void HubPage_Loaded(object sender, RoutedEventArgs e)
        {
            // We are specifically setting the focus on this control, to avoid having the SearchBox control focused,
            // and displaying the suggestion list when the app is first launched.
            this.shoppingCartTabUserControl.Focus(FocusState.Programmatic);
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
