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
using Microsoft.Practices.Prism.StoreApps;
using System.Globalization;

namespace AdventureWorks.UILogic.Services
{
    public class OrderServiceProxy : IOrderService
    {
        private string _clientBaseUrl = string.Format(CultureInfo.InvariantCulture, "{0}/api/Order/", Constants.ServerAddress);

        public async Task<int> CreateOrderAsync(Order order, string serverCookieHeader)
        {
            using (HttpClientHandler handler = new HttpClientHandler { CookieContainer = new CookieContainer() })
            {
                using (var orderClient = new HttpClient(handler))
                {
                    Uri serverUri = new Uri(Constants.ServerAddress);
                    handler.CookieContainer.SetCookies(serverUri, serverCookieHeader);
                    orderClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    // In order to meet the Windows 8 app certification requirements, 
                    // you cannot send Credit Card information to a Service in the clear.                    
                    // The payment processing must meet the current PCI Data Security Standard (PCI DSS).
                    // See http://go.microsoft.com/fwlink/?LinkID=288837
                    // and http://go.microsoft.com/fwlink/?LinkID=288839
                    // for more information.

                    // Replace sensitive information with dummy values
                    var orderToSend = new Order()
                        {
                            UserId = order.UserId,
                            ShippingMethod = order.ShippingMethod,
                            ShoppingCart = order.ShoppingCart,
                            BillingAddress = order.BillingAddress,
                            ShippingAddress = order.ShippingAddress,
                            PaymentMethod = new PaymentMethod()
                                {
                                    CardNumber = "**** **** **** ****",
                                    CardVerificationCode = "****",
                                    CardholderName = order.PaymentMethod.CardholderName,
                                    ExpirationMonth = order.PaymentMethod.ExpirationMonth,
                                    ExpirationYear = order.PaymentMethod.ExpirationYear,
                                    Phone = order.PaymentMethod.Phone
                                }
                        };

                    string requestUrl = _clientBaseUrl;
                    var response = await orderClient.PostAsJsonAsync<Order>(requestUrl, orderToSend);
                    await response.EnsureSuccessWithValidationSupportAsync();

                    return await response.Content.ReadAsAsync<int>();
                }
            }
        }

        public async Task ProcessOrderAsync(Order order, string serverCookieHeader)
        {
            using (HttpClientHandler handler = new HttpClientHandler { CookieContainer = new CookieContainer() })
            {
                using (var orderClient = new HttpClient(handler))
                {
                    Uri serverUri = new Uri(Constants.ServerAddress);
                    handler.CookieContainer.SetCookies(serverUri, serverCookieHeader);
                    orderClient.DefaultRequestHeaders.Add("Accept", "application/json");

                    // In order to meet the Windows 8 app certification requirements, 
                    // you cannot send Credit Card information to a Service in the clear.                    
                    // The payment processing must meet the current PCI Data Security Standard (PCI DSS).
                    // See http://go.microsoft.com/fwlink/?LinkID=288837
                    // and http://go.microsoft.com/fwlink/?LinkID=288839
                    // for more information.

                    // Replace sensitive information with dummy values
                    var orderToSend = new Order()
                    {
                        Id = order.Id,
                        UserId = order.UserId,
                        ShippingMethod = order.ShippingMethod,
                        ShoppingCart = order.ShoppingCart,
                        BillingAddress = order.BillingAddress,
                        ShippingAddress = order.ShippingAddress,
                        PaymentMethod = new PaymentMethod()
                        {
                            CardNumber = "**** **** **** ****",
                            CardVerificationCode = "****",
                            CardholderName = order.PaymentMethod.CardholderName,
                            ExpirationMonth = order.PaymentMethod.ExpirationMonth,
                            ExpirationYear = order.PaymentMethod.ExpirationYear,
                            Phone = order.PaymentMethod.Phone
                        }
                    };

                    string requestUrl = _clientBaseUrl + orderToSend.Id;
                    var response = await orderClient.PutAsJsonAsync<Order>(requestUrl, orderToSend);
                    await response.EnsureSuccessWithValidationSupportAsync();
                }
            }
        }
    }
}
