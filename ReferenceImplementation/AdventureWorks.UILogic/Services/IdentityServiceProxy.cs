// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Globalization;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using System.Net;
using System;
using Microsoft.Practices.StoreApps.Infrastructure;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace AdventureWorks.UILogic.Services
{
    public class IdentityServiceProxy : IIdentityService
    {
        private readonly string _clientBaseUrl = string.Format(CultureInfo.InvariantCulture, "{0}/api/Identity/", Constants.ServerAddress);

        // <snippet508>
        public async Task<LogOnResult> LogOnAsync(string userId, string password)
        {
            using (var handler = new HttpClientHandler { CookieContainer = new CookieContainer() })
            {
                using (var client = new HttpClient(handler))
                {
                    client.AddCurrentCultureHeader();

                    // Ask the server for a password challenge string
                    var requestId = CryptographicBuffer.EncodeToHexString(CryptographicBuffer.GenerateRandom(4));
                    var challengeResponse = await client.GetAsync(_clientBaseUrl + "GetPasswordChallenge?requestId=" + requestId);
                    challengeResponse.EnsureSuccessStatusCode();
                    var challengeEncoded = await challengeResponse.Content.ReadAsAsync<string>();
                    var challengeBuffer = CryptographicBuffer.DecodeFromHexString(challengeEncoded);

                    // Use HMAC_SHA512 hash to encode the challenge string using the password being authenticated as the key.
                    var provider = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha512);
                    var passwordBuffer = CryptographicBuffer.ConvertStringToBinary(password, BinaryStringEncoding.Utf8);
                    var hmacKey = provider.CreateKey(passwordBuffer);
                    var buffHmac = CryptographicEngine.Sign(hmacKey, challengeBuffer);
                    var hmacString = CryptographicBuffer.EncodeToHexString(buffHmac);

                    // Send the encoded challenge to the server for authentication (to avoid sending the password itself)
                    var response = await client.GetAsync(_clientBaseUrl + userId + "?requestID=" + requestId +"&passwordHash=" + hmacString);

                    // Raise exception if sign in failed
                    response.EnsureSuccessStatusCode();

                    // On success, return sign in results from the server response packet
                    var result = await response.Content.ReadAsAsync<UserInfo>();
                    var serverUri = new Uri(Constants.ServerAddress);
                    return new LogOnResult { ServerCookieHeader = handler.CookieContainer.GetCookieHeader(serverUri), UserInfo = result };
                }
            }
        }
        // </snippet508>

        public async Task<bool> VerifyActiveSessionAsync(string userId, string serverCookieHeader)
        {
            using (var handler = new HttpClientHandler { CookieContainer = new CookieContainer() })
            {
                using (var client = new HttpClient(handler))
                {
                    client.AddCurrentCultureHeader();
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
