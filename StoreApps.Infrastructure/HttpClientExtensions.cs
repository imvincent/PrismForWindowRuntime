// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Microsoft.Practices.StoreApps.Infrastructure
{
    /// <summary>
    /// This extension class adds a method to an HttpClient object for adding the current culture to the request header. 
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Adds the current culture header to the HttpClient request.
        /// </summary>
        /// <param name="client">The client.</param>
        public static void AddCurrentCultureHeader(this HttpClient client)
        {
            if (client != null)
            {
                // Setting the user's current language in the Accept-Language header.
                // This doc http://msdn.microsoft.com/en-us/library/windows/apps/xaml/Hh967761(v=win.10).aspx 
                // states that by default, the HTTP header is set to the user's language preferences.
                // However the default behavior described does not seem to be implemented.
                client.DefaultRequestHeaders.AcceptLanguage.Add(
                    new StringWithQualityHeaderValue(CultureInfo.CurrentUICulture.Name));
            }
        }
    }
}
