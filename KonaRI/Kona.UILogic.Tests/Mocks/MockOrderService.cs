// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.Services;

namespace Kona.UILogic.Tests.Mocks
{
    public class MockOrderService : IOrderService
    {
        public Func<Order, string, Task<OrderSubmissionResult>> SubmitOrderDelegate { get; set; }
        public Task<OrderSubmissionResult> SubmitOrder(Order order, string serverCookieHeader)
        {
            return SubmitOrderDelegate(order, serverCookieHeader);
        }

        public Task<OrderSubmissionResult> CreateOrderAsync(Order order, string serverCookieHeader)
        {
            throw new NotImplementedException();
        }

        public Task<OrderSubmissionResult> UpdateOrderAsync(Order order, string serverCookieHeader)
        {
            throw new NotImplementedException();
        }


        public Task<OrderSubmissionResult> ProcessOrderAsync(Order order, string serverCookieHeader)
        {
            throw new NotImplementedException();
        }
    }
}
