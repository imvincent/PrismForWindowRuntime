// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kona.UILogic.Services;

namespace Kona.UILogic.Tests.Mocks
{
    public class MockSettingsStoreService : ISettingsStoreService
    {

        public Action<string> DeleteContainerDelegate { get; set; }
        public Func<string, List<object>> RetrieveAllValuesDelegate { get; set; }
        private readonly Dictionary<string, object> _defaultValues = new Dictionary<string, object>();

        public void DeleteContainer(string container)
        {
            DeleteContainerDelegate(container);
        }

        public List<T> RetrieveAllValues<T>(string container) where T : new()
        {
            var values = RetrieveAllValuesDelegate(container);
            return values.Select(value => (T) value).ToList();
        }

        public void SaveValue(string container, object entity)
        {
        }

        public void DeleteValue(string container, string id)
        {
        }

        public bool ContainsDefaultValue(string container)
        {
            return _defaultValues.ContainsKey(container);
        }

        public T GetDefaultValue<T>(string container) where T : new()
        {
            if (!_defaultValues.ContainsKey(container)) return default(T);
            var id = _defaultValues[container];
            var defaultValue = RetrieveAllValues<T>(container).FirstOrDefault(entity => entity.GetType().GetRuntimeProperty("Id").GetValue(entity) == id);
            return defaultValue;
        }

        public void SetAsDefaultValue(string container, string id)
        {
            _defaultValues[container] = id;
        }
    }
}
