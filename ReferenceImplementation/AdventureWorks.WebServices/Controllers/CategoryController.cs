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
using AdventureWorks.WebServices.Models;
using AdventureWorks.WebServices.Repositories;


namespace AdventureWorks.WebServices.Controllers
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

        // GET /api/Category?parentId={parentId}&maxAmmountOfProducts={maxAmountOfProducts}
        // <snippet514>
        public IEnumerable<Category> GetCategories(int parentId, int maxAmountOfProducts)
        {
            var categories = _categoryRepository.GetAll().Where(c => c.ParentId == parentId).ToList();
            FillProducts(categories);
            var trimmedCategories = categories.Select(NewCategory).ToList();

            foreach (var trimmedCategory in trimmedCategories)
            {
                var products = trimmedCategory.Products.ToList();
                products.Sort(Comparison);
                if (maxAmountOfProducts > 0)
                {
                    products = products.Take(maxAmountOfProducts).ToList();
                }
                trimmedCategory.Products = products;
            }

            return trimmedCategories;
        }

        // </snippet514>

        // GET /api/Category/id
        public Category GetCategory(int id)
        {
            var item = _categoryRepository.GetItem(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            var products = item.Products.ToList();
            products.Sort(Comparison);
            item.Products = products;

            return item;
        }

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
                todaysDealsCategory.Products = _productRepository.GetTodaysDealsProducts();
                todaysDealsCategory.TotalNumberOfItems = _productRepository.GetTodaysDealsProducts().Count();
                return new List<Category>() { todaysDealsCategory };
            }
        }

        private void FillProducts(IEnumerable<Category> categories)
        {
            foreach (var category in categories)
            {

                if (category.Id != 0)
                {
                    var subcategories = _categoryRepository.GetAll().Where(c => c.ParentId == category.Id).ToList();
                    var productList = new List<Product>();
                    if (subcategories.Count > 0)
                    {
                        category.HasSubcategories = true;
                        foreach (var subcategory in subcategories)
                        {
                            productList.AddRange(_productRepository.GetProductsForCategory(subcategory.Id));
                        }
                    }
                    else
                    {
                        category.HasSubcategories = false;
                        productList.AddRange(_productRepository.GetProductsForCategory(category.Id));
                    }
                    category.TotalNumberOfItems = productList.Count;
                    category.Products = productList;
                }
                else
                {
                    //Today's Deals Category
                    category.Products = _productRepository.GetTodaysDealsProducts();
                    category.TotalNumberOfItems = _productRepository.GetTodaysDealsProducts().Count();
                }
            }
        }

        private void FillSubcategories(IEnumerable<Category> subcategories)
        {
            foreach (var category in subcategories)
            {
                var productList = new List<Product>();
                productList.AddRange(_productRepository.GetProductsForCategory(category.Id));
                category.Products = productList;
            }
        }

        private static Category NewCategory(Category category)
        {
            if (category != null)
            {
                return new Category()
                    {
                        Id = category.Id,
                        Title = category.Title,
                        ParentId = category.ParentId,
                        HasSubcategories = category.HasSubcategories,
                        Subcategories = category.Subcategories,
                        ImageUri = category.ImageUri,
                        Products = category.Products,
                        TotalNumberOfItems = category.TotalNumberOfItems
                    };
            }
            return null;
        }

        private static int Comparison(Product product, Product product1)
        {
            if (product.ImageUri.AbsoluteUri.EndsWith("no_image_available_large.gif", StringComparison.OrdinalIgnoreCase) &&
                   !product1.ImageUri.AbsoluteUri.EndsWith("no_image_available_large.gif", StringComparison.OrdinalIgnoreCase))
            {
                return 1;
            }
            if (!product.ImageUri.AbsoluteUri.EndsWith("no_image_available_large.gif", StringComparison.OrdinalIgnoreCase) &&
                product1.ImageUri.AbsoluteUri.EndsWith("no_image_available_large.gif", StringComparison.OrdinalIgnoreCase))
            {
                return -1;
            }
            return 0;
        }
    }
}