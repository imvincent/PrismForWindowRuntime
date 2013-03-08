// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Services;

namespace AdventureWorks.UILogic.Repositories
{
    public class ProductCatalogRepository : IProductCatalogRepository
    {
        private readonly IProductCatalogService _productCatalogService;
        private readonly ICacheService _cacheService;

        public ProductCatalogRepository(IProductCatalogService productCatalogService, ICacheService cacheService)
        {
            _productCatalogService = productCatalogService;
            _cacheService = cacheService;
        }

        public async Task<ReadOnlyCollection<Category>> GetRootCategoriesAsync(int maxAmountOfProducts)
        {
            return await GetSubcategoriesAsync(0, maxAmountOfProducts);
        }

        // <snippet512>
        public async Task<ReadOnlyCollection<Category>> GetSubcategoriesAsync(int parentId, int maxAmountOfProducts)
        {
            string cacheFileName = String.Format("Categories-{0}-{1}", parentId, maxAmountOfProducts);

            try
            {
                // Case 1: Retrieve the items from the cache
                return await _cacheService.GetDataAsync<ReadOnlyCollection<Category>>(cacheFileName);
            }
            catch (FileNotFoundException)
            { }

            // Retrieve the items from the service
            var categories = await _productCatalogService.GetCategoriesAsync(parentId, maxAmountOfProducts);

            // Save the items in the cache
            await _cacheService.SaveDataAsync(cacheFileName, categories);

            return categories;
        }
        // </snippet512>

        public async Task<SearchResult> GetFilteredProductsAsync(string productsQueryString)
        {
            // Retrieve the items from the service
            var searchResult = await _productCatalogService.GetFilteredProductsAsync(productsQueryString);
            return searchResult;
        }



        public async Task<ReadOnlyCollection<Product>> GetProductsAsync(int categoryId)
        {
            string cacheFileName = string.Format("SubProductsOfCategoryId{0}", categoryId);

            try
            {
                // Retrieve the items from the cache
                return await _cacheService.GetDataAsync<ReadOnlyCollection<Product>>(cacheFileName);
            }
            catch (FileNotFoundException)
            {
            }

            // Retrieve the items from the service
            var products = await _productCatalogService.GetProductsAsync(categoryId);

            await _cacheService.SaveDataAsync(cacheFileName, products);

            return products;

        }

        public async Task<Category> GetCategoryAsync(int categoryId)
        {
            string cacheFileName = string.Format("CategoryId{0}", categoryId);

            try
            {
                // Retrieve the items from the cache
                return await _cacheService.GetDataAsync<Category>(cacheFileName);
            }
            catch (FileNotFoundException)
            {
            }

            // Retrieve the items from the service
            var category = await _productCatalogService.GetCategoryAsync(categoryId);

            // Save the items in the cache
            await _cacheService.SaveDataAsync(cacheFileName, category);

            return category;
        }

        public async Task<Product> GetProductAsync(string productNumber)
        {
            string cacheFileName = string.Format("Product{0}", productNumber);

            try
            {
                // Retrieve the items from the cache
                return await _cacheService.GetDataAsync<Product>(cacheFileName);
            }
            catch (FileNotFoundException)
            {

            }
            // Retrieve the items from the service
            var product = await _productCatalogService.GetProductAsync(productNumber);

            // Save the items in the cache
            await _cacheService.SaveDataAsync(cacheFileName, product);

            return product;
        }
    }
}