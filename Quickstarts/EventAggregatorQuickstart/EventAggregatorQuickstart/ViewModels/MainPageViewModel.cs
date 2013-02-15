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
    public class MainPageViewModel : ViewModel
    {
        public MainPageViewModel(IEventAggregator eventAggregator)
        {
            // Pass the injected event aggregator singleton down to children since there is no container to do the dependency injection
            SubscriberViewModel = new SubscriberViewModel(eventAggregator);
            PublisherViewModel = new PublisherViewModel(eventAggregator);
        }

        public SubscriberViewModel SubscriberViewModel { get; set; }
        public PublisherViewModel PublisherViewModel { get; set; }

        public string ThreadMessage { get { return string.Format("UI Thread ID: {0}", Environment.CurrentManagedThreadId); } }
    }
}
