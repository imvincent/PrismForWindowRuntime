// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using GridViewListViewPerf.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Expression.Blend.SampleData.SampleDataSource;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace GridViewListViewPerf
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class BlendBehaviorScenario : Page
    {
        // Holds the sample data - See SampleData folder
        StoreData storeData = null;

        DateTime startTime = DateTime.Now;
        DateTime lastLayoutUpdated;

        private NavigationHelper navigationHelper;
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public BlendBehaviorScenario()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            // Create a new instance of store data
            storeData = new StoreData();
            // Set the source of the GridView to be the sample data
            ItemGridView.ItemsSource = storeData.Collection;
            ItemsTextBlock.Text = storeData.Collection.Count().ToString();
        }

        private void ItemGridView_Loaded(object sender, RoutedEventArgs e)
        {
            // Find the ScrollViewer inside the GridView
            var scrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(ItemGridView);
            scrollViewer.LayoutUpdated += scrollViewer_LayoutUpdated;
        }

        /// <summary>
        /// Stores the time before beginning the new layout.
        /// </summary>
        void scrollViewer_LayoutUpdated(object sender, object e)
        {
            lastLayoutUpdated = DateTime.Now;
            
        }

        /// <summary>
        /// Handles the Show Duration button and displays the approximate time
        /// span for the operation to complete.
        /// </summary>
        private void ShowDuration_Click(object sender, RoutedEventArgs e)
        {
            var timespan = lastLayoutUpdated.Subtract(startTime);
            ItemsGridViewDuration.Text = timespan.TotalSeconds.ToString();
        }
    }
}
