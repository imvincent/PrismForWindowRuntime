// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.ObjectModel;
using System.Linq;
using Kona.Infrastructure;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ValidationQuickStart.Behaviors
{
    public static class HighlightOnErrors
    {
        // <snippet1304> 
        public static readonly DependencyProperty PropertyErrorsProperty =
            DependencyProperty.RegisterAttached("PropertyErrors", typeof(ReadOnlyCollection<string>), typeof(HighlightOnErrors),
                                                                   new PropertyMetadata(EntityValidator.EmptyErrorsCollection, OnPropertyErrorsChanged));
        // </snippet1304> 

        public static ReadOnlyCollection<string> GetPropertyErrors(DependencyObject obj)
        {
            if (obj == null)
            {
                return null;
            }

            return (ReadOnlyCollection<string>)obj.GetValue(PropertyErrorsProperty);
        }

        public static void SetPropertyErrors(DependencyObject obj, ReadOnlyCollection<string> value)
        {
            if (obj == null)
            {
                return;
            }

            obj.SetValue(PropertyErrorsProperty, value);
        }

        // <snippet1305>
        private static void OnPropertyErrorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            if (args == null || args.NewValue == null)
            {
                return;
            }

            TextBox textBox = (TextBox)d;
            var propertyErrors = (ReadOnlyCollection<string>)args.NewValue;

            Style textBoxStyle = (propertyErrors.Count() > 0) ? (Style)Application.Current.Resources["HighlightTextStyle"] : null;

            textBox.Style = textBoxStyle;
        }
        // </snippet1305>
    }
}
