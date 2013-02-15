// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.ViewModels;

namespace Kona.AWShopper.DesignViewModels
{
    public class SearchResultsPageDesignViewModel
    {
        public SearchResultsPageDesignViewModel()
        {
            this.QueryText = '\u201c' + "bike" + '\u201d';
            this.SearchTerm = "bike";
            FillWithDummyData();
        }

        public string QueryText { get; set; }

        public string SearchTerm { get; set; }

        public List<CategoryViewModel> Results { get; set; }

        public void FillWithDummyData()
        {
            Results = new List<CategoryViewModel>()
                {
                    new CategoryViewModel(new Category()
                    { 
                        Title = "Category 1", 
                        Products = new List<Product>()
                            {
                                new Product() {  Title = "Bike 1",  Description = "Description of Product 1", ListPrice = 25.10, DiscountPercentage = 10, ProductNumber = "1", ImageName = (new Uri("ms-appx:///Assets/StoreLogo.png")).AbsoluteUri, Currency = "USD"},
                                new Product() {  Title = "Bike 2",  Description = "Description of Product 2", ListPrice = 25.10, DiscountPercentage = 10, ProductNumber = "2", ImageName = (new Uri("ms-appx:///Assets/StoreLogo.png")).AbsoluteUri, Currency = "USD" },
                                new Product() {  Title = "Bike 3",  Description = "Description of Product 3", ListPrice = 25.10, DiscountPercentage = 10, ProductNumber = "3", ImageName = (new Uri("ms-appx:///Assets/StoreLogo.png")).AbsoluteUri, Currency = "USD" },
                            }
                    }, null),
                    new CategoryViewModel(new Category()
                    { 
                        Title = "Category 2", 
                        Products = new List<Product>()
                            {
                                new Product() {  Title = "Bike Lock",  Description = "Description of Product 1", ListPrice = 25.10, DiscountPercentage = 10, ProductNumber = "1", ImageName = (new Uri("ms-appx:///Assets/StoreLogo.png")).AbsoluteUri, Currency = "USD" },
                                new Product() {  Title = "Red Mountain Bike with light blue inclusions in the frame.",  Description = "Description of Product 2", ListPrice = 25.10, DiscountPercentage = 10, ProductNumber = "2", ImageName = (new Uri("ms-appx:///Assets/StoreLogo.png")).AbsoluteUri, Currency = "USD" },
                            }
                    }, null),
                    new CategoryViewModel(new Category()
                    { 
                        Title = "Category 3", 
                        Products = new List<Product>()
                            {
                                new Product() {  Title = "Blue Bike Cover",  Description = "Description of Product 1", ListPrice = 25.10, DiscountPercentage = 10, ProductNumber = "1", ImageName = (new Uri("ms-appx:///Assets/StoreLogo.png")).AbsoluteUri, Currency = "USD" },
                            }
                    }, null)
                };
        }

    }
}
