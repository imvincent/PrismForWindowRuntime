// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;

namespace Kona.WebServices.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }

        public Product Product{ get; set;}

        public bool IsGift { get; set; }

        public int Quantity { get; set; }

        public double DiscountPercentage { get; set; }

        public string Currency { get; set; }
    }
}
