// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using Windows.UI.Popups;

namespace EventAggregatorQuickstart
{
    public class NonUISubscriber
    {
        // <snippet3106>
        public void HandleShoppingCartChanged(ShoppingCart cart)
        {
            MessageDialog dialog = new MessageDialog("Shopping cart updated in Non UI subscriber on thread " + Environment.CurrentManagedThreadId);
            // Assign into local variable because it is meant to be fire and forget and calling would require an await/async
            var task = dialog.ShowAsync();
        }
        // </snippet3106>
    }
}
