// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.Services
{
    public class ShippingMethodServiceProxy : IShippingMethodService
    {
        private string _clientBaseUrl = string.Format(CultureInfo.InvariantCulture, "{0}/api/ShippingMethod/", Constants.ServerAddress);

        public async Task<IEnumerable<ShippingMethod>> GetShippingMethodsAsync()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(_clientBaseUrl);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsAsync<IEnumerable<ShippingMethod>>();

                return result;
            }
        }

        public async Task<ShippingMethod> GetBasicShippingMethodAsync()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(_clientBaseUrl + "basic");
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsAsync<ShippingMethod>();

                return result;
            }
        }
    }
}
