// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Kona.Infrastructure;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Windows.Globalization.NumberFormatting;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Kona.UILogic.ViewModels
{
    public class ProductViewModel : BindableBase
    {
        private readonly Product _product;
        private ImageSource _image;
        private IShoppingCartRepository _shoppingCartRepository;
        private string _selectedColor;
        private string _selectedSize;

        public ProductViewModel(Product product)
        {
            _product = product;
        }

        public ProductViewModel(Product product, IShoppingCartRepository shoppingCartRepository)
            : this(product)
        {
            _shoppingCartRepository = shoppingCartRepository;
            AddToCartCommand = new DelegateCommand(AddToCart);
            PopulateColorsAndSizes();
            SelectedColor = "";
            SelectedSize = "";
        }

        public string SelectedColor
        {
            get { return _selectedColor; }
            set { SetProperty(ref _selectedColor, value); }
        }
        public ReadOnlyCollection<ComboBoxItemValue> Colors { get; set; }
        public ReadOnlyCollection<ComboBoxItemValue> Sizes { get; set; }
        public string SelectedSize
        {
            get { return _selectedSize; }
            set { SetProperty(ref _selectedSize, value); }
        }
        public string Title { get { return _product.Title; } }
        public string Description { get { return _product.Description; } }
        public string ProductNumber { get { return _product.ProductNumber; } }
        public int ItemPosition { get; set; }
        public DelegateCommand AddToCartCommand { get; private set; }
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

        public void PopulateColorsAndSizes()
        {
            var colors = new List<ComboBoxItemValue> { new ComboBoxItemValue() { Id = string.Empty, Value = "Select a Color" },
                new ComboBoxItemValue() { Id = "Red", Value = "Red" },
                new ComboBoxItemValue() { Id = "Blue", Value = "Blue" },
                new ComboBoxItemValue() { Id = "White", Value = "White" },
                new ComboBoxItemValue() { Id = "Black", Value = "Black" }
            };

            var sizes = new List<ComboBoxItemValue> { new ComboBoxItemValue() { Id = string.Empty, Value = "Select a Size" },
                new ComboBoxItemValue() { Id = "Small", Value = "Small" },
                new ComboBoxItemValue() { Id = "Medium", Value = "Medium" },
                new ComboBoxItemValue() { Id = "Large", Value = "Large" }
            };

            Colors = new ReadOnlyCollection<ComboBoxItemValue>(colors);
            Sizes = new ReadOnlyCollection<ComboBoxItemValue>(sizes);
        }

        public void AddToCart()
        {
            _shoppingCartRepository.AddProductToShoppingCartAsync(ProductNumber);
        }

        public override string ToString()
        {
            return _product.ProductNumber;
        }
    }
}
