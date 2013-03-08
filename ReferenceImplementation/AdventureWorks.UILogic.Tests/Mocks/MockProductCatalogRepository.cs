// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Repositories;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockProductCatalogRepository : IProductCatalogRepository
    {
        public Func<int, Task<ReadOnlyCollection<Category>>> GetRootCategoriesAsyncDelegate { get; set; }
        public Func<int, int, Task<ReadOnlyCollection<Category>>> GetSubcategoriesAsyncDelegate { get; set; }
        public Func<string, Task<SearchResult>> GetFilteredProductsAsyncDelegate { get; set; }
        public Func<int, Task<ReadOnlyCollection<Product>>> GetProductsAsyncDelegate { get; set; }
        public Func<int, Task<Category>> GetCategoryAsyncDelegate { get; set; }
        public Func<string, Task<Product>> GetProductAsyncDelegate { get; set; }

        public Task<ReadOnlyCollection<Category>> GetRootCategoriesAsync(int maxAmountOfProducts)
        {
            return GetRootCategoriesAsyncDelegate(maxAmountOfProducts);
        }

        public Task<ReadOnlyCollection<Category>> GetSubcategoriesAsync(int parentId, int maxAmountOfProducts)
        {
            return this.GetSubcategoriesAsyncDelegate(parentId, maxAmountOfProducts);
        }

        public Task<SearchResult> GetFilteredProductsAsync(string productsQueryString)
        {
            return this.GetFilteredProductsAsyncDelegate(productsQueryString);
        }

        public Task<ReadOnlyCollection<Product>> GetProductsAsync(int categoryId)
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
