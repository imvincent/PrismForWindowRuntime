// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.Infrastructure.Flyouts;

namespace Kona.AWShopper.Views
{
    public sealed partial class ChangeDefaultsFlyout : FlyoutView
    {
        private bool _isShippingAddressDisplayed = true;
        private bool _isBillingAddressDisplayed = true;
        private bool _isPaymentMethodDisplayed = true;

        public ChangeDefaultsFlyout(string commandId, string commandTitle)
            : base(commandId, commandTitle, StandardFlyoutSize.Narrow)
        {
            this.InitializeComponent();

            HideShippingAddress.Completed += (senderObject, arguments) => CollapseShippingAddress.Begin();
            UncollapseShippingAddress.Completed += (senderObject, arguments) => ShowShippingAddress.Begin();
            HideBillingAddress.Completed += (senderObject, arguments) => CollapseBillingAddress.Begin();
            UncollapseBillingAddress.Completed += (senderObject, arguments) => ShowBillingAddress.Begin();
            HidePaymentMethod.Completed += (senderObject, arguments) => CollapsePaymentMethod.Begin();
            UncollapsePaymentMethod.Completed += (senderObject, arguments) => ShowPaymentMethod.Begin();
        }

        private void ShippingAddressToggleClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_isShippingAddressDisplayed)
            {
                HideShippingAddress.Begin();
                _isShippingAddressDisplayed = false;
            }
            else
            {
                UncollapseShippingAddress.Begin();
                _isShippingAddressDisplayed = true;
            }
        }

        private void BillingAddressToggleClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_isBillingAddressDisplayed)
            {
                HideBillingAddress.Begin();
                _isBillingAddressDisplayed = false;
            }
            else
            {
                UncollapseBillingAddress.Begin();
                _isBillingAddressDisplayed = true;
            }
        }

        private void PaymentMethodToggleClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_isPaymentMethodDisplayed)
            {
                HidePaymentMethod.Begin();
                _isPaymentMethodDisplayed = false;
            }
            else
            {
                UncollapsePaymentMethod.Begin();
                _isPaymentMethodDisplayed = true;
            }
        }
    }
}
