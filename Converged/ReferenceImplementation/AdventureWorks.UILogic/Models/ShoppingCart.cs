// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AdventureWorks.UILogic.Models
{
    [DataContract]
    public class ShoppingCart
    {
        public ShoppingCart(ICollection<ShoppingCartItem> shoppingCartItems)
        {
            ShoppingCartItems = shoppingCartItems;
        }

        [DataMember]
        public ICollection<ShoppingCartItem> ShoppingCartItems { get; private set; }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public double FullPrice { get; set; }

        [DataMember]
        public double TotalDiscount { get; set; }

        [DataMember]
        public double TotalPrice { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public double TaxRate { get; set; }
    }
}
