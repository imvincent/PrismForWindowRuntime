// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


namespace EventAggregatorQuickstart
{
    public class ShoppingCartItem
    {
        public string Name { get; private set; }
        public decimal Cost { get; private set; }

        public ShoppingCartItem(string name, decimal cost)
        {
            Name = name;
            Cost = cost;
        }
    }
}
