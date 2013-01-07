// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kona.WebServices.Models;

namespace Kona.WebServices.Controllers
{
    [Authorize]
    public class OrderController : ApiController
    {
        private static List<Order> _orders = new List<Order>();
        private const string serviceAddress = "/api/Order/";

        // POST /api/Order 
        public HttpResponseMessage PostOrder(Order order)
        {
            if (order == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            if (_orders.Count > 0) order.OrderId = _orders.Max(o => o.OrderId) + 1;
            else order.OrderId = 1;
            _orders.Add(order);
            // Returning order because of server generated id or other server computed fields
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, order);
            string uri = Url.Link("DefaultApi", new { id = order.OrderId });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        // GET /api/Order/id
        public Order GetOrder(int id)
        {
            var order = _orders.FirstOrDefault(o => o.OrderId == id);
            if (order == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            return order;
        }
    }
}
