// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using Kona.UILogic.Models;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Kona.UILogic.ViewModels
{
    public class ProductViewModel
    {
        private readonly Product _product;
        private ImageSource _image;

        public ProductViewModel(Product product)
        {
            _product = product;
        }

        public string Title { get { return _product.Title; } }
        public string Description { get { return _product.Description; } }
        public string ProductNumber { get { return _product.ProductNumber; } }
        public string ListPrice
        {
            get
            {
                var currencyFormatter = new CurrencyFormatter(_product.Currency);
                return currencyFormatter.FormatDouble(_product.ListPrice);
            }
        }

        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._product.ImageName != null)
                {
                    this._image = new BitmapImage(new Uri(this._product.ImageName, UriKind.Absolute));
                }
                return this._image;
            }
        }
    }
}
