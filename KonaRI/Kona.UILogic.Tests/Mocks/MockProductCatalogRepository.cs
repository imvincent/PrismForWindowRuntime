// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;

namespace Kona.UILogic.Tests.Mocks
{
    public class MockProductCatalogRepository : IProductCatalogRepository
    {
        public Func<Task<ObservableCollection<Category>>> GetCategoriesAsyncDelegate { get; set; }
        public Func<int, Task<ObservableCollection<Category>>> GetSubcategoriesAsyncDelegate { get; set; }
        public Func<int, Task<ObservableCollection<Product>>> GetProductsAsyncDelegate { get; set; }
        public Func<int, Task<Category>> GetCategoryAsyncDelegate { get; set; }
        public Func<string, Task<Product>> GetProductAsyncDelegate { get; set; }

        public Task<ObservableCollection<Category>> GetCategoriesAsync()
        {
            return this.GetCategoriesAsyncDelegate();
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
