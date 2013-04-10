// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using Microsoft.Practices.Prism.PubSubEvents;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockEventAggregator : IEventAggregator
    {
        public Func<Type, EventBase> GetEventDelegate { get; set; }

        public TEventType GetEvent<TEventType>() where TEventType : EventBase, new()
        {
            return (TEventType)GetEventDelegate(typeof(TEventType));
        }
    }
}
