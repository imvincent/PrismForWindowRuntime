// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using Kona.Infrastructure;
using Kona.UILogic.Models;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Kona.UILogic.ViewModels
{
    public class ShoppingCartItemViewModel : ViewModel
    {
        private int _id;
        private string _title;
        private string _description;
        private int _quantity;
        private string _totalPrice;
        private string _discountPrice;
        private double _discountPercentage;
        private string _imagePath;

        public ShoppingCartItemViewModel(ShoppingCartItem shoppingCartItem)
        {
            if (shoppingCartItem == null)
            {
                throw new ArgumentNullException("shoppingCartItem", "shoppingCartItem cannot be null");
            }

            Id = shoppingCartItem.Id;
            Title = shoppingCartItem.Product.Title;
            Description = shoppingCartItem.Product.Description;
            Quantity = shoppingCartItem.Quantity;
            DiscountPercentage = shoppingCartItem.DiscountPercentage;
            _imagePath = shoppingCartItem.Product.ImageName;
            EntityId = shoppingCartItem.Id.ToString();

            var totalPrice = Math.Round(shoppingCartItem.Quantity * shoppingCartItem.Product.ListPrice * (100 - shoppingCartItem.DiscountPercentage) / 100, 2);
            var discountedPrice = Math.Round(totalPrice * (1 - (shoppingCartItem.DiscountPercentage / 100)),2);

            var currencyFormatter = new CurrencyFormatter(shoppingCartItem.Currency);
            TotalPrice = currencyFormatter.FormatDouble(totalPrice);
            DiscountedPrice = currencyFormatter.FormatDouble(discountedPrice);
        }

        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }
        
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public int Quantity
        {
            get { return _quantity; }
            set { SetProperty(ref _quantity, value); }
        }
        
        public string TotalPrice
        {
            get { return _totalPrice; }
            set { SetProperty(ref _totalPrice, value); }
        }

        public double DiscountPercentage
        {
            get { return _discountPercentage; }
            set { SetProperty(ref _discountPercentage, value); }
        }

        public ImageSource Image
        {
            get { return new BitmapImage(new Uri(_imagePath, UriKind.Absolute)); }
        }

        public string DiscountedPrice 
        {
            get { return _discountPrice; }
            set { SetProperty(ref _discountPrice, value); }
        }
    }
}
