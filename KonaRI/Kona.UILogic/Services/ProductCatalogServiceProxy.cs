// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.ObjectModel;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;

namespace Kona.UILogic.Services
{
    public class ProductCatalogServiceProxy : IProductCatalogService
    {
        private string _productsBaseUrl = string.Format("{0}/api/Product/", Constants.ServerAddress);
        private string _categoriesBaseUrl = string.Format("{0}/api/Category/", Constants.ServerAddress);

        public async Task<ObservableCollection<Category>> GetCategoriesAsync(int depth)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(string.Format("{0}?depth={1}", _categoriesBaseUrl, depth));
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsAsync<ObservableCollection<Category>>();

                return result;
            }
        }

        public async Task<ObservableCollection<Category>> GetSubcategoriesAsync(int categoryId)
        {
            using (var httpClient = new HttpClient())
            {
                var response =
                    await httpClient.GetAsync(string.Format("{0}?categoryId={1}", _categoriesBaseUrl, categoryId));
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsAsync<ObservableCollection<Category>>();

                return result;
            }
        }

        public async Task<ObservableCollection<Product>> GetProductsAsync(int categoryId)
        {
            using (var httpClient = new HttpClient())
            {
                var response =
                    await httpClient.GetAsync(string.Format("{0}?categoryId={1}", _productsBaseUrl, categoryId));
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsAsync<ObservableCollection<Product>>();

                return result;
            }
        }

        public async Task<Category> GetCategoryAsync(int categoryId)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(_categoriesBaseUrl + categoryId.ToString());
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsAsync<Category>();

                return result;
            }
        }

        public async Task<Product> GetProductAsync(string productNumber)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(_productsBaseUrl + productNumber);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsAsync<Product>();

                return result;
            }
        }
    }
}
