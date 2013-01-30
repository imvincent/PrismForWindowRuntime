// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Kona.WebServices.Models;
using Kona.WebServices.Repositories;

namespace Kona.WebServices.Controllers
{
    [Authorize]
    public class OrderController : ApiController
    {
        private IRepository<Order> _orderRepository;

        public OrderController()
            : this(new OrderRepository())
        { }

        public OrderController(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // GET /api/order/id
        [HttpGet]
        public Order GetOrder(int id)
        {
            var order = _orderRepository.GetItem(id);

            if (order == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return order;
        }

        // POST /api/order/create
        [HttpPost]
        public HttpResponseMessage CreateOrder(Order order)
        {
            if (order == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid order");
            }

            if (ModelState.IsValid)
            {
                order = _orderRepository.Create(order);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, order);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = order.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // PUT /api/order/process 
        [HttpPut]
        public HttpResponseMessage ProcessOrder(int id, Order order)
        {
            if (order == null || id != order.Id)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid order");
            }

            if (ModelState.IsValid)
            {
                // TODO: add business logic validation (check stock, approve transaction, etc)
                // for instance, validate the transaction before performing the purchase
                var result = order.PaymentInfo.CardNumber != "22222" ? "APPROVED" : string.Format(CultureInfo.CurrentCulture, "Invalid Payment Info. Reason: {0}", "DECLINED_CONTACT_YOUR_BANK");

                if (result == "APPROVED")
                {
                    // TODO: Process the order
                    _orderRepository.Delete(order);
                    return Request.CreateResponse();
                }
                else
                {
                    ModelState.AddModelError("order.PaymentInfo", result);
                }
            }

            // Only get here if there are modelstate errors
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }
    }
}
