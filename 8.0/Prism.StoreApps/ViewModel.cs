// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Practices.Prism.StoreApps
{
    /// <summary>
    /// This is the view model base class that includes INotifyPropertyChange support and is aware of navigation events.
    /// </summary>
    // Documentation on using the MVVM pattern is at http://go.microsoft.com/fwlink/?LinkID=288814&clcid=0x409
    public class ViewModel : BindableBase, INavigationAware
    {
        /// <summary>
        /// Called when navigation is performed to a page. You can use this method to load state if it is available.
        /// </summary>
        /// <param name="navigationParameter">The navigation parameter.</param>
        /// <param name="navigationMode">The navigation mode.</param>
        /// <param name="viewModelState">The state of the view model.</param>
        public virtual void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            if (viewModelState != null)
            {
                RestoreViewModel(viewModelState, this);
            }
        }

        /// <summary>
        /// This method will be called when navigating away from a page. You can use this method to save your view model data in case of a suspension event.
        /// </summary>
        /// <param name="viewModelState">The state of the view model.</param>
        /// <param name="suspending">if set to <c>true</c> you are navigating away from this viewmodel due to a suspension event.</param>
        public virtual void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            if (viewModelState != null)
            {
                FillStateDictionary(viewModelState, this);
            }
        }

        /// <summary>
        /// Retrieves the entity state value of the specified entity state key.
        /// </summary>
        /// <typeparam name="T">Type of the expected return value</typeparam>
        /// <param name="entityStateKey">The entity state key.</param>
        /// <param name="viewModelState">State of the view model.</param>
        /// <returns>The T type object that represents the state value of the specified entity.</returns>
        static public T RetrieveEntityStateValue<T>(string entityStateKey, IDictionary<string, object> viewModelState)
        {
            if (viewModelState != null && viewModelState.ContainsKey(entityStateKey))
            {
                return (T)viewModelState[entityStateKey];
            }

            return default(T);
        }

        /// <summary>
        /// Adds an entity state value to the view model state dictionary.
        /// </summary>
        /// <param name="viewModelStateKey">The view model state key.</param>
        /// <param name="viewModelStateValue">The view model state value.</param>
        /// <param name="viewModelState">The view model state dictionary.</param>
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
