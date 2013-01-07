// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.WebServices.Models;
using Kona.WebServices.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace Kona.WebServices.Controllers
{
    public class ProductController : ApiController
    {
        // GET /api/Product
        public IEnumerable<Product> GetProducts()
        {
            return ProductRepository.Products;
        }

        // GET /api/Product/id
        public Product GetProduct(string id)
        {
            var item = ProductRepository.Products.Where(p => p.ProductNumber == id).FirstOrDefault();
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return item;
        }

        // GET /api/Product?categoryId={categoryId}
        public IEnumerable<Product> GetProducts(int categoryId)
        {
            return ProductRepository.Products.Where(p => p.SubcategoryId == categoryId);
        }
    }
}
