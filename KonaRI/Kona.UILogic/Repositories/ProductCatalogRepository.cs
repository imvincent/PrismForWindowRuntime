// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Kona.Infrastructure;
using Kona.UILogic.Models;
using Kona.UILogic.Services;

namespace Kona.UILogic.Repositories
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

        public async Task<ObservableCollection<Category>> GetCategoriesAsync()
        {
            string cacheFileName = "Categories";

            if (await _cacheService.DataExistsAndIsValidAsync(cacheFileName))
            {
                // Retrieve the items from the cache
                return await _cacheService.GetDataAsync<ObservableCollection<Category>>(cacheFileName);
            }
            else
            {
                // Retrieve the items from the service
                var items = await _productCatalogService.GetCategoriesAsync(depth: 1);

                // Save the images locally
                // Update the item's local URI
                foreach (var item in items)
                {
                    if (item.ImageExternalUri != null)
                    {
                        string imageFileName = item.ImageExternalUri.ToString().Substring(item.ImageExternalUri.ToString().LastIndexOf('/') + 1);
                        item.ImageLocalUri = await _cacheService.SaveExternalDataAsync(imageFileName, item.ImageExternalUri);
                    }
                }

                // Save the items in the cache
                await _cacheService.SaveDataAsync(cacheFileName, items);

                return items;
            }
        }

        public async Task<ObservableCollection<Category>> GetSubcategoriesAsync(int categoryId)
        {
            string cacheFileName = string.Format("SubCategoriesOfCategoryId{0}", categoryId);

            if (await _cacheService.DataExistsAndIsValidAsync(cacheFileName))
            {
                // Retrieve the items from the cache
                return await _cacheService.GetDataAsync<ObservableCollection<Category>>(cacheFileName);
            }
            else
            {
                // Retrieve the items from the service
                var items = await _productCatalogService.GetSubcategoriesAsync(categoryId);

                // Save the images locally
                // Update the item's local URI
                foreach (var item in items)
                {
                    if (item.ImageExternalUri != null)
                    {
                        string imageFileName = item.ImageExternalUri.ToString().Substring(item.ImageExternalUri.ToString().LastIndexOf('/') + 1);
                        item.ImageLocalUri = await _cacheService.SaveExternalDataAsync(imageFileName, item.ImageExternalUri);
                    }
                }

                if (items.Count > 0)
                {
                    // Save the items in the cache
                    await _cacheService.SaveDataAsync(cacheFileName, items);
                }

                return items;
            }
        }

        public async Task<ObservableCollection<Product>> GetProductsAsync(int categoryId)
        {
            string cacheFileName = string.Format("SubProductsOfCategoryId{0}", categoryId);

            if (await _cacheService.DataExistsAndIsValidAsync(cacheFileName))
            {
                // Retrieve the items from the cache
                return await _cacheService.GetDataAsync<ObservableCollection<Product>>(cacheFileName);
            }
            else
            {
                // Retrieve the items from the service
                var items = await _productCatalogService.GetProductsAsync(categoryId);

                // Save the items in the cache
                await _cacheService.SaveDataAsync(cacheFileName, items);

                return items;
            }
        }

        public async Task<Category> GetCategoryAsync(int categoryId)
        {
            string cacheFileName = string.Format("CategoryId{0}", categoryId);

            if (await _cacheService.DataExistsAndIsValidAsync(cacheFileName))
            {
                // Retrieve the items from the cache
                return await _cacheService.GetDataAsync<Category>(cacheFileName);
            }
            else
            {
                // Retrieve the items from the service
                var category = await _productCatalogService.GetCategoryAsync(categoryId);

                // Save the items in the cache
                await _cacheService.SaveDataAsync(cacheFileName, category);

                return category;
            }
        }

        public async Task<Product> GetProductAsync(string productNumber)
        {
            string cacheFileName = string.Format("Product{0}", productNumber);

            if (await _cacheService.DataExistsAndIsValidAsync(cacheFileName))
            {
                // Retrieve the items from the cache
                return await _cacheService.GetDataAsync<Product>(cacheFileName);
            }
            else
            {
                // Retrieve the items from the service
                var product = await _productCatalogService.GetProductAsync(productNumber);

                // Save the items in the cache
                await _cacheService.SaveDataAsync(cacheFileName, product);

                return product;
            }
        }
    }
}
