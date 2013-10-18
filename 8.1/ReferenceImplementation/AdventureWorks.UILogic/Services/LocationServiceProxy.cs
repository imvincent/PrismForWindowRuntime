// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.StoreApps;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.Services
{
    public class LocationServiceProxy : ILocationService
    {
        private string _clientBaseUrl = string.Format(CultureInfo.InvariantCulture, "{0}/api/Location", Constants.ServerAddress);

        public async Task<IReadOnlyCollection<string>> GetStatesAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(_clientBaseUrl);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsAsync<ReadOnlyCollection<string>>();
                return result;
            }
        }
    }
}
