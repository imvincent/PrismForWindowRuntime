// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Threading.Tasks;

namespace Microsoft.Practices.StoreApps.Infrastructure.Interfaces
{
    /// <summary>
    /// The IRequestService interface is used for classes that can request an external resource and return it.
    /// The default implementation of IRequestService is the RequestService class,
    /// which creates an Http request for getting a resource from the especified Url and returns it.
    /// </summary>
    public interface IRequestService
    {
        /// <summary>
        /// Gets the external resource asynchronously.
        /// </summary>
        /// <param name="resourceUrl">The resource URL.</param>
        /// <returns>A task of a byte array</returns>
        Task<byte[]> GetExternalResourceAsync(Uri resourceUrl);
    }
}
