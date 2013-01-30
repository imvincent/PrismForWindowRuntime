// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.Infrastructure;
using Microsoft.Practices.PubSubEvents;
using System;

namespace EventAggregatorQuickstart
{
    public class SubscriberViewModel : ViewModel
    {
        private bool _showWarning;
        private int _itemsInCart = 0;
        private NonUISubscriber _Subscriber;
        private IEventAggregator _eventAggregator;

        public SubscriberViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            AddPassiveSubscriberCommand = new DelegateCommand(AddWeakReferenceSubscriber);
            GCPassiveSubscriberCommand = new DelegateCommand(GCWeakReferenceSubscriber);

            // <snippet3104>
            // Subscribe indicating this handler should always be called on the UI Thread
            _eventAggregator.GetEvent<ShoppingCartChangedEvent>().Subscribe(HandleShoppingCartUpdate, ThreadOption.UIThread);
            // Subscribe indicating that this handler should always be called on UI thread, but only if more than 10 items in cart
            _eventAggregator.GetEvent<ShoppingCartChangedEvent>().Subscribe(HandleShoppingCartUpdateFiltered, ThreadOption.UIThread, false, sc => sc.Items.Count > 10);
            // </snippet3104>
        }

        public DelegateCommand AddPassiveSubscriberCommand { get; private set; }
        public DelegateCommand GCPassiveSubscriberCommand { get; private set; }

        public bool ShowWarning
        {
            get { return _showWarning; }
            set { SetProperty(ref _showWarning, value); }
        }

        public int ItemsInCart
        {
            get { return _itemsInCart; }
            set { SetProperty(ref _itemsInCart, value); }
        }

        private void HandleShoppingCartUpdate(ShoppingCart cart)
        {
            ItemsInCart = cart.Items.Count;
        }

        private void HandleShoppingCartUpdateFiltered(ShoppingCart cart)
        {
            ShowWarning = true;
        }

        // <snippet3105>
        private void AddWeakReferenceSubscriber()
        {
            // Create subscriber and hold on to it so it does not get garbage collected
            _Subscriber = new NonUISubscriber();
            // Subscribe with defaults, pointing to subscriber method that pops a message box when the event fires
            _eventAggregator.GetEvent<ShoppingCartChangedEvent>().Subscribe(_Subscriber.HandleShoppingCartChanged);
        }

        private void GCWeakReferenceSubscriber()
        {
            // Release and GC, showing that we don't have to unsubscribe to keep the subscriber from being garbage collected
            _Subscriber = null;
            GC.Collect();
        }
        // </snippet3105>
    }
}
