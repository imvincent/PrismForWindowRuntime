// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Kona.UILogic.Models;

namespace Kona.UILogic.Services
{
    public class ShippingMethodServiceProxy : IShippingMethodService
    {
        private string _clientBaseUrl = string.Format("{0}/api/ShippingMethod/", Constants.ServerAddress);

        public async Task<ObservableCollection<ShippingMethod>> GetShippingMethodsAsync()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(_clientBaseUrl + "getall");
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsAsync<ObservableCollection<ShippingMethod>>();

                return result;
            }
        }

        public async Task<ShippingMethod> GetDefaultShippingMethodAsync()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(_clientBaseUrl + "default");
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsAsync<ShippingMethod>();

                return result;
            }
        }
    }
}
