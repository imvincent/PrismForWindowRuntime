// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.WebServices.Models;
using Kona.WebServices.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kona.WebServices.Controllers
{
    public class ShoppingCartController : ApiController
    {
        private static IShoppingCartRepository repository;

        public ShoppingCartController()
        {
            if (repository == null)
            {
                repository = new ShoppingCartRepository();
            }
        }

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository)
        {
            repository = shoppingCartRepository;
        }

        // GET /api/ShoppingCart/id
        public ShoppingCart Get(string id)
        {
            var item = repository.GetShoppingCart(id);
            return item;
        }

        // POST /api/ShoppingCart/5
        public HttpResponseMessage Post(string id, [FromBody] string productId)
        {
            var shoppingCartItem = repository.AddProductToCart(id, productId);
            
            var response = Request.CreateResponse<ShoppingCartItem>(HttpStatusCode.Created, shoppingCartItem);
            string uri = Url.Link("DefaultApi", new { id = shoppingCartItem.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        // DELETE /api/ShoppingCart/5
        public void DeleteShoppingCartItem(string id, string itemId)
        {
            var item = repository.GetShoppingCart(id);

            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            if (!repository.Remove(id, itemId))
                throw new HttpResponseException(HttpStatusCode.NotFound);
        }
    }
}
