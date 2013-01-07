// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.Reflection;
using Windows.Storage;

namespace Kona.UILogic.Services
{
    public class SettingsStoreService : ISettingsStoreService
    {
        private ApplicationDataContainer _settingsContainer;

        public SettingsStoreService()
        {
            _settingsContainer = ApplicationData.Current.LocalSettings;
        }

        public void DeleteContainer(string container)
        {
            _settingsContainer.DeleteContainer(container);
        }

        public List<T> RetrieveAllValues<T>(string container) where T: new()
        {
            var selectedContainer = _settingsContainer.CreateContainer(container, ApplicationDataCreateDisposition.Always);
            var values = new List<T>();

            var defaultValueId = selectedContainer.Values["default"] as string;
            if (!string.IsNullOrEmpty(defaultValueId))
            {
                values.Add(PopulateEntity<T>(selectedContainer.Values[defaultValueId] as ApplicationDataCompositeValue));
            }

            foreach (var compositeValue in selectedContainer.Values)
            {
                if (compositeValue.Key != "default" && compositeValue.Key != defaultValueId)
                {
                    var entity = PopulateEntity<T>((ApplicationDataCompositeValue)compositeValue.Value);
                    values.Add(entity);
                }
            }
            return values;
        }
        
        public void SaveValue(string container, object entity)
        {
            if (entity == null)
            {
                return;
            }

            ApplicationDataCompositeValue compositeValue = GetCompositeValue(entity);

            var selectedContainer = _settingsContainer.CreateContainer(container, ApplicationDataCreateDisposition.Always);
            selectedContainer.Values[compositeValue["Id"].ToString()] = compositeValue;
        }

        public void DeleteValue(string container, string id)
        {
            var selectedContainer = _settingsContainer.CreateContainer(container, ApplicationDataCreateDisposition.Always);
            selectedContainer.Values.Remove(id);
        }

        public bool ContainsDefaultValue(string container)
        {
            var selectedContainer = _settingsContainer.CreateContainer(container, ApplicationDataCreateDisposition.Always);
            return selectedContainer.Values.ContainsKey("default");
        }

        public T GetDefaultValue<T>(string container) where T : new()
        {
            var selectedContainer = _settingsContainer.CreateContainer(container, ApplicationDataCreateDisposition.Always);
            var defaultValueId = selectedContainer.Values["default"] as string;
            var defaultValue = selectedContainer.Values[defaultValueId];
            if (defaultValue != null)
            {
                return PopulateEntity<T>(defaultValue as ApplicationDataCompositeValue);
            }

            return new T();
        }

        public void SetAsDefaultValue(string container, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return;
            }

            var selectedContainer = _settingsContainer.CreateContainer(container, ApplicationDataCreateDisposition.Always);
            selectedContainer.Values["default"] = id;
        }

        private T PopulateEntity<T>(ApplicationDataCompositeValue compositeValue) where T : new()
        {
            var entity = new T();

            if (entity != null)
            {
                foreach (var keyValue in compositeValue)
                {
                    entity.GetType().GetRuntimeProperty(keyValue.Key).SetValue(entity, keyValue.Value);
                }
            }

            return entity;
        }

        private ApplicationDataCompositeValue GetCompositeValue(object entity)
        {
            var compositeValue = new ApplicationDataCompositeValue();
            foreach (var property in entity.GetType().GetRuntimeProperties())
            {
                compositeValue[property.Name] = entity.GetType().GetRuntimeProperty(property.Name).GetValue(entity);
            }

            return compositeValue;
        }
    }
}