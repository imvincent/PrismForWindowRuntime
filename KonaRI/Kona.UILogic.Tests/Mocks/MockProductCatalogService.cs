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
using Kona.UILogic.Services;

namespace Kona.UILogic.Tests.Mocks
{
    public class MockProductCatalogService : IProductCatalogService
    {
        public Func<int, Task<ObservableCollection<Category>>> GetCategoriesAsyncDelegate { get; set; }
        public Func<int, Task<ObservableCollection<Category>>> GetSubcategoriesAsyncDelegate { get; set; }
        public Func<int, Task<ObservableCollection<Product>>> GetProductsAsyncDelegate { get; set; }
        public Func<int, Task<Category>> GetCategoryAsyncDelegate { get; set; }
        public Func<string, Task<Product>> GetProductAsyncDelegate { get; set; }

        public Task<ObservableCollection<Category>> GetCategoriesAsync(int depth = 1)
        {
            return this.GetCategoriesAsyncDelegate(depth);
        }

        public Task<ObservableCollection<Category>> GetSubcategoriesAsync(int categoryId)
        {
            return this.GetSubcategoriesAsyncDelegate(categoryId);
        }

        public Task<ObservableCollection<Product>> GetProductsAsync(int categoryId)
        {
            return this.GetProductsAsyncDelegate(categoryId);
        }

        public Task<Category> GetCategoryAsync(int categoryId)
        {
            return this.GetCategoryAsyncDelegate(categoryId);
        }

        public Task<Product> GetProductAsync(string productNumber)
        {
            return this.GetProductAsyncDelegate(productNumber);
        }
    }
}