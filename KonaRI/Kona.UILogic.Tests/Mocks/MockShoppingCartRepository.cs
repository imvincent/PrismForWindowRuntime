// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Threading.Tasks;
using Kona.UILogic.Repositories;
using Kona.UILogic.Models;

namespace Kona.UILogic.Tests.Mocks
{
    public class MockShoppingCartRepository : IShoppingCartRepository
    {
        public Func<Task<ShoppingCart>> GetShoppingCartAsyncDelegate { get; set; }

        public Action<string> AddProductToShoppingCartAsyncDelegate { get; set; }

        public Action<int> RemoveShoppingCartItemDelegate { get; set; }

        public Task ClearCartAsync()
        {
            return Task.Factory.StartNew(() => { });
        }

        public Task<ShoppingCart> GetShoppingCartAsync()
        {
            return this.GetShoppingCartAsyncDelegate();
        }

        public void AddProductToShoppingCartAsync(string productId)
        {
            AddProductToShoppingCartAsyncDelegate(productId);
        }

        public void RemoveShoppingCartItemAsync(int itemId)
        {
            RemoveShoppingCartItemDelegate(itemId);
        }

        public UserInfo CurrentUser { get; set; }
    }
}
