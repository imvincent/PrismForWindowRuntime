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

        static ShoppingCartRepository()
        {
            var shoppingCartItems = new List<ShoppingCartItem>();
            shoppingCartItems.Add(new ShoppingCartItem { Currency = "USD", Quantity = 1, Product = new Product { Currency = "USD", Title = "Mountain-400-W Silver, 42", ProductNumber = "BK-M38S-42", SubcategoryId = 1, Description = "This bike delivers a high-level of performance on a budget. It is responsive and maneuverable, and offers peace-of-mind when you decide to go off-road.", ListPrice = 769.4900, Weight = 27.13, Color = "Silver", ImageName = "hotrodbike_f_large.gif" } });
            shoppingCartItems.Add(new ShoppingCartItem { Currency = "USD", Quantity = 1, Product = new Product { Currency = "USD", Title = "Touring Pedal", ProductNumber = "PD-T852", SubcategoryId = 13, Description = "A stable pedal for all-day riding.", ListPrice = 80.9900, Weight = 0, Color = "Silver/Black", ImageName = "clipless_pedals_large.gif" } });
            shoppingCartItems.Add(new ShoppingCartItem { Currency = "USD", Quantity = 1, Product = new Product { Currency = "USD", Title = "LL Touring Frame - Yellow, 62", ProductNumber = "FR-T67Y-62", SubcategoryId = 16, Description = "Lightweight butted aluminum frame provides a more upright riding position for a trip around town.  Our ground-breaking design provides optimum comfort.", ListPrice = 333.4200, Weight = 3.20, Color = "Yellow", ImageName = "frame_large.gif" } });
            _shoppingCarts.Add("JohnDoe", new ShoppingCart(shoppingCartItems) { Currency = "USD", FullPrice = 1183.90, TotalPrice = 1183.90});
        }

        public ShoppingCart GetById(string userId)
        {
            return _shoppingCarts.ContainsKey(userId) ? _shoppingCarts[userId] : null;
        }

        public ShoppingCartItem AddProductToCart(string userId, Product product)
        {
            ShoppingCart shoppingCart = GetById(userId);

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
                    Id = product.ProductNumber,
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

        public bool RemoveProductFromCart(string userId, string productId)
        {
            ShoppingCart shoppingCart = GetById(userId);
            if (shoppingCart == null) return false;

            var shoppingCartItem =
                shoppingCart.ShoppingCartItems.FirstOrDefault((item) => item.Product.ProductNumber == productId);

            if (shoppingCartItem == null) return false;

            shoppingCartItem.Quantity--;

            return true;
        }

        public bool RemoveItemFromCart(ShoppingCart shoppingCart, string itemId)
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