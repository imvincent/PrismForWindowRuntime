// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


namespace Kona.WebServices.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public string UserId { get; set; }

        public ShoppingCart Cart { get; set; }

        public Address ShippingAddress { get; set; }

        public Address BillingAddress { get; set; }

        public PaymentInfo Payment { get; set; }

        public ShippingMethod ShipMethod { get; set; }
    }
}
