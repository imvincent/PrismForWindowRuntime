// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

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
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace IncrementalLoadingQuickstart
{
    public sealed partial class ItemViewer : UserControl
    {
        public ItemViewer()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// This method visualizes the placeholder state of the data item. When 
        /// showing a placeholder, we set the opacity of other elements to zero
        /// so that stale data is not visible to the end user.  Note that we use
        /// Grid's background color as the placeholder background.
        /// </summary>
        /// <param name="item"></param>
        public void ShowPlaceholder(Item item)
        {
            _item = item;
            titleTextBlock.Opacity = 0;
            categoryTextBlock.Opacity = 0;
            image.Opacity = 0;
            linkButton.Opacity = 0;
        }

        /// <summary>
        /// Visualize the Title by updating the TextBlock for Title and setting Opacity
        /// to 1.
        /// </summary>
        public void ShowTitle()
        {
            titleTextBlock.Text = _item.Title;
            titleTextBlock.Opacity = 1;
        }

        public void ShowLinkbutton()
        {
            linkButton.Content = _item.Link;
            linkButton.NavigateUri = new System.Uri(_item.Link);
            linkButton.Opacity = 1;
        }
        /// <summary>
        /// Visualize category information by updating the correct TextBlock and 
        /// setting Opacity to 1.
        /// </summary>
        public void ShowCategory()
        {
            categoryTextBlock.Text = _item.Category;
            categoryTextBlock.Opacity = 1;
        }

        /// <summary>
        /// Visualize the Image associated with the data item by updating the Image 
        /// object and setting Opacity to 1.
        /// </summary>
        public void ShowImagePlaceHolder()
        {
            imagePlaceHolder.Opacity = 1;
        }


        /// <summary>
        /// Visualize the Image associated with the data item by updating the Image 
        /// object and setting Opacity to 1.
        /// </summary>
        public void ShowImage()
        {
            image.Source = new BitmapImage(_item.ImageUri);
            image.Opacity = 1;
        }

        /// <summary>
        /// Drop all refrences to the data item.
        /// </summary>
        public void ClearData()
        {
            _item = null;
            titleTextBlock.ClearValue(TextBlock.TextProperty);
            categoryTextBlock.ClearValue(TextBlock.TextProperty);
            image.ClearValue(Image.SourceProperty);
        }

        private Item _item;
    }
}