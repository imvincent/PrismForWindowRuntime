// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using Kona.WebServices.Models;
using Kona.WebServices.Repositories;

namespace Kona.WebServices.Tests.Mocks
{
    public class MockShoppingCartRepository : IShoppingCartRepository
    {
        public Func<string, ShoppingCart> GetDelegate { get; set; }

        public Func<string, string, ShoppingCartItem> AddProductToCartDelegate { get; set; }

        public Func<string, string, bool> RemoveDelegate { get; set; }

        public Func<string, ShoppingCartItem> CreateShoppingCartItemDelegate { get; set; } 

        public ShoppingCart GetShoppingCart(string id)
        {
            return this.GetDelegate(id);
        }

        public bool Remove(string id, string itemId)
        {
            return this.RemoveDelegate(id, itemId);
        }

        public ShoppingCartItem AddProductToCart(string userId, string productId)
        {
            return AddProductToCartDelegate(userId, productId);
        }

        public ShoppingCartItem CreateShoppingCartItem(string productId)
        {
            return CreateShoppingCartItemDelegate(productId);
        }
    }
}
