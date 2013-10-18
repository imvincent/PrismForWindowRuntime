// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Globalization;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace EventAggregatorQuickstart
{
    public class BackgroundSubscriber
    {
        CoreDispatcher _dispatcher;
        public BackgroundSubscriber(CoreDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void HandleShoppingCartChanged(ShoppingCart cart)
        {
            var threadId = Environment.CurrentManagedThreadId;
            var count = cart.Count;

            // Assign into local variable because it is meant to be fire and forget and calling would require an await/async
            var dialogAction = _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    MessageDialog dialog = new MessageDialog(string.Format(CultureInfo.InvariantCulture, 
                        "Shopping cart updated to {0} item(s) in background subscriber on thread {1}", count, threadId));
                    var showAsync = dialog.ShowAsync();
                });
        }
    }
}
