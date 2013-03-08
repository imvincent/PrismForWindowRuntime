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
using Microsoft.Practices.StoreApps.Infrastructure.Interfaces;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Practices.StoreApps.Infrastructure
{
    public class ViewModel : BindableBase, INavigationAware
    {
        // <snippet706>
        public virtual void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            if (viewModelState != null)
            {
                RestoreViewModel(viewModelState, this);
            }
        }
        // </snippet706>

        // <snippet703>
        public virtual void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            if (viewModelState != null)
            {
                FillStateDictionary(viewModelState, this);
            }
        }
        // </snippet703>

        public static T RetrieveEntityStateValue<T>(string entityStateKey, IDictionary<string, object> viewModelState)
        {
            if (viewModelState != null && viewModelState.ContainsKey(entityStateKey))
            {
                return (T)viewModelState[entityStateKey];
            }

            return default(T);
        }

        public static void AddEntityStateValue(string viewModelStateKey, object viewModelStateValue, IDictionary<string, object> viewModelState)
        {
            if (viewModelState != null)
            {
                viewModelState[viewModelStateKey] = viewModelStateValue;
            }
        }

        private static void FillStateDictionary(IDictionary<string, object> viewModelState, object viewModel)
        {
            var viewModelProperties = viewModel.GetType().GetRuntimeProperties().Where(
                                                            c => c.GetCustomAttribute(typeof(RestorableStateAttribute)) != null);

            foreach (PropertyInfo propertyInfo in viewModelProperties)
            {
                viewModelState[propertyInfo.Name] = propertyInfo.GetValue(viewModel);
            }
        }

        private static void RestoreViewModel(IDictionary<string, object> viewModelState, object viewModel)
        {
            var viewModelProperties = viewModel.GetType().GetRuntimeProperties().Where(
                                                            c => c.GetCustomAttribute(typeof(RestorableStateAttribute)) != null);

            foreach (PropertyInfo propertyInfo in viewModelProperties)
            {
                if (viewModelState.ContainsKey(propertyInfo.Name))
                {
                    propertyInfo.SetValue(viewModel, viewModelState[propertyInfo.Name]);
                }
            }
        }
    }
}
