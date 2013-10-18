// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Microsoft.Practices.Prism.StoreApps;
using Windows.System;

namespace AdventureWorks.Shopper.Views
{
    public sealed partial class SignInFlyout : FlyoutView
    {
        public SignInFlyout()
            : base(StandardFlyoutSize.Narrow)
        {
            this.InitializeComponent();
            this.PasswordBox.KeyDown += PasswordBox_KeyDown;
        }

        void PasswordBox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                SubmitButton.Command.Execute(null);
            }
        }
    }
}
