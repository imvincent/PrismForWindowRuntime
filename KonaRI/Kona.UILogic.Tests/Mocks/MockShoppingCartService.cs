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
        public Func<string, string, Task> RemoveShoppingCartItemAsyncDelegate { get; set; }

        public System.Threading.Tasks.Task<Models.ShoppingCart> GetShoppingCartAsync(string shoppingCartId)
        {
            return GetShoppingCartAsyncDelegate(shoppingCartId);
        }

        public System.Threading.Tasks.Task<Models.ShoppingCartItem> AddProductToShoppingCartAsync(string shoppingCartId, string productId)
        {
            return AddProductToShoppingCartAsyncDelegate(shoppingCartId, productId);
        }

        public System.Threading.Tasks.Task RemoveShoppingCartItemAsync(string shoppingCartId, string itemId)
        {
            return RemoveShoppingCartItemAsyncDelegate(shoppingCartId, itemId);
        }
    }
}
