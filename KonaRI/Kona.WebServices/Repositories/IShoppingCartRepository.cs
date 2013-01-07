// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.WebServices.Models;

namespace Kona.WebServices.Repositories
{
    public interface IShoppingCartRepository
    {
        ShoppingCart GetShoppingCart(string userId);
        bool Remove(string userId, string itemId);
        ShoppingCartItem AddProductToCart(string userId, string productId);
    }
}