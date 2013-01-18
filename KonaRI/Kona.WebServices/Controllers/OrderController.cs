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
        [ActionName("Create")]
        public HttpResponseMessage CreateOrder(Order order)
        {
            if (order == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            object responseContent = null;
            HttpStatusCode statusCode = HttpStatusCode.Created;

            if (ModelState.IsValid)
            {
                order = _orderRepository.Create(order);
                responseContent = new { IsValid = true, Order = order };
            }
            else
            {
                statusCode = HttpStatusCode.BadRequest;
                var errorMessages = ModelState.Select(c => new { Property = c.Key, Messages = c.Value.Errors.Select(d => d.ErrorMessage) });
                responseContent = new { IsValid = false, Order = order, Message = errorMessages };
            }

            // Send the response to the client
            HttpResponseMessage response = Request.CreateResponse(statusCode, responseContent);
            string uri = Url.Link("DefaultApi", null);
            response.Headers.Location = new Uri(uri);

            return response;
        }

        // POST /api/order/id
        [HttpPut]
        [ActionName("Update")]
        public HttpResponseMessage UpdateOrder(Order order)
        {
            if (order == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            object responseContent = null;
            HttpStatusCode statusCode = HttpStatusCode.OK;

            if (ModelState.IsValid)
            {
                _orderRepository.Update(order);
                responseContent = new { IsValid = true, Order = order };
            }
            else
            {
                statusCode = HttpStatusCode.BadRequest;
                var errorMessages = ModelState.Select(c => new { Property = c.Key, Messages = c.Value.Errors });
                responseContent = new { IsValid = true, Order = order, Message = errorMessages };
            }

            // Send the response to the client
            HttpResponseMessage response = Request.CreateResponse(statusCode, responseContent);
            string uri = Url.Link("DefaultApi", null);
            response.Headers.Location = new Uri(uri);

            return response;
        }

        // POST /api/order/process 
        [HttpPost]
        [ActionName("Process")]
        public HttpResponseMessage ProcessOrder(Order order)
        {
            if (order == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            object responseContent = null;
            HttpStatusCode statusCode = HttpStatusCode.OK;

            if (ModelState.IsValid)
            {
                // TODO: add business logic validation (check stock, approve transaction, etc)
                // for instance, validate the transaction before performing the purchase
                var result = order.PaymentInfo.CardNumber != "22222" ? "APPROVED" : string.Format(CultureInfo.CurrentCulture, "Invalid Payment Info. Reason: {0}", "DECLINED_CONTACT_YOUR_BANK");

                if (result == "APPROVED")
                {
                    // TODO: Process the order
                    _orderRepository.Delete(order);
                    responseContent = new { IsValid = true };
                }
                else
                {
                    statusCode = HttpStatusCode.BadRequest;
                    responseContent = new { IsValid = false, Order = order, Message = result };
                }
            }
            else
            {
                statusCode = HttpStatusCode.BadRequest;
                var errorMessages = ModelState.Values.SelectMany(c => c.Errors).Select(c => c.ErrorMessage);
                responseContent = new { IsValid = false, Order = order, Message = string.Join(Environment.NewLine, errorMessages) };
            }

            // Send the response to the client
            HttpResponseMessage response = Request.CreateResponse(statusCode, responseContent);
            string uri = Url.Link("DefaultApi", null);
            response.Headers.Location = new Uri(uri);

            return response;
        }
    }
}
