// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using Kona.UILogic.Events;

namespace Kona.UILogic.Tests.Mocks
{
    public class MockShoppingCartUpdatedEvent : ShoppingCartUpdatedEvent
    {
        public Action<string> PublishDelegate { get; set; }

        public override void Publish(string payload)
        {
            PublishDelegate(payload);
        }
    }
}
