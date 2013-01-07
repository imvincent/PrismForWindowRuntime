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

        private Dictionary<string, ShoppingCart> shoppingCarts;

        public ShoppingCartRepository()
        {
            this.shoppingCarts = new Dictionary<string, ShoppingCart>
                                {
                                    { "JohnDoe", new ShoppingCart(new List<ShoppingCartItem>()
                                        {   new ShoppingCartItem
                                                {Id = Guid.NewGuid(), Currency = "USD", Product = new Product {Title = "Item1 with a long long long long description.", Description="Only 3 units left!", ImageName = "image1"}, Quantity = 1, IsGift = true, DiscountPercentage = 10},
                                            new ShoppingCartItem
                                                {Id = Guid.NewGuid(), Currency = "USD", Product = new Product {Title = "Item2 Loremp Ipsum dolor", Description="Eligible for free shipping", ImageName = "image2"}, Quantity = 2, DiscountPercentage = 10},
                                            new ShoppingCartItem
                                                {Id = Guid.NewGuid(), Currency = "USD", Product = new Product {Title = "Item3", Description="", ImageName = "image3"}, Quantity = 3, DiscountPercentage = 10}
                                        })
                                            {FullPrice = 350, Id = Guid.NewGuid(), TotalDiscount = 10, TaxRate = .09, Currency = "USD"}
                                     }
                                };
        }

        public ShoppingCart GetShoppingCart(string userId)
        {
            return this.shoppingCarts.ContainsKey(userId) ? this.shoppingCarts[userId] : null;
        }

        public ShoppingCartItem AddProductToCart(string userId, string productId)
        {
            ShoppingCart shoppingCart = GetShoppingCart(userId);
            if (shoppingCart == null)
            {
                var shoppingCartItem = CreateShoppingCartItem(productId);
                shoppingCart = new ShoppingCart(new List<ShoppingCartItem>()){Currency = "USD", TaxRate = .09};
                shoppingCart.ShoppingCartItems.Add(shoppingCartItem);

                UpdatePrices(shoppingCart);

                shoppingCarts.Add(userId, shoppingCart);

                return shoppingCartItem;
            }
            var matchingShoppingCartItem =
                shoppingCart.ShoppingCartItems.FirstOrDefault(
                    item => item.Product != null && item.Product.ProductNumber == productId);
            
            if (matchingShoppingCartItem != null)
            {
                matchingShoppingCartItem.Quantity++;
                UpdatePrices(shoppingCart);
                return matchingShoppingCartItem;
            }

            var newShoppingCartItem = CreateShoppingCartItem(productId);
            shoppingCart.ShoppingCartItems.Add(newShoppingCartItem);

            UpdatePrices(shoppingCart);
            return newShoppingCartItem;
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

        public bool Remove(string userId, string itemId)
        {
            var shoppingcart = GetShoppingCart(userId);
            var itemGuid = new Guid(itemId);
            var item = shoppingcart.ShoppingCartItems.FirstOrDefault(i => i.Id == itemGuid);
            return shoppingcart.ShoppingCartItems.Remove(item);
        }

        private static ShoppingCartItem CreateShoppingCartItem(string productId)
        {
            var product = ProductRepository.Products.Where(p => p.ProductNumber == productId).FirstOrDefault();
            return new ShoppingCartItem
                       {
                           Product = product,
                           Quantity = 1,
                           Currency = "USD"
                       };
        }
    }
}