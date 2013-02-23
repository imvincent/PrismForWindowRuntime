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
using System.Threading.Tasks;

namespace Kona.UILogic.ViewModels
{
    public class ProductViewModel
    {
        private readonly Product _product;
        private readonly Uri _image;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public ProductViewModel(Product product) : this(product, null) { }

        public ProductViewModel(Product product, IShoppingCartRepository shoppingCartRepository)
        {
            _product = product;
            _shoppingCartRepository = shoppingCartRepository;
            _image = product.ImageUri;

            AddToCartCommand = DelegateCommand.FromAsyncHandler(AddToCart);
        }

        public string Title { get { return _product.Title; } }

        public string Description { get { return _product.Description; } }

        public string ProductNumber { get { return _product.ProductNumber; } }

        public Uri Image { get { return _image; } }

        public int ItemPosition { get; set; }
        
        public string SalePrice
        {
            get
            {
                var currencyFormatter = new CurrencyFormatter(_product.Currency);
                return currencyFormatter.FormatDouble(Math.Round(_product.ListPrice*(1-_product.DiscountPercentage/100), 2));
            }
        }

        public DelegateCommand AddToCartCommand { get; private set; }

        public async Task AddToCart()
        {
            await _shoppingCartRepository.AddProductToShoppingCartAsync(ProductNumber);
        }

        public override string ToString()
        {
            return _product.ProductNumber;
        }
    }
}
