// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Windows.ApplicationModel.Resources;

namespace Microsoft.Practices.Prism.Properties
{
    public static class Resources
    {
        private static ResourceLoader _resourceLoader = new ResourceLoader();

        public static string InvalidDelegateRerefenceTypeException
        {
            get { return _resourceLoader.GetString("InvalidDelegateReferenceTypeException"); }
        }
    }
}
