// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
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

        // <snippet512>
        public async Task<ReadOnlyCollection<Category>> GetCategoriesAsync(int maxAmountOfProducts)
        {
            string cacheFileName = String.Format("{0}{1}", "Categories", maxAmountOfProducts);

            if (await _cacheService.DataExistsAndIsValidAsync(cacheFileName))
            {
                // Retrieve the items from the cache
                return await _cacheService.GetDataAsync<ReadOnlyCollection<Category>>(cacheFileName);
            }
            else
            {
                // Retrieve the items from the service
                var items = await _productCatalogService.GetCategoriesAsync(maxAmountOfProducts);

                // Save the images locally
                // Update the item's local URI
                foreach (var item in items)
                {
                    if (item.ImageExternalUri != null)
                    {
                        string imageFileName = item.ImageExternalUri.ToString().Substring(item.ImageExternalUri.ToString().LastIndexOf('/') + 1);
                        item.ImageLocalUri = await _cacheService.SaveExternalDataAsync(imageFileName, item.ImageExternalUri);

                        // TODO Save the images locally for each subcategory & update the item's local URI
                        if (item.Products != null)
                        {
                            foreach (var subItem in item.Products)
                            {
                                if (subItem.ImageName != null)
                                {
                                    var localUri = await _cacheService.SaveExternalDataAsync(subItem.ImageName, new Uri("/Images/" + subItem.ImageName, UriKind.Relative));
                                    subItem.ImageName = localUri.ToString();
                                }
                            }
                        }
                    }
                }

                // Save the items in the cache
                await _cacheService.SaveDataAsync(cacheFileName, items);

                return items;
            }
        }
        // </snippet512>

        public async Task<ReadOnlyCollection<Category>> GetFilteredProductsAsync(string queryString)
        {

            // Retrieve the items from the service
            var items = await _productCatalogService.GetFilteredProductsAsync(queryString);

            // Save the images locally
            // Update the item's local URI
            foreach (var item in items)
            {
                if (item.ImageExternalUri != null)
                {
                    string imageFileName = item.ImageExternalUri.ToString().Substring(item.ImageExternalUri.ToString().LastIndexOf('/') + 1);
                    item.ImageLocalUri = await _cacheService.SaveExternalDataAsync(imageFileName, item.ImageExternalUri);

                    // TODO Save the images locally for each subcategory & update the item's local URI
                    if (item.Products != null)
                    {
                        foreach (var subItem in item.Products)
                        {
                            if (subItem.ImageName != null)
                            {
                                var localUri = await _cacheService.SaveExternalDataAsync(subItem.ImageName, new Uri("/Images/" + subItem.ImageName, UriKind.Relative));
                                subItem.ImageName = localUri.ToString();
                            }
                        }
                    }
                }
            }

            return items;
        }

        public async Task<ReadOnlyCollection<Category>> GetSubcategoriesAsync(int categoryId)
        {
            string cacheFileName = string.Format("SubCategoriesOfCategoryId{0}", categoryId);

            if (await _cacheService.DataExistsAndIsValidAsync(cacheFileName))
            {
                // Retrieve the items from the cache
                return await _cacheService.GetDataAsync<ReadOnlyCollection<Category>>(cacheFileName);
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
                    if (item.Products != null)
                    {
                        foreach (var product in item.Products)
                        {
                            if (product.ImageName != null)
                            {
                                var localUri = await _cacheService.SaveExternalDataAsync(product.ImageName, new Uri("/Images/" + product.ImageName, UriKind.Relative));
                                product.ImageName = localUri.ToString();
                            }
                        }
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

        public async Task<ReadOnlyCollection<Product>> GetProductsAsync(int categoryId)
        {
            string cacheFileName = string.Format("SubProductsOfCategoryId{0}", categoryId);

            if (await _cacheService.DataExistsAndIsValidAsync(cacheFileName))
            {
                // Retrieve the items from the cache
                return await _cacheService.GetDataAsync<ReadOnlyCollection<Product>>(cacheFileName);
            }
            else
            {
                // Retrieve the items from the service
                var items = await _productCatalogService.GetProductsAsync(categoryId);

                // Save the images locally
                // Update the item's local URI
                foreach (var item in items)
                {
                    if (item.ImageName != null)
                    {
                        var localUri = await _cacheService.SaveExternalDataAsync(item.ImageName, new Uri("/Images/" + item.ImageName, UriKind.Relative));
                        item.ImageName = localUri.ToString();
                    }
                }

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

                if (product.ImageName != null)
                {
                    var localUri = await _cacheService.SaveExternalDataAsync(product.ImageName, new Uri("/Images/" + product.ImageName, UriKind.Relative));
                    product.ImageName = localUri.ToString();
                }

                // Save the items in the cache
                await _cacheService.SaveDataAsync(cacheFileName, product);

                return product;
            }
        }
    }
}
