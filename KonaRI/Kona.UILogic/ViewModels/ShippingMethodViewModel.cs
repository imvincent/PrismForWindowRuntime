// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.Infrastructure;
using Kona.UILogic.Models;
using Windows.Globalization.NumberFormatting;
namespace Kona.UILogic.ViewModels
{
    public class ShippingMethodViewModel : BindableBase
    {
        private readonly ShippingMethod _shippingMethod;

        public ShippingMethodViewModel(ShippingMethod shippingMethod)
        {
            _shippingMethod = shippingMethod;
        }

        public ShippingMethod ShippingMethod { get { return _shippingMethod; } }

        public string Currency { get; set; }

        public string FormattedCost
        {
            get
            {
                var currencyFormatter = new CurrencyFormatter(Currency);
                return currencyFormatter.FormatDouble(_shippingMethod.Cost);
            }
        }
    }
}
