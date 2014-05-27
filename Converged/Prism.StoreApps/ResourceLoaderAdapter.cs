// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Windows.ApplicationModel.Resources;

namespace Microsoft.Practices.Prism.StoreApps
{
    /// <summary>
    /// The ResourceLoader class abstracts the Windows.ApplicationModel.Resources.ResourceLoader object for use by apps that derive from the MvvmAppBase class.
    /// A ResourceLoader represents a class that reads the assembly resource file and looks for a named resource.
    /// This class simply passes method invocations to an underlying Windows.ApplicationModel.Resources.ResourceLoader object.
    /// </summary>
    public class ResourceLoaderAdapter : IResourceLoader
    {
        private readonly ResourceLoader _resourceLoader;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceLoaderAdapter"/> class.
        /// </summary>
        /// <param name="resourceLoader">The resource loader.</param>
        public ResourceLoaderAdapter(ResourceLoader resourceLoader)
        {
            _resourceLoader = resourceLoader;
        }

        /// <summary>
        /// Gets the value of the named resource.
        /// </summary>
        /// <param name="resource">The resource name.</param>
        /// <returns>
        /// The named resource value.
        /// </returns>
        public string GetString(string resource)
        {
            return _resourceLoader.GetString(resource);
        }
    }
}
