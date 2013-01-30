// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Kona.WebServices.Models;
using Kona.WebServices.Repositories;


namespace Kona.WebServices.Controllers
{
    public class CategoryController : ApiController
    {
        private IRepository<Category> _categoryRepository;
        private IProductRepository _productRepository;

        public CategoryController()
            : this(new CategoryRepository(), new ProductRepository())
        { }

        public CategoryController(IRepository<Category> categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        // GET /api/Category?maxAmmountOfProducts={maxAmountOfProducts}
        public IEnumerable<Category> GetCategories(int maxAmountOfProducts)
        {
            var rootCategories = _categoryRepository.GetAll().Where(c => c.ParentId == 0).ToList();
            FillProducts(rootCategories, maxAmountOfProducts);
            return rootCategories;
        }

        // GET /api/Category/id
        // <snippet514>
        public Category GetCategory(int id)
        {
            var item = _categoryRepository.GetItem(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return item;
        }
        // </snippet514>

        // GET /api/Category?categoryId={categoryId}
        public IEnumerable<Category> GetSubcategories(int categoryId)
        {
            if (categoryId != 0)
            {
                var subcategories = _categoryRepository.GetAll().Where(c => c.ParentId == categoryId).ToList();
                FillSubcategories(subcategories);
                return subcategories;
            }
            else
            {
                var todaysDealsCategory = GetCategory(0);
                todaysDealsCategory.Products = _productRepository.TodaysDealsProducts;
                return new List<Category>() { todaysDealsCategory };
            }
        }

        private void FillProducts(IEnumerable<Category> categories, int maxAmountofItems)
        {
            foreach (var category in categories)
            {

                if (category.Id != 0)
                {
                    var subcategories = _categoryRepository.GetAll().Where(c => c.ParentId == category.Id).ToList();
                    var productList = new List<Product>();
                    foreach (var subcategory in subcategories)
                    {
                        productList.AddRange(_productRepository.GetProductsFromCategory(subcategory.Id));
                    }
                    category.Products = maxAmountofItems > 0
                                            ? productList.Where(p => p.ImageName != "no_image_available_large.gif")
                                                         .Take(maxAmountofItems)
                                            : productList;
                }
                else
                {
                    //Today's Deals Category
                    category.Products = _productRepository.TodaysDealsProducts;
                }
            }
        }

        private void FillSubcategories(IEnumerable<Category> subcategories)
        {
            foreach (var category in subcategories)
            {
                var productList = new List<Product>();
                productList.AddRange(_productRepository.GetProductsFromCategory(category.Id));
                category.Products = productList;
            }
        }
    }
}
