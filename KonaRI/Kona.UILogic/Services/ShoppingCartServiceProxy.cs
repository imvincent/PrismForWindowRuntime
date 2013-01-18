// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;

namespace Kona.UILogic.Services
{
    public class ShoppingCartServiceProxy : IShoppingCartService
    {
        private string _shoppingCartBaseUrl = string.Format("{0}/api/ShoppingCart/", Constants.ServerAddress);

        public async Task<ShoppingCart> GetShoppingCartAsync(string shoppingCartId)
        {
            using (var shoppingCartClient = new HttpClient())
            {
                var response = await shoppingCartClient.GetAsync(_shoppingCartBaseUrl + shoppingCartId);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsAsync<ShoppingCart>();

                return result;
            }
        }

        public async Task<ShoppingCartItem> AddProductToShoppingCartAsync(string shoppingCartId, string productId)
        {
            using (var shoppingCartClient = new HttpClient())
            {
                string requestUri = _shoppingCartBaseUrl + shoppingCartId;
                var response = await shoppingCartClient.PostAsJsonAsync<string>(requestUri, productId);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsAsync<ShoppingCartItem>();
            }
        }

        public async void RemoveShoppingCartItemAsync(string shoppingCartId, int itemId)
        {
            using (var shoppingCartClient = new HttpClient())
            {
                var response = await shoppingCartClient.DeleteAsync(_shoppingCartBaseUrl + shoppingCartId + "?itemId=" + itemId);
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task DeleteShoppingCartAsync(string shoppingCartId)
        {
            using (var shoppingCartClient = new HttpClient())
            {
                var response = await shoppingCartClient.DeleteAsync(_shoppingCartBaseUrl + shoppingCartId);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
