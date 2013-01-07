// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.Infrastructure.Flyouts;

namespace Kona.AWShopper.Views
{
    /// <summary>
    /// Settings Flyout to add/Edit Payment Infos
    /// </summary>
    public sealed partial class PaymentMethodFlyout : FlyoutView
    {
        public PaymentMethodFlyout(string commandId, string commandTitle)
            : base(commandId, commandTitle, StandardFlyoutSize.Narrow)
        {
            this.InitializeComponent();

            var viewModel = this.DataContext as IFlyoutViewModel;
            if (viewModel != null)
            {
                viewModel.CloseFlyout = this.Close;
                viewModel.GoBack = this.GoBack;
            }
        }
    }
}
