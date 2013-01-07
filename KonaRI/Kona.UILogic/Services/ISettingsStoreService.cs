// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;

namespace Kona.UILogic.Services
{
    public interface ISettingsStoreService
    {        
        void DeleteContainer(string container);

        List<T> RetrieveAllValues<T>(string container) where T: new();
        
        void SaveValue(string container, object entity);
        
        void DeleteValue(string container, string id);
        
        bool ContainsDefaultValue(string container);
        
        T GetDefaultValue<T>(string container) where T : new();
        
        void SetAsDefaultValue(string container, string id);
    }
}