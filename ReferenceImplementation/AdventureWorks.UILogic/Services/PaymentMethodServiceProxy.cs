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
using Microsoft.Practices.StoreApps.Infrastructure;
using System.Globalization;

namespace AdventureWorks.UILogic.Services
{
    public class PaymentMethodServiceProxy : IPaymentMethodService
    {
        private readonly IAccountService _accountService;
        private string _clientBaseUrl = string.Format(CultureInfo.InvariantCulture, "{0}/api/PaymentMethod", Constants.ServerAddress);

        public PaymentMethodServiceProxy(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IReadOnlyCollection<PaymentMethod>> GetPaymentMethodsAsync()
        {
            using (var handler = new HttpClientHandler { CookieContainer = new CookieContainer() })
            {
                using (var client = new HttpClient(handler))
                {
                    client.AddCurrentCultureHeader();
                    var serverUri = new Uri(Constants.ServerAddress);
                    handler.CookieContainer.SetCookies(serverUri, _accountService.ServerCookieHeader);
                    var response = await client.GetAsync(_clientBaseUrl);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsAsync<ReadOnlyCollection<PaymentMethod>>();
                }
            }
        }

        public async Task SavePaymentMethodAsync(PaymentMethod paymentMethod)
        {
            using (var handler = new HttpClientHandler { CookieContainer = new CookieContainer() })
            {
                using (var client = new HttpClient(handler))
                {
                    client.AddCurrentCultureHeader();
                    var serverUri = new Uri(Constants.ServerAddress);
                    handler.CookieContainer.SetCookies(serverUri, _accountService.ServerCookieHeader);
                    var response = await client.PostAsync(_clientBaseUrl, paymentMethod, new JsonMediaTypeFormatter());
                    await response.EnsureSuccessWithValidationSupportAsync();
                }
            }
        }

        public async Task SetDefault(string defaultPaymentMethodId)
        {
            using (var handler = new HttpClientHandler { CookieContainer = new CookieContainer() })
            {
                using (var client = new HttpClient(handler))
                {
                    client.AddCurrentCultureHeader();
                    var serverUri = new Uri(Constants.ServerAddress);
                    handler.CookieContainer.SetCookies(serverUri, _accountService.ServerCookieHeader);
                    var response = await client.PutAsync(_clientBaseUrl + "?defaultPaymentMethodId=" + defaultPaymentMethodId, null);
                    await response.EnsureSuccessWithValidationSupportAsync();
                }
            }
        }
    }
}

