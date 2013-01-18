// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.Services;
namespace Kona.UILogic.Tests.Mocks
{
    public class MockShoppingCartService : IShoppingCartService
    {
        public Func<string, Task<ShoppingCart>> GetShoppingCartAsyncDelegate { get; set; }
        public Func<string, string, Task<ShoppingCartItem>> AddProductToShoppingCartAsyncDelegate { get; set; }
        public Action<string, int> RemoveShoppingCartItemDelegate { get; set; }
        public Func<string, Task> DeleteShoppingCartAsyncDelegate { get; set; }

        public Task<ShoppingCart> GetShoppingCartAsync(string shoppingCartId)
        {
            return GetShoppingCartAsyncDelegate(shoppingCartId);
        }

        public Task<ShoppingCartItem> AddProductToShoppingCartAsync(string shoppingCartId, string productId)
        {
            return AddProductToShoppingCartAsyncDelegate(shoppingCartId, productId);
        }

        public void RemoveShoppingCartItemAsync(string shoppingCartId, int itemId)
        {
            RemoveShoppingCartItemDelegate(shoppingCartId, itemId);
        }

        public Task DeleteShoppingCartAsync(string shoppingCartId)
        {
            return DeleteShoppingCartAsyncDelegate(shoppingCartId);
        }
    }
}
