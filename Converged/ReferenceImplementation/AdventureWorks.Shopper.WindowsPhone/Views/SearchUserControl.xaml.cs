// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using AdventureWorks.Events;
using AdventureWorks.UILogic.ViewModels;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AdventureWorks.Shopper.Views
{
    /// <summary>
    /// The SearchUserControl encapsulates the functionality and configuration related to the SearchBox control.
    /// http://go.microsoft.com/fwlink/?LinkID=386787
    /// </summary>
    public sealed partial class SearchUserControl : UserControl, IView
    {
        public static readonly DependencyProperty IsCompactProperty =
            DependencyProperty.RegisterAttached("IsCompact", typeof(bool), typeof(SearchUserControl),
            new PropertyMetadata(false, (o, e) => ((SearchUserControl)o).ChangeSearchBoxVisibility(!(bool)e.NewValue)));

        public SearchUserControl()
        {
            this.InitializeComponent();
        }

        public bool IsCompact {get; set;}

        internal void EnableFocusOnKeyboardInput()
        {
        }

        internal void DisableFocusOnKeyboardInput()
        {
        }

        private void ChangeSearchBoxVisibility(bool showSearchBox)
        {
        }
    }
}
