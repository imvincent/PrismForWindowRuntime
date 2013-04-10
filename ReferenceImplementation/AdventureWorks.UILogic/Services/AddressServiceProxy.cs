// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using Microsoft.Practices.Prism.StoreApps;
using System.Globalization;

namespace AdventureWorks.UILogic.Services
{
    public class AddressServiceProxy : IAddressService
    {
        private readonly IAccountService _accountService;
        private string _clientBaseUrl = string.Format(CultureInfo.InvariantCulture, "{0}/api/Address", Constants.ServerAddress);

        public AddressServiceProxy(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IReadOnlyCollection<Address>> GetAddressesAsync()
        {
            using (var handler = new HttpClientHandler { CookieContainer = new CookieContainer() })
            {
                using (var client = new HttpClient(handler))
                {
                    var serverUri = new Uri(Constants.ServerAddress);
                    handler.CookieContainer.SetCookies(serverUri, _accountService.ServerCookieHeader);
                    var response = await client.GetAsync(_clientBaseUrl);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsAsync<ReadOnlyCollection<Address>>();
                }
            }
        }

        public async Task SaveAddressAsync(Address address)
        {
            using (var handler = new HttpClientHandler { CookieContainer = new CookieContainer() })
            {
                using (var client = new HttpClient(handler))
                {
                    var serverUri = new Uri(Constants.ServerAddress);
                    handler.CookieContainer.SetCookies(serverUri, _accountService.ServerCookieHeader);
                    var response = await client.PostAsync(_clientBaseUrl, address, new JsonMediaTypeFormatter());
                    await response.EnsureSuccessWithValidationSupportAsync();
                }
            }
        }

        public async Task SetDefault(string defaultAddressId, AddressType addressType)
        {
            using (var handler = new HttpClientHandler { CookieContainer = new CookieContainer() })
            {
                using (var client = new HttpClient(handler))
                {
                    var serverUri = new Uri(Constants.ServerAddress);
                    handler.CookieContainer.SetCookies(serverUri, _accountService.ServerCookieHeader);
                    var response = await client.PutAsync(_clientBaseUrl + "/?defaultAddressId=" + defaultAddressId + "&addressType=" + addressType, null);
                    await response.EnsureSuccessWithValidationSupportAsync();
                }
            }
        }
    }
}
