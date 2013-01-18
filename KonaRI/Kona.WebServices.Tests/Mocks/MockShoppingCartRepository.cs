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
        public Func<string, ShoppingCart> GetShoppingCartDelegate { get; set; }

        public Func<string, Product, ShoppingCartItem> AddProductToCartDelegate { get; set; }

        public Func<ShoppingCart, string, bool> RemoveItemFromCartDelegate { get; set; }

        public Func<string, ShoppingCartItem> CreateShoppingCartItemDelegate { get; set; }

        public Func<string, bool> DeleteCartDelegate { get; set; }

        public ShoppingCart GetShoppingCart(string id)
        {
            return this.GetShoppingCartDelegate(id);
        }

        public bool RemoveItemFromCart(ShoppingCart shoppingCart, string itemId)
        {
            return this.RemoveItemFromCartDelegate(shoppingCart, itemId);
        }

        public ShoppingCartItem AddProductToCart(string userId, Product productId)
        {
            return AddProductToCartDelegate(userId, productId);
        }

        public bool DeleteCart(string userId)
        {
            return DeleteCartDelegate(userId);
        }

        public ShoppingCartItem CreateShoppingCartItem(string productId)
        {
            return CreateShoppingCartItemDelegate(productId);
        }

        public ShoppingCart GetByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveItemFromCart(ShoppingCart shoppingCart, int itemId)
        {
            throw new NotImplementedException();
        }

        public System.Collections.Generic.IEnumerable<ShoppingCart> GetAll()
        {
            throw new NotImplementedException();
        }

        public ShoppingCart GetItem(int id)
        {
            throw new NotImplementedException();
        }

        public ShoppingCart Create(ShoppingCart item)
        {
            throw new NotImplementedException();
        }

        public bool Update(ShoppingCart item)
        {
            throw new NotImplementedException();
        }

        public bool Delete(ShoppingCart item)
        {
            throw new NotImplementedException();
        }

        ShoppingCart IShoppingCartRepository.GetByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        ShoppingCartItem IShoppingCartRepository.AddProductToCart(string userId, Product product)
        {
            throw new NotImplementedException();
        }

        bool IShoppingCartRepository.RemoveItemFromCart(ShoppingCart shoppingCart, int itemId)
        {
            throw new NotImplementedException();
        }

        System.Collections.Generic.IEnumerable<ShoppingCart> IRepository<ShoppingCart>.GetAll()
        {
            throw new NotImplementedException();
        }

        ShoppingCart IRepository<ShoppingCart>.GetItem(int id)
        {
            throw new NotImplementedException();
        }

        ShoppingCart IRepository<ShoppingCart>.Create(ShoppingCart item)
        {
            throw new NotImplementedException();
        }

        bool IRepository<ShoppingCart>.Update(ShoppingCart item)
        {
            throw new NotImplementedException();
        }

        bool IRepository<ShoppingCart>.Delete(ShoppingCart item)
        {
            throw new NotImplementedException();
        }
    }
}
