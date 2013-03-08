// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Microsoft.Practices.StoreApps.Infrastructure
{
    /// <summary>
    /// This converter converts a boolean value to a visibility a value. If true it will return a Visibility.Visible value, otherwise;
    /// it will return a Visibility.Collapsed value.
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean to a Visibility object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns>A visibilty object.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool && targetType == typeof(Visibility))
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;
            else
                return value;
        }

        /// <summary>
        /// Reverse conversion. Converts a visibility object to a boolean value. As this is not required, this method is not implemented. 
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
