// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using IncrementalLoadingQuickstart.Common;
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

namespace IncrementalLoadingQuickstart
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class BlendBehaviorScenario : Page
    {
        // Holds the sample data - See SampleData folder
        StoreData storeData = null;

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
        }
    }
}
