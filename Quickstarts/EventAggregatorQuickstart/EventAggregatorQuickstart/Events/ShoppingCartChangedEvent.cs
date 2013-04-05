// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Microsoft.Practices.PubSubEvents;

namespace EventAggregatorQuickstart
{
    // This QuickStart is documented at http://go.microsoft.com/fwlink/?LinkID=288828&clcid=0x409

    // <snippet3100>
    public class ShoppingCartChangedEvent : PubSubEvent<ShoppingCart> { }
    // </snippet3100>
}
