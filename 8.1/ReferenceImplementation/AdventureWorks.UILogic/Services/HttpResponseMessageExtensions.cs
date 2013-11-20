// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using AdventureWorks.UILogic.Models;
using Newtonsoft.Json;
using System;
using System.Security;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace AdventureWorks.UILogic.Services
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task EnsureSuccessWithValidationSupportAsync(this HttpResponseMessage response)
        {
            // If BadRequest, see if it contains a validation payload
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                ModelValidationResult result = null;
                try
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ModelValidationResult>(responseContent);
                }
                catch { } // Fall through logic will take care of it
                if (result != null) throw new ModelValidationException(result);

            }
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new SecurityException();

            response.EnsureSuccessStatusCode(); // Will throw for any other service call errors
        }
    }
}
