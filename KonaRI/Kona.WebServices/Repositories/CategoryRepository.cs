// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kona.WebServices.Models;

namespace Kona.WebServices.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private static IEnumerable<Category> _categories;

        public CategoryRepository()
        {
            if (_categories == null)
            {
                PopulateCategories();
            }
        }

        public IEnumerable<Category> GetAll()
        {
            return _categories;
        }

        public Category GetItem(int id)
        {
            return _categories.FirstOrDefault(c => c.Id == id);
        }

        public Category Create(Category item)
        {
            throw new NotImplementedException();
        }

        public bool Update(Category item)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Category item)
        {
            throw new NotImplementedException();
        }

        private static void PopulateCategories()
        {
            _categories = new List<Category>
             {
                 new Category {Title = "Today's Deals", Id = 0, ImageExternalUri = new Uri("/Images/water_bottle_cage_small.gif", UriKind.Relative) },
                 new Category {Title = "Accessories", Id = 4000, ImageExternalUri = new Uri("/Images/water_bottle_cage_small.gif", UriKind.Relative) },
                 new Category {Title = "Bikes", Id = 1000, ImageExternalUri = new Uri("/Images/racer02_yellow_f_small.gif", UriKind.Relative) },
                 new Category {Title = "Clothing", Id = 3000, ImageExternalUri = new Uri("/Images/awc_jersey_male_small.gif", UriKind.Relative) },
                 new Category {Title = "Components", Id = 2000, ImageExternalUri = new Uri("/Images/silver_chain_small.gif", UriKind.Relative) },
new Category { Title = "Mountain Bikes", Id=1, ParentId=1000, ImageExternalUri = new Uri("/Images/hotrodbike_f_large.gif", UriKind.Relative) },
new Category { Title = "Road Bikes", Id=2, ParentId=1000, ImageExternalUri = new Uri("/Images/roadster_black_large.gif", UriKind.Relative) },
new Category { Title = "Touring Bikes", Id=3, ParentId=1000, ImageExternalUri = new Uri("/Images/julianax_r_02_blue_large.gif", UriKind.Relative) },
new Category { Title = "Handlebars", Id=4, ParentId=2000, ImageExternalUri = new Uri("/Images/handlebar_large.gif", UriKind.Relative) },
new Category { Title = "Bottom Brackets", Id=5, ParentId=2000, ImageExternalUri = new Uri("/Images/no_image_available_large.gif", UriKind.Relative) },
new Category { Title = "Brakes", Id=6, ParentId=2000, ImageExternalUri = new Uri("/Images/no_image_available_large.gif", UriKind.Relative) },
new Category { Title = "Chains", Id=7, ParentId=2000, ImageExternalUri = new Uri("/Images/silver_chain_large.gif", UriKind.Relative) },
new Category { Title = "Cranksets", Id=8, ParentId=2000, ImageExternalUri = new Uri("/Images/no_image_available_large.gif", UriKind.Relative) },
new Category { Title = "Derailleurs", Id=9, ParentId=2000, ImageExternalUri = new Uri("/Images/sprocket_large.gif", UriKind.Relative) },
new Category { Title = "Forks", Id=10, ParentId=2000, ImageExternalUri = new Uri("/Images/fork_large.gif", UriKind.Relative) },
new Category { Title = "Headsets", Id=11, ParentId=2000, ImageExternalUri = new Uri("/Images/no_image_available_large.gif", UriKind.Relative) },
new Category { Title = "Mountain Frames", Id=12, ParentId=2000, ImageExternalUri = new Uri("/Images/frame_silver_large.gif", UriKind.Relative) },
new Category { Title = "Pedals", Id=13, ParentId=2000, ImageExternalUri = new Uri("/Images/pedal_large.gif", UriKind.Relative) },
new Category { Title = "Road Frames", Id=14, ParentId=2000, ImageExternalUri = new Uri("/Images/no_image_available_large.gif", UriKind.Relative) },
new Category { Title = "Saddles", Id=15, ParentId=2000, ImageExternalUri = new Uri("/Images/saddle_large.gif", UriKind.Relative) },
new Category { Title = "Touring Frames", Id=16, ParentId=2000, ImageExternalUri = new Uri("/Images/frame_large.gif", UriKind.Relative) },
new Category { Title = "Wheels", Id=17, ParentId=2000, ImageExternalUri = new Uri("/Images/wheel_large.gif", UriKind.Relative) },
new Category { Title = "Bib-Shorts", Id=18, ParentId=3000, ImageExternalUri = new Uri("/Images/no_image_available_large.gif", UriKind.Relative) },
new Category { Title = "Caps", Id=19, ParentId=3000, ImageExternalUri = new Uri("/Images/no_image_available_large.gif", UriKind.Relative) },
new Category { Title = "Gloves", Id=20, ParentId=3000, ImageExternalUri = new Uri("/Images/no_image_available_large.gif", UriKind.Relative) },
new Category { Title = "Jerseys", Id=21, ParentId=3000, ImageExternalUri = new Uri("/Images/awc_tee_male_yellow_large.gif", UriKind.Relative) },
new Category { Title = "Shorts", Id=22, ParentId=3000, ImageExternalUri = new Uri("/Images/shorts_female_large.gif", UriKind.Relative) },
new Category { Title = "Socks", Id=23, ParentId=3000, ImageExternalUri = new Uri("/Images/no_image_available_large.gif", UriKind.Relative) },
new Category { Title = "Tights", Id=24, ParentId=3000, ImageExternalUri = new Uri("/Images/no_image_available_large.gif", UriKind.Relative) },
new Category { Title = "Vests", Id=25, ParentId=3000, ImageExternalUri = new Uri("/Images/no_image_available_large.gif", UriKind.Relative) },
new Category { Title = "Bike Racks", Id=26, ParentId=4000, ImageExternalUri = new Uri("/Images/no_image_available_large.gif", UriKind.Relative) },
new Category { Title = "Bike Stands", Id=27, ParentId=4000, ImageExternalUri = new Uri("/Images/no_image_available_large.gif", UriKind.Relative) },
new Category { Title = "Bottles and Cages", Id=28, ParentId=4000, ImageExternalUri = new Uri("/Images/water_bottle_cage_large.gif", UriKind.Relative) },
new Category { Title = "Cleaners", Id=29, ParentId=4000, ImageExternalUri = new Uri("/Images/no_image_available_large.gif", UriKind.Relative) },
new Category { Title = "Fenders", Id=30, ParentId=4000, ImageExternalUri = new Uri("/Images/no_image_available_large.gif", UriKind.Relative) },
new Category { Title = "Helmets", Id=31, ParentId=4000, ImageExternalUri = new Uri("/Images/no_image_available_large.gif", UriKind.Relative) },
new Category { Title = "Hydration Packs", Id=32, ParentId=4000, ImageExternalUri = new Uri("/Images/no_image_available_large.gif", UriKind.Relative) },
new Category { Title = "Lights", Id=33, ParentId=4000, ImageExternalUri = new Uri("/Images/tail_lights_large.gif", UriKind.Relative) },
new Category { Title = "Locks", Id=34, ParentId=4000, ImageExternalUri = new Uri("/Images/bike_lock_large.gif", UriKind.Relative) },
new Category { Title = "Panniers", Id=35, ParentId=4000, ImageExternalUri = new Uri("/Images/no_image_available_large.gif", UriKind.Relative) },
new Category { Title = "Pumps", Id=36, ParentId=4000, ImageExternalUri = new Uri("/Images/handpump_large.gif", UriKind.Relative) },
new Category { Title = "Tires and Tubes", Id=37, ParentId=4000, ImageExternalUri = new Uri("/Images/mb_tires_large.gif", UriKind.Relative) },

            };
        }
    }
}