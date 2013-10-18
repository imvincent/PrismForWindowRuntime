// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Globalization;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Repositories;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Windows.Globalization.NumberFormatting;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Services;

namespace AdventureWorks.UILogic.ViewModels
{
    public class ProductViewModel
    {
        private readonly Product _product;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IAlertMessageService _alertMessageService;
        private readonly IResourceLoader _resourceLoader;

        public ProductViewModel(Product product) : this(product, null, null, null) { }

        public ProductViewModel(Product product, IShoppingCartRepository shoppingCartRepository, IAlertMessageService alertMessageService,
                                IResourceLoader resourceLoader)
        {
            if (product == null) throw new ArgumentNullException("product");
            _product = product;
            _shoppingCartRepository = shoppingCartRepository;
            _alertMessageService = alertMessageService;
            _resourceLoader = resourceLoader;

            AddToCartCommand = DelegateCommand.FromAsyncHandler(AddToCart);
        }

        public string Title { get { return _product.Title; } }

        public string Description { get { return _product.Description; } }

        public string ProductNumber { get { return _product.ProductNumber; } }

        public Uri Image { get { return _product.ImageUri; } }

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
            string errorMessage = string.Empty;
            try
            {
                await _shoppingCartRepository.AddProductToShoppingCartAsync(ProductNumber);
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                await _alertMessageService.ShowAsync(_resourceLoader.GetString("ErrorServiceUnreachable"), _resourceLoader.GetString("Error"));
            }
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}, {1}, {2}, {3}", Title, Description, ProductNumber, SalePrice);
        }
    }
}
