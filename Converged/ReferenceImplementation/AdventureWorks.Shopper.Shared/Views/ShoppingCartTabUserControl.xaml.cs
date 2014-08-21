// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Prism.Mvvm;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AdventureWorks.Shopper.Views
{
    public sealed partial class ShoppingCartTabUserControl : UserControl, IView
    {
        public ShoppingCartTabUserControl()
        {
            this.InitializeComponent();
        }
    }
}
