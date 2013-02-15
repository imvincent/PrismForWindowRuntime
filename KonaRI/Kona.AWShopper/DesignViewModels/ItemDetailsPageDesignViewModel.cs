// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.UILogic.Models;
using Kona.UILogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Kona.AWShopper.DesignViewModels
{
    public class ItemDetailsPageDesignViewModel
    {

        public ItemDetailsPageDesignViewModel()
        {
            FillWithDummyData();
        }

        public ProductViewModel SelectedProduct { get; set; }

        public ObservableCollection<ProductViewModel> Items { get; private set; }

        private void FillWithDummyData()
        {
            Items = new ObservableCollection<ProductViewModel>(new List<ProductViewModel> {
            new ProductViewModel(
                new Product() { Title = "Product 1",  Description = "Description of Product 1", ListPrice = 99.99, DiscountPercentage = 0, ProductNumber = "1", ImageName = (new Uri("ms-appx:///Assets/StoreLogo.png")).AbsoluteUri, Currency= "USD" }
                )
            });

            SelectedProduct = Items[0];
        }
    }
}