// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using Microsoft.Practices.StoreApps.Infrastructure;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.UILogic.Services
{
    public class ValidationServiceProxy : IValidationService
    {
        private string _clientBaseUrl = string.Format(CultureInfo.InvariantCulture, "{0}/api/Validation", Constants.ServerAddress);

        public async Task<bool> ValidateAddressAsync(Address address)
        {
            using (var client = new HttpClient())
            {
                client.AddCurrentCultureHeader();

                string requestUrl = string.Format(CultureInfo.InvariantCulture, "{0}/ValidateAddress", _clientBaseUrl);
                var response = await client.PostAsJsonAsync<Address>(requestUrl, address);
                await response.EnsureSuccessWithValidationSupportAsync();

                return await response.Content.ReadAsAsync<bool>();
            }
        }
    }
}
