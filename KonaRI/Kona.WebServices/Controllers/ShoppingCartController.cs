// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.WebServices.Models;
using Kona.WebServices.Repositories;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kona.WebServices.Controllers
{
    public class ShoppingCartController : ApiController
    {
        private IShoppingCartRepository _shoppingCartRepository;
        private IRepository<Product> _productRepository;

        public ShoppingCartController()
            : this(new ShoppingCartRepository(), new ProductRepository())
        { }

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IRepository<Product> productRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productRepository = productRepository;
        }

        // GET /api/ShoppingCart/id
        public ShoppingCart Get(string id)
        {
            return _shoppingCartRepository.GetByUserId(id);
        }

        // POST /api/ShoppingCart/5
        public HttpResponseMessage Post(string id, [FromBody] string productNumber)
        {
            var product = _productRepository.GetAll().FirstOrDefault(c => c.ProductNumber == productNumber);
            var shoppingCartItem = _shoppingCartRepository.AddProductToCart(id, product);

            var response = Request.CreateResponse<ShoppingCartItem>(HttpStatusCode.Created, shoppingCartItem);
            string uri = Url.Link("DefaultApi", new { id = shoppingCartItem.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        // DELETE /api/ShoppingCart/5
        public void DeleteShoppingCart(string id)
        {
            ShoppingCart shoppingCart = _shoppingCartRepository.GetByUserId(id);

            if (shoppingCart == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            _shoppingCartRepository.Delete(shoppingCart);
        }

        // DELETE /api/ShoppingCart/5?itemId={itemId}
        public void DeleteShoppingCartItem(string id, int itemId)
        {
            ShoppingCart shoppingCart = _shoppingCartRepository.GetByUserId(id);

            if (shoppingCart == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            if (!_shoppingCartRepository.RemoveItemFromCart(shoppingCart, itemId))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}
