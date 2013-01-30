// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using Kona.WebServices.Models;

namespace Kona.WebServices.Repositories
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        ShoppingCart GetById(string shoppingCartId);
        ShoppingCartItem AddProductToCart(string shoppingCartId, Product product);
        bool RemoveProductFromCart(string shoppingCartId, string productId);
        bool RemoveItemFromCart(ShoppingCart shoppingCart, string itemId);
    }
}