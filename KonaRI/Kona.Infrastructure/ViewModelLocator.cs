// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Windows.UI.Xaml;

namespace Kona.Infrastructure
{
    public static class ViewModelLocator
    {
        static Dictionary<Type, Func<BindableBase>> factories = new Dictionary<Type, Func<BindableBase>>();
        private static Func<Type, ViewModel> defaultViewModelFactory = type => Activator.CreateInstance(type) as ViewModel;
        
        //Default View Type to VM Type resolver assumes VM is in same assembly and namespace as View Type.
        private static Func<Type, Type> defaultViewTypeToViewModelTypeResolver= 
            viewType =>
            {
                var viewName = viewType.FullName;
                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                var viewModelName = String.Format(CultureInfo.InvariantCulture, "{0}ViewModel, {1}", viewName, viewAssemblyName);
                return Type.GetType(viewModelName);
            };

        public static void SetDefaultViewModelFactory(Func<Type, ViewModel> viewModelFactory)
        {
            defaultViewModelFactory = viewModelFactory;
        }

        public static void SetDefaultViewTypeToViewModelTypeResolver(Func<Type, Type> viewTypeToViewModelTypeResolver)
        {
            defaultViewTypeToViewModelTypeResolver = viewTypeToViewModelTypeResolver;
        }

        #region Attached property with convention-or-mapping based approach

        public static readonly DependencyProperty AutoWireViewModelProperty =
            DependencyProperty.RegisterAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), 
            new PropertyMetadata(false, AutoWireViewModelChanged));

        public static bool GetAutoWireViewModel(DependencyObject obj)
        {
            if (obj != null)
            {
                return (bool) obj.GetValue(AutoWireViewModelProperty);
            }
            return false;
        }

        public static void SetAutoWireViewModel(DependencyObject obj, bool value)
        {
            if (obj != null)
            {
                obj.SetValue(AutoWireViewModelProperty, value);
            }
        }

        #endregion

        private static void AutoWireViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement view = d as FrameworkElement;
            if (view == null) return; // Incorrect hookup, do no harm

            // Try mappings first
            object viewModel = GetViewModelForView(view);
            // Fallback to convention based
            if (viewModel == null)
            {
                var viewModelType = defaultViewTypeToViewModelTypeResolver(view.GetType());
                if (viewModelType == null) return;

                // Really need Container or Factories here to deal with injecting dependencies on construction
                viewModel = defaultViewModelFactory(viewModelType);
            }
            view.DataContext = viewModel;
        }

        private static object GetViewModelForView(FrameworkElement view)
        {
            // Mapping of view models base on view type (or instance) goes here
            if (factories.ContainsKey(view.GetType()))
                return factories[view.GetType()]();
            return null;
        }

        public static void Register(Type viewType, Func<BindableBase> factory)
        {
            factories[viewType] = factory;
        }
    }
}
