// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Windows.UI.Xaml.Controls;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Prism.Mvvm;

namespace AdventureWorks.Shopper.Views
{
    public sealed partial class ChangeDefaultsFlyout : SettingsFlyout, IView
    {
        public ChangeDefaultsFlyout()
        {
            InitializeComponent();

            var viewModel = this.DataContext as IFlyoutViewModel;
            viewModel.CloseFlyout = () => this.Hide();
        }
    }
}
