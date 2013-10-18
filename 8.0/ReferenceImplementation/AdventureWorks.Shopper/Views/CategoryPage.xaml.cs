// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.ComponentModel;
using System.Globalization;
using Microsoft.Practices.Prism.StoreApps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AdventureWorks.Shopper.Common;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace AdventureWorks.Shopper.Views
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class CategoryPage : VisualStateAwarePage
    {
        private double _virtualizingStackPanelHorizontalOffset;
        private double _scrollViewerHorizontalOffset;
       
        public CategoryPage()
        {
            this.InitializeComponent();
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
            var adventureWorksApp = Application.Current as App;
            if (adventureWorksApp != null && (!adventureWorksApp.IsSuspending && viewModel != null))
            {
                viewModel.PropertyChanged -= viewModel_PropertyChanged;
            }
        }

        void viewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Subcategories")
            {
                var listViewBase = semanticZoom.ZoomedOutView as ListViewBase;
                if (listViewBase != null)
                    listViewBase.ItemsSource = groupedItemsViewSource.View.CollectionGroups;
            }
        }

        private void virtualizingStackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            var virtualizingStackPanel = (VirtualizingStackPanel)sender;

            // Find the ScrollViewer inside the GridView
            var scrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(itemsGridView);

            if (scrollViewer != null)
            {
                if (scrollViewer.ComputedHorizontalScrollBarVisibility == Visibility.Visible)
                {
                    // Update the Horizontal offset
                    virtualizingStackPanel.SetHorizontalOffset(_virtualizingStackPanelHorizontalOffset);
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
            var virtualizingStackPanel = VisualTreeUtilities.GetVisualChild<VirtualizingStackPanel>(itemsGridView);
            
            if (((Visibility)e.NewValue) == Visibility.Visible)
            {
                // Update the Horizontal offset
                virtualizingStackPanel.SetHorizontalOffset(_virtualizingStackPanelHorizontalOffset);
                scrollViewer.ScrollToHorizontalOffset(_scrollViewerHorizontalOffset);
                helper.PropertyChanged -= ScrollBarHorizontalVisibilityChanged;
            };
        }

        protected override void SaveState(System.Collections.Generic.Dictionary<string, object> pageState)
        {
            if (pageState == null) return;

            base.SaveState(pageState);

            var virtualizingStackPanel = VisualTreeUtilities.GetVisualChild<VirtualizingStackPanel>(itemsGridView);
            if (virtualizingStackPanel != null)
            {
                pageState["virtualizingStackPanelHorizontalOffset"] = virtualizingStackPanel.HorizontalOffset;
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

            if (pageState.ContainsKey("virtualizingStackPanelHorizontalOffset"))
            {
                _virtualizingStackPanelHorizontalOffset = double.Parse(pageState["virtualizingStackPanelHorizontalOffset"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
            }

            if (pageState.ContainsKey("scrollViewerHorizontalOffset"))
            {
                _scrollViewerHorizontalOffset = double.Parse(pageState["scrollViewerHorizontalOffset"].ToString(), CultureInfo.InvariantCulture.NumberFormat);
            }
        }
    }
}
