// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.ComponentModel;
using Kona.Infrastructure;
using Kona.UILogic.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace Kona.AWShopper.Views
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class HubPage : VisualStateAwarePage
    {
        private double virtualizingStackPanelHorizontalOffset;

        public HubPage()
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

        private void virtualizingStackPanel_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var virtualizingStackPanel = (VirtualizingStackPanel)sender;
            virtualizingStackPanel.SetHorizontalOffset(virtualizingStackPanelHorizontalOffset);
        }


        protected override void SaveState(System.Collections.Generic.Dictionary<string, object> pageState)
        {
            base.SaveState(pageState);
            var virtualizingStackPanel = VisualTreeUtilities.GetVisualChild<VirtualizingStackPanel>(itemGridView);
            if (virtualizingStackPanel != null && pageState != null)
            {
                pageState["virtualizingStackPanelHorizontalOffset"] = virtualizingStackPanel.HorizontalOffset;
            }
        }

        protected override void LoadState(object navigationParameter, System.Collections.Generic.Dictionary<string, object> pageState)
        {
            base.LoadState(navigationParameter, pageState);
            if (pageState != null && pageState.ContainsKey("virtualizingStackPanelHorizontalOffset"))
            {
                double.TryParse(pageState["virtualizingStackPanelHorizontalOffset"].ToString(),
                                out virtualizingStackPanelHorizontalOffset);
            }

        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        //protected override async void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        //{

        //}

        /// <summary>
        /// Invoked when a group header is clicked.
        /// </summary>
        /// <param name="sender">The Button used as a group header for the selected group.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        //void Header_Click(object sender, RoutedEventArgs e)
        //{
        //    // Determine what group the Button instance represents
        //    var group = (sender as FrameworkElement).DataContext;

        //    // Navigate to the appropriate destination page, configuring the new page
        //    // by passing required information as a navigation parameter
        //    //this.Frame.Navigate(typeof(GroupedItemsPage), ((SampleDataGroup)group).UniqueId);
        //    throw new NotImplementedException("TODO!!!");
        //}
    }
}
