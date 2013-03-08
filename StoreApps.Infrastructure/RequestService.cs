// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Practices.StoreApps.Infrastructure.Interfaces;

namespace Microsoft.Practices.StoreApps.Infrastructure
{
    /// <summary>
    /// The RequestService is used to request an external resource and return it.
    /// This class implements the IRequestService interface, and creates an Http request for getting a resource from the especified Url and returns it.
    /// </summary>
    public class RequestService : IRequestService
    {
        // <snippet517>
        /// <summary>
        /// Gets the external resource asynchronously.
        /// </summary>
        /// <param name="resourceUrl">The resource URL.</param>
        /// <returns>A task of a byte array</returns>
        public async Task<byte[]> GetExternalResourceAsync(Uri resourceUrl)
        {
            using (HttpClient request = new HttpClient())
            {
                HttpResponseMessage response = await request.GetAsync(resourceUrl);
                return await response.Content.ReadAsByteArrayAsync();
            }
        }
        // </snippet517>
    }
}
