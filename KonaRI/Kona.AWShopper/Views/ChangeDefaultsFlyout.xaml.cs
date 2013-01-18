// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.Infrastructure.Flyouts;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Kona.AWShopper.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChangeDefaultsFlyout : FlyoutView
    {
        private bool _isShippingAddressDisplayed = true;
        private bool _isBillingAddressDisplayed = true;
        private bool _isPaymentInfoDisplayed = true;

        public ChangeDefaultsFlyout(string commandId, string commandTitle)
            : base(commandId, commandTitle, StandardFlyoutSize.Narrow)
        {
            this.InitializeComponent();

            HideShippingAddress.Completed += (senderObject, arguments) => CollapseShippingAddress.Begin();
            UncollapseShippingAddress.Completed += (senderObject, arguments) => ShowShippingAddress.Begin();
            HideBillingAddress.Completed += (senderObject, arguments) => CollapseBillingAddress.Begin();
            UncollapseBillingAddress.Completed += (senderObject, arguments) => ShowBillingAddress.Begin();
            HidePaymentInfo.Completed += (senderObject, arguments) => CollapsePaymentInfo.Begin();
            UncollapsePaymentInfo.Completed += (senderObject, arguments) => ShowPaymentInfo.Begin();

            var viewModel = this.DataContext as IFlyoutViewModel;
            if (viewModel != null)
            {
                viewModel.CloseFlyout = this.Close;
                viewModel.GoBack = this.GoBack;
            }
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

        private void PaymentInfoToggleClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_isPaymentInfoDisplayed)
            {
                HidePaymentInfo.Begin();
                _isPaymentInfoDisplayed = false;
            }
            else
            {
                UncollapsePaymentInfo.Begin();
                _isPaymentInfoDisplayed = true;
            }
        }
    }
}
