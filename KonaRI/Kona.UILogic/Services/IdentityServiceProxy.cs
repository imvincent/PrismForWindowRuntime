// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Net.Http;
using System.Security;
using System.Threading.Tasks;
using Kona.UILogic.Models;
using System.Net;
using System;

namespace Kona.UILogic.Services
{
    public class IdentityServiceProxy : IIdentityService
    {
        private readonly string _clientBaseUrl = string.Format("{0}/api/Identity/", Constants.ServerAddress);

        // <snippet508>
        public async Task<LogOnResult> LogOnAsync(string userId, string password)
        {
            using (var handler = new HttpClientHandler { CookieContainer = new CookieContainer() })
            {
                using (var client = new HttpClient(handler))
                {
                    //The password is intentionally not sent to the IdentityController to simplify the deployment of this app.
                    var response = await client.GetAsync(_clientBaseUrl + userId + "?password=");
                    response.EnsureSuccessStatusCode();
                    var result = await response.Content.ReadAsAsync<UserValidationResult>();
                    var serverUri = new Uri(Constants.ServerAddress);
                    return new LogOnResult { ServerCookieHeader = handler.CookieContainer.GetCookieHeader(serverUri), UserValidationResult = result };
                }
            }
        }
        // </snippet508>

        public async Task<bool> VerifyActiveSession(string userId, string serverCookieHeader)
        {
            using (var handler = new HttpClientHandler { CookieContainer = new CookieContainer() })
            {
                using (var client = new HttpClient(handler))
                {
                    var serverUri = new Uri(Constants.ServerAddress);
                    handler.CookieContainer.SetCookies(serverUri, serverCookieHeader);
                    var response = await client.GetAsync(_clientBaseUrl + "GetIsValidSession");
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                        throw new SecurityException();
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsAsync<bool>();
                }
            }
        }
    }
}
