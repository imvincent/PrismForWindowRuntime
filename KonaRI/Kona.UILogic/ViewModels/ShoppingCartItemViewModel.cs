// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Windows.Input;
using Kona.Infrastructure;
using Kona.UILogic.Models;
using Windows.Globalization.NumberFormatting;

namespace Kona.UILogic.ViewModels
{
    public class ShoppingCartItemViewModel : ViewModel
    {
        private Guid _id;
        private string _title;
        private string _description;
        private bool _isGift;
        private int _quantity;
        private string _totalPrice;
        private string _discountPrice;
        private double _discountPercentage;
        private Uri _image;
        
        public Guid Id
        {
            get { return _id; }
            set { this.SetProperty(ref _id, value); }
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

        [RestorableState]
        public bool IsGift
        {
            get { return _isGift; }
            set { SetProperty(ref _isGift, value); }
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

        public Uri Image 
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
        }

        public string DiscountedPrice 
        {
            get { return _discountPrice; }
            set { SetProperty(ref _discountPrice, value); }
        }

        public ICommand EditAmountCommand { get; private set; }

        public ShoppingCartItemViewModel()
        {
            this.EditAmountCommand = new DelegateCommand(EditAmount);
        }

        public ShoppingCartItemViewModel(ShoppingCartItem shoppingCartItem)
            : this()
        {
            if (shoppingCartItem == null)
            {
                throw new ArgumentNullException("shoppingCartItem", "shoppingCartItem cannot be null");
            }

            Id = shoppingCartItem.Id;
            Title = shoppingCartItem.Product.Title;
            Description = shoppingCartItem.Product.Description;
            IsGift = shoppingCartItem.IsGift;
            Quantity = shoppingCartItem.Quantity;
            DiscountPercentage = shoppingCartItem.DiscountPercentage;
            Image = new Uri(shoppingCartItem.Product.ImageName, UriKind.Relative);
            EntityId = shoppingCartItem.Id.ToString();

            var totalPrice = shoppingCartItem.Quantity * shoppingCartItem.Product.ListPrice * (100 - shoppingCartItem.DiscountPercentage) / 100;
            var discountedPrice = totalPrice * (1 - (shoppingCartItem.DiscountPercentage / 100));

            var currencyFormatter = new CurrencyFormatter(shoppingCartItem.Currency);
            TotalPrice = currencyFormatter.FormatDouble(totalPrice);
            DiscountedPrice = currencyFormatter.FormatDouble(discountedPrice);
        }

        private void EditAmount()
        {
            // edit amount
        }
    }
}
