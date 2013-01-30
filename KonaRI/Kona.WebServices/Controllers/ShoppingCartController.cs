// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Web.Http;
using Kona.WebServices.Models;
using Kona.WebServices.Repositories;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

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
            return _shoppingCartRepository.GetById(id);
        }

        // PUT /api/ShoppingCart/{id}?productIdToIncrement={productIdToIncrement}
        [HttpPut]
        public void AddProductToShoppingCart(string id, string productIdToIncrement)
        {
            var product = _productRepository.GetAll().FirstOrDefault(c => c.ProductNumber == productIdToIncrement);
            if (product == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);                    
            }

            _shoppingCartRepository.AddProductToCart(id, product);
        }

        // PUT /api/ShoppingCart/{id}?productIdToIncrement={productIdToIncrement}
        //This action decrements the quantity of an item in the shopping cart. 
        //For example, if a shopping cart has 3 socks, this action will update the quantity to 2.
        [HttpPut]
        public void RemoveProductFromShoppingCart(string id, string productIdToDecrement)
        {
            var product = _productRepository.GetAll().FirstOrDefault(c => c.ProductNumber == productIdToDecrement);
            if (product == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            if (!_shoppingCartRepository.RemoveProductFromCart(id, productIdToDecrement))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        // DELETE /api/ShoppingCart/{id}
        public void DeleteShoppingCart(string id)
        {
            ShoppingCart shoppingCart = _shoppingCartRepository.GetById(id);

            if (shoppingCart == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            _shoppingCartRepository.Delete(shoppingCart);
        }

        // PUT /api/ShoppingCart/{id}?itemIdToRemove={itemIdToRemove}
        //This action removes the entire item group from the shopping cart. 
        //For example, if a shopping cart has 2 socks and 3 bikes, this action will remove socks.
        [HttpPut]
        public void RemoveShoppingCartItem(string id, string itemIdToRemove)
        {
            ShoppingCart shoppingCart = _shoppingCartRepository.GetById(id);

            if (shoppingCart == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            if (!_shoppingCartRepository.RemoveItemFromCart(shoppingCart, itemIdToRemove))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        // PUT /api/ShoppingCart/{id}?oldShoppingCartId={oldShoppingCartId}
        [HttpPut]
        public HttpResponseMessage MergeShoppingCarts(string id, string oldShoppingCartId)
        {
            if (id == oldShoppingCartId) return new HttpResponseMessage(HttpStatusCode.OK);

            var oldCart = _shoppingCartRepository.GetById(oldShoppingCartId);

            if (oldCart != null)
            {
                //Merge shopping carts by adding items from old cart to new cart.
                foreach (var shoppingCartItem in oldCart.ShoppingCartItems)
                {
                    for (int i = 0; i < shoppingCartItem.Quantity; i++)
                    {
                        _shoppingCartRepository.AddProductToCart(id, shoppingCartItem.Product);
                    }
                }

                //Delete old cart
                _shoppingCartRepository.Delete(oldCart);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
