// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.Infrastructure;
using Microsoft.Practices.PubSubEvents;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EventAggregatorQuickstart
{
    public class PublisherViewModel : ViewModel
    {
        IEventAggregator _eventAggregator;
        ShoppingCart _cart = new ShoppingCart();

        public PublisherViewModel(IEventAggregator eventAggregator)
        {
            // Get the singleton event aggregator injected from parent
            _eventAggregator = eventAggregator;
            AddItemToCartUIThreadCommand = new DelegateCommand(PublishOnUIThread);
            AddItemToCartBackgroundThreadCommand = new DelegateCommand(PublishOnBackgroundThread);
            Debug.WriteLine(String.Format("UI thread is: {0}", Environment.CurrentManagedThreadId));
        }

        public DelegateCommand AddItemToCartUIThreadCommand { get; private set; }
        public DelegateCommand AddItemToCartBackgroundThreadCommand { get; private set; }

        // <snippet3103>
        private void PublishOnUIThread()
        {
            AddItemToCart();
            lock (_cart) // lock for multithreaded access to cart
            {
                // Fire the event on the UI thread
                _eventAggregator.GetEvent<ShoppingCartChangedEvent>().Publish(_cart);
            }
        }

        private void PublishOnBackgroundThread()
        {
            AddItemToCart();
            Task.Factory.StartNew(() => 
                {
                    lock (_cart) // lock for multithreaded access to cart
                    {
                        // Fire the event on a background thread
                        _eventAggregator.GetEvent<ShoppingCartChangedEvent>().Publish(_cart);
                        Debug.WriteLine(String.Format("Publishing from thread: {0}", Environment.CurrentManagedThreadId));
                    }
                });
        }

        private void AddItemToCart()
        {
            lock (_cart) // lock for multithreaded access to cart
            {
                ShoppingCartItem item = new ShoppingCartItem { Name = "Widget", Cost = 19.99m };
                _cart.Items.Add(item);
            }
        }
        // </snippet3103>
    }
}
