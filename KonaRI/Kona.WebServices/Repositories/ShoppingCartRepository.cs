// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Linq;
using Kona.WebServices.Models;

namespace Kona.WebServices.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        // key: user | value: shopping cart items
        private static Dictionary<string, ShoppingCart> _shoppingCarts = new Dictionary<string, ShoppingCart>();

        public ShoppingCart GetByUserId(string userId)
        {
            return _shoppingCarts.ContainsKey(userId) ? _shoppingCarts[userId] : null;
        }

        public ShoppingCartItem AddProductToCart(string userId, Product product)
        {
            ShoppingCart shoppingCart = GetByUserId(userId);

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart(new List<ShoppingCartItem>())
                {
                    UserId = userId,
                    Currency = "USD",
                    TaxRate = .09
                };

                Create(shoppingCart);
            }

            ShoppingCartItem item = shoppingCart.ShoppingCartItems.FirstOrDefault(c => c.Product.ProductNumber == product.ProductNumber);

            if (item == null)
            {
                item = new ShoppingCartItem
                {
                    Id = shoppingCart.ShoppingCartItems.Any() ? shoppingCart.ShoppingCartItems.Max(c => c.Id) : 1,
                    Product = product,
                    Quantity = 1,
                    Currency = shoppingCart.Currency
                };

                shoppingCart.ShoppingCartItems.Add(item);
            }
            else
            {
                item.Quantity++;
            }

            UpdatePrices(shoppingCart);
            return item;
        }

        public bool RemoveItemFromCart(ShoppingCart shoppingCart, int itemId)
        {
            if (shoppingCart == null)
            {
                throw new ArgumentNullException("shoppingCart");
            }

            ShoppingCartItem item = shoppingCart.ShoppingCartItems.FirstOrDefault(i => i.Id == itemId);
            bool itemRemoved = shoppingCart.ShoppingCartItems.Remove(item);

            if (itemRemoved)
            {
                UpdatePrices(shoppingCart);
            }

            return itemRemoved;
        }

        public ShoppingCart Create(ShoppingCart item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            item.Id = _shoppingCarts.Values.Any() ? _shoppingCarts.Values.Max(c => c.Id) : 1;

            if (_shoppingCarts.ContainsKey(item.UserId))
            {
                _shoppingCarts.Add(item.UserId, item);
            }
            else
            {
                _shoppingCarts[item.UserId] = item;
            }

            return item;
        }

        public bool Delete(ShoppingCart item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (_shoppingCarts.ContainsKey(item.UserId))
            {
                _shoppingCarts.Remove(item.UserId);
                return true;
            }

            return false;
        }

        public IEnumerable<ShoppingCart> GetAll()
        {
            throw new NotImplementedException();
        }

        public ShoppingCart GetItem(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(ShoppingCart item)
        {
            throw new NotImplementedException();
        }

        private static void UpdatePrices(ShoppingCart shoppingCart)
        {
            double fullPrice = 0, discount = 0;
            foreach (var shoppingCartItem in shoppingCart.ShoppingCartItems)
            {
                fullPrice += shoppingCartItem.Product.ListPrice * shoppingCartItem.Quantity;
                discount += fullPrice * shoppingCartItem.DiscountPercentage / 100;
                shoppingCart.FullPrice = fullPrice;
                shoppingCart.TotalDiscount = discount;
                shoppingCart.TotalPrice = fullPrice - discount;
            }
        }
    }
}