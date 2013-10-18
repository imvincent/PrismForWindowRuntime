// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Globalization;
using AdventureWorks.UILogic.Models;
using Microsoft.Practices.Prism.StoreApps;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.Serialization;
using Microsoft.Practices.Prism.StoreApps.Interfaces;

namespace AdventureWorks.UILogic.ViewModels
{
    [DataContract]
    public class ShoppingCartItemViewModel : ViewModel
    {
        private readonly IResourceLoader _resourceLoader;
        private string _id;
        private string _title;
        private string _description;
        private int _quantity;
        private double _listPrice;
        private double _discountPercentage;
        private Uri _imageUri;
        private CurrencyFormatter _currencyFormatter;

        public ShoppingCartItemViewModel(ShoppingCartItem shoppingCartItem, IResourceLoader resourceLoader)
        {
            _resourceLoader = resourceLoader;
            if (shoppingCartItem == null)
            {
                throw new ArgumentNullException("shoppingCartItem", "shoppingCartItem cannot be null");
            }

            _id = shoppingCartItem.Id;
            _title = shoppingCartItem.Product.Title;
            _description = shoppingCartItem.Product.Description;
            _quantity = shoppingCartItem.Quantity;
            _listPrice = shoppingCartItem.Product.ListPrice;
            _discountPercentage = shoppingCartItem.Product.DiscountPercentage;
            _imageUri = shoppingCartItem.Product.ImageUri;
            ProductId = shoppingCartItem.Product.ProductNumber;
            _currencyFormatter = new CurrencyFormatter(shoppingCartItem.Currency);
        }

        public string ProductId { get; private set; }

        public string Id
        {
            get { return _id; }
        }
        
        public string Title
        {
            get { return _title; }
        }

        public string Description
        {
            get { return _description; }
        }

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (SetProperty(ref _quantity, value))
                {
                    OnPropertyChanged("FullPrice");
                    OnPropertyChanged("DiscountedPrice");
                    OnPropertyChanged("FullPriceDouble");
                    OnPropertyChanged("DiscountedPriceDouble");
                }
            }
        }

        public double FullPriceDouble { get { return Math.Round(Quantity*_listPrice, 2); } }

        public string FullPrice
        {
            get { return _currencyFormatter.FormatDouble(FullPriceDouble); }
        }

        public double DiscountPercentage
        {
            get { return _discountPercentage; }
        }

        public ImageSource Image
        {
            get { return new BitmapImage(_imageUri); }
        }

        public double DiscountedPriceDouble { get { return Math.Round(FullPriceDouble*(1 - DiscountPercentage/100), 2); } }

        public string DiscountedPrice 
        {
            get { return _currencyFormatter.FormatDouble(DiscountedPriceDouble); }
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}, {1}, {2}, {3} {4}, {5}", Title, Description, ProductId, _resourceLoader.GetString("Quantity"), Quantity, DiscountedPrice);
        }
    }
}
