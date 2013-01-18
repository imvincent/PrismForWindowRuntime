// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Threading.Tasks;
using Kona.UILogic.Models;

namespace Kona.UILogic.Services
{
    public interface IShoppingCartService
    {
        Task<ShoppingCart> GetShoppingCartAsync(string shoppingCartId);
        Task<ShoppingCartItem> AddProductToShoppingCartAsync(string shoppingCartId, string productId);
        void RemoveShoppingCartItemAsync(string shoppingCartId, int itemId);
        Task DeleteShoppingCartAsync(string shoppingCartId);
    }
}
