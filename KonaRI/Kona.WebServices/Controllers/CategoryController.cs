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

        public CategoryController()
            : this(new CategoryRepository())
        { }

        public CategoryController(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET /api/Category?depth={depth}
        public IEnumerable<Category> GetCategories(int depth)
        {
            var rootCategories = _categoryRepository.GetAll().Where(c => c.ParentId == 0);
            this.FillSubcategories(rootCategories, depth);

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
            return _categoryRepository.GetAll().Where(c => c.ParentId == categoryId);
        }

        private void FillSubcategories(IEnumerable<Category> categories, int depth)
        {
            if (depth == 0) return;
            depth--;
            
            foreach (var category in categories)
            {
                var subcategories = GetSubcategories(category.Id);
                category.Subcategories = new List<Category>(subcategories);
                this.FillSubcategories(subcategories, depth);
            }
        }
    }
}
