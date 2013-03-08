// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;

namespace EventAggregatorQuickstart
{
    // <snippet3101>
    // Thread-safe shopping cart
    public class ShoppingCart
    {
        private List<ShoppingCartItem> _items = new List<ShoppingCartItem>();

        public void AddItem(ShoppingCartItem item)
        {
            lock (_items)
            {
                _items.Add(item);
            }
        }

        public int Count
        {
            get
            {
                lock (_items)
                {
                    return _items.Count;
                }
            }
        }
    }
    // </snippet3101>
}
