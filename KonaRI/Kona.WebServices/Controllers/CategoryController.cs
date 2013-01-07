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

namespace Kona.WebServices.Controllers
{
    public class CategoryController : ApiController
    {
        // GET /api/Category?depth={depth}
        public IEnumerable<Category> GetCategories(int depth)
        {
            var rootCategories = categories.Where(c=>c.ParentCategoryId == 0);

            this.FillSubcategories(rootCategories, depth);

            return rootCategories;
        }

        // GET /api/Category/id
        public Category GetCategory(int id)
        {
            var item = categories.FirstOrDefault(c => c.CategoryId == id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return item;
        }

        // GET /api/Category?categoryId={categoryId}
        public IEnumerable<Category> GetSubcategories(int categoryId)
        {
            return categories.Where(c => c.ParentCategoryId == categoryId);
        }

        private void FillSubcategories(IEnumerable<Category> categories, int depth)
        {
            if (depth == 0) return;
            depth--;
            
            foreach (var category in categories)
            {
                var subcategories = GetSubcategories(category.CategoryId);
                category.Subcategories = new List<Category>(subcategories);
                this.FillSubcategories(subcategories, depth);
            }
        }

        private static List<Category> categories = new List<Category>
            {
                 new Category {Title = "Accessories", CategoryId = 4000, ImageExternalUri = new Uri("/Images/water_bottle_cage_small.gif", UriKind.Relative) },
                 new Category {Title = "Bikes", CategoryId = 1000, ImageExternalUri = new Uri("/Images/racer02_yellow_f_small.gif", UriKind.Relative) },
                 new Category {Title = "Clothing", CategoryId = 2000, ImageExternalUri = new Uri("/Images/awc_jersey_male_small.gif", UriKind.Relative) },
                 new Category {Title = "Components", CategoryId = 3000, ImageExternalUri = new Uri("/Images/silver_chain_small.gif", UriKind.Relative) },
                 new Category { Title = "Mountain Bikes", CategoryId=1000, ParentCategoryId=1},
                 new Category { Title = "Road Bikes", CategoryId=2, ParentCategoryId=1000},
                 new Category { Title = "Touring Bikes", CategoryId=3, ParentCategoryId=1000},
                 new Category { Title = "Handlebars", CategoryId=4, ParentCategoryId=2000},
                 new Category { Title = "Bottom Brackets", CategoryId=5, ParentCategoryId=2000},
                 new Category { Title = "Brakes", CategoryId=6, ParentCategoryId=2000},
                 new Category { Title = "Chains", CategoryId=7, ParentCategoryId=2000},
                 new Category { Title = "Cranksets", CategoryId=8, ParentCategoryId=2000},
                 new Category { Title = "Derailleurs", CategoryId=9, ParentCategoryId=2000},
                 new Category { Title = "Forks", CategoryId=10, ParentCategoryId=2000},
                 new Category { Title = "Headsets", CategoryId=11, ParentCategoryId=2000},
                 new Category { Title = "Mountain Frames", CategoryId=12, ParentCategoryId=2000},
                 new Category { Title = "Pedals", CategoryId=13, ParentCategoryId=2000},
                 new Category { Title = "Road Frames", CategoryId=14, ParentCategoryId=2000},
                 new Category { Title = "Saddles", CategoryId=15, ParentCategoryId=2000},
                 new Category { Title = "Touring Frames", CategoryId=16, ParentCategoryId=2000},
                 new Category { Title = "Wheels", CategoryId=17, ParentCategoryId=2000},
                 new Category { Title = "Bib-Shorts", CategoryId=18, ParentCategoryId=3000},
                 new Category { Title = "Caps", CategoryId=19, ParentCategoryId=3000},
                 new Category { Title = "Gloves", CategoryId=20, ParentCategoryId=3000},
                 new Category { Title = "Jerseys", CategoryId=21, ParentCategoryId=3000},
                 new Category { Title = "Shorts", CategoryId=22, ParentCategoryId=3000},
                 new Category { Title = "Socks", CategoryId=23, ParentCategoryId=3000},
                 new Category { Title = "Tights", CategoryId=24, ParentCategoryId=3000},
                 new Category { Title = "Vests", CategoryId=25, ParentCategoryId=3000},
                 new Category { Title = "Bike Racks", CategoryId=26, ParentCategoryId=4000, ImageExternalUri = new Uri("/Images/silver_chain_small.gif", UriKind.Relative)},
                 new Category { Title = "Bike Stands", CategoryId=27, ParentCategoryId=4000},
                 new Category { Title = "Bottles and Cages", CategoryId=28, ParentCategoryId=4000},
                 new Category { Title = "Cleaners", CategoryId=29, ParentCategoryId=4000},
                 new Category { Title = "Fenders", CategoryId=30, ParentCategoryId=4000},
                 new Category { Title = "Helmets", CategoryId=31, ParentCategoryId=4000},
                 new Category { Title = "Hydration Packs", CategoryId=32, ParentCategoryId=4000},
                 new Category { Title = "Lights", CategoryId=33, ParentCategoryId=4000},
                 new Category { Title = "Locks", CategoryId=34, ParentCategoryId=4000},
                 new Category { Title = "Panniers", CategoryId=35, ParentCategoryId=4000},
                 new Category { Title = "Pumps", CategoryId=36, ParentCategoryId=4000},
                 new Category { Title = "Tires and Tubes", CategoryId=37, ParentCategoryId=4000},
            };
    }
}
