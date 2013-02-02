// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.Infrastructure.Flyouts;
using Windows.UI.Xaml.Controls;

namespace Kona.AWShopper.Views
{
    public sealed partial class BillingAddressFlyout : FlyoutView
    {
        public BillingAddressFlyout(string commandId, string commandTitle)
            : base(commandId, commandTitle, StandardFlyoutSize.Narrow)
        {
            this.InitializeComponent();
        }
    }
}
