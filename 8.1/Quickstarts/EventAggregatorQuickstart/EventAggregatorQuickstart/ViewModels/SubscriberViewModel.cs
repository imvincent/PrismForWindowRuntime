// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Microsoft.Practices.Prism.PubSubEvents;
using System;
using Microsoft.Practices.Prism.StoreApps;
using Windows.UI.Xaml;

namespace EventAggregatorQuickstart.ViewModels
{
    // This QuickStart is documented at http://go.microsoft.com/fwlink/?LinkID=288828&clcid=0x409

    public class SubscriberViewModel : ViewModel
    {
        private bool _showWarning;
        private int _itemsInCart = 0;
        private BackgroundSubscriber _subscriber;
        private IEventAggregator _eventAggregator;

        public SubscriberViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            AddBackgroundSubscriberCommand = new DelegateCommand(AddBackgroundSubscriber);
            GCBackgroundSubscriberCommand = new DelegateCommand(GCBackgroundSubscriber);

            // Subscribe indicating this handler should always be called on the UI Thread
            _eventAggregator.GetEvent<ShoppingCartChangedEvent>().Subscribe(HandleShoppingCartUpdate, ThreadOption.UIThread);
            // Subscribe indicating that this handler should always be called on UI thread, but only if more than 10 items in cart
            _eventAggregator.GetEvent<ShoppingCartChangedEvent>().Subscribe(HandleShoppingCartUpdateFiltered, ThreadOption.UIThread, false, IsCartCountPossiblyTooHigh);
        }

        public DelegateCommand AddBackgroundSubscriberCommand { get; private set; }
        public DelegateCommand GCBackgroundSubscriberCommand { get; private set; }

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
            ItemsInCart = cart.Count;
        }

        private void HandleShoppingCartUpdateFiltered(ShoppingCart cart)
        {
            ShowWarning = true;
        }

        private void AddBackgroundSubscriber()
        {
            // Create subscriber and hold on to it so it does not get garbage collected
            _subscriber = new BackgroundSubscriber(Window.Current.Dispatcher);
            // Subscribe with defaults, pointing to subscriber method that pops a message box when the event fires
            _eventAggregator.GetEvent<ShoppingCartChangedEvent>().Subscribe(_subscriber.HandleShoppingCartChanged);
        }

        private void GCBackgroundSubscriber()
        {
            // Release and GC, showing that we don't have to unsubscribe to keep the subscriber from being garbage collected
            _subscriber = null;
            GC.Collect();
        }

        private bool IsCartCountPossiblyTooHigh(ShoppingCart shoppingCart)
        {
            return (shoppingCart.Count > 10);
        }
    }
}
