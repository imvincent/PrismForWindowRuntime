// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
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
    public class OrderServiceProxy : IOrderService
    {
        private string _clientBaseUrl = string.Format("{0}/api/Order/", Constants.ServerAddress);

        public async Task<OrderSubmissionResult> CreateOrderAsync(Order order, string serverCookieHeader)
        {
            using (HttpClientHandler handler = new HttpClientHandler { CookieContainer = new CookieContainer() })
            {
                using (var orderClient = new HttpClient(handler))
                {
                    Uri serverUri = new Uri(Constants.ServerAddress);
                    handler.CookieContainer.SetCookies(serverUri, serverCookieHeader);
                    orderClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    string requestUrl = _clientBaseUrl + "create";
                    var response = await orderClient.PostAsJsonAsync<Order>(requestUrl, order);
                    
                    return await response.Content.ReadAsAsync<OrderSubmissionResult>();
                }
            }
        }

        public async Task<OrderSubmissionResult> UpdateOrderAsync(Order order, string serverCookieHeader)
        {
            using (HttpClientHandler handler = new HttpClientHandler { CookieContainer = new CookieContainer() })
            {
                using (var orderClient = new HttpClient(handler))
                {
                    Uri serverUri = new Uri(Constants.ServerAddress);
                    handler.CookieContainer.SetCookies(serverUri, serverCookieHeader);
                    orderClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    string requestUrl = _clientBaseUrl + "update" + order.Id;
                    var response = await orderClient.PutAsJsonAsync<Order>(requestUrl, order);
                    
                    return await response.Content.ReadAsAsync<OrderSubmissionResult>();
                }
            }
        }

        public async Task<OrderSubmissionResult> ProcessOrderAsync(Order order, string serverCookieHeader)
        {
            using (HttpClientHandler handler = new HttpClientHandler { CookieContainer = new CookieContainer() })
            {
                using (var orderClient = new HttpClient(handler))
                {
                    Uri serverUri = new Uri(Constants.ServerAddress);
                    handler.CookieContainer.SetCookies(serverUri, serverCookieHeader);
                    orderClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    string requestUrl = _clientBaseUrl + "process";
                    var response = await orderClient.PostAsJsonAsync<Order>(requestUrl, order);
                    
                    return await response.Content.ReadAsAsync<OrderSubmissionResult>();
                }
            }
        }
    }
}
