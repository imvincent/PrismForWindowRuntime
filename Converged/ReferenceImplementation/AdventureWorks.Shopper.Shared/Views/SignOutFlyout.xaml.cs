// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Windows.UI.Xaml.Controls;

namespace AdventureWorks.Shopper.Views
{
    public sealed partial class SignOutFlyout : SettingsFlyout, IView
    {
        public SignOutFlyout()
        {
            this.InitializeComponent();

            var viewModel = this.DataContext as IFlyoutViewModel;
            viewModel.CloseFlyout = () => this.Hide();
        }
    }
}
