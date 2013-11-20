// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Globalization;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using Newtonsoft.Json;
using Windows.Web.Http;
using System;

namespace AdventureWorks.UILogic.Services
{
    public class ShoppingCartServiceProxy : IShoppingCartService
    {
        private string _shoppingCartBaseUrl = string.Format(CultureInfo.InvariantCulture, "{0}/api/ShoppingCart/", Constants.ServerAddress);

        public async Task<ShoppingCart> GetShoppingCartAsync(string shoppingCartId)
        {
            using (var shoppingCartClient = new HttpClient())
            {
                var response = await shoppingCartClient.GetAsync(new Uri(_shoppingCartBaseUrl + shoppingCartId));
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ShoppingCart>(responseContent);
                return result;
            }
        }

        public async Task AddProductToShoppingCartAsync(string shoppingCartId, string productIdToIncrement)
        {
            using (var shoppingCartClient = new HttpClient())
            {
                string requestUrl = _shoppingCartBaseUrl + shoppingCartId + "?productIdToIncrement=" + productIdToIncrement;
                var response = await shoppingCartClient.PostAsync(new Uri(requestUrl), null);
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task RemoveProductFromShoppingCartAsync(string shoppingCartId, string productIdToDecrement)
        {
            using (var shoppingCartClient = new HttpClient())
            {
                string requestUrl = _shoppingCartBaseUrl + shoppingCartId + "?productIdToDecrement=" + productIdToDecrement;
                var response = await shoppingCartClient.PostAsync(new Uri(requestUrl), null);
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task RemoveShoppingCartItemAsync(string shoppingCartId, string itemIdToRemove)
        {
            using (var shoppingCartClient = new HttpClient())
            {
                string requestUrl = _shoppingCartBaseUrl + shoppingCartId + "?itemIdToRemove=" + itemIdToRemove;
                var response = await shoppingCartClient.PutAsync(new Uri(requestUrl), null);
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task DeleteShoppingCartAsync(string shoppingCartId)
        {
            using (var shoppingCartClient = new HttpClient())
            {
                var response = await shoppingCartClient.DeleteAsync(new Uri(_shoppingCartBaseUrl + shoppingCartId));
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task<bool> MergeShoppingCartsAsync(string anonymousShoppingCartId, string authenticatedShoppingCartId)
        {
            using (var shoppingCartClient = new HttpClient())
            {
                string requestUrl = _shoppingCartBaseUrl + authenticatedShoppingCartId + "?anonymousShoppingCartId=" + anonymousShoppingCartId;
                var response = await shoppingCartClient.PostAsync(new Uri(requestUrl), null);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<bool>(responseContent);

                return result;
            }
        }
    }
}
