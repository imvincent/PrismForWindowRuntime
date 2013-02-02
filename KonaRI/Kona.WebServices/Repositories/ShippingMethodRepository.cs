// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using Kona.WebServices.Models;
using System.Linq;
using System;

namespace Kona.WebServices.Repositories
{
    public class ShippingMethodRepository : IRepository<ShippingMethod>
    {
        private static ICollection<ShippingMethod> _shippingMethod;

        public ShippingMethodRepository()
        {
            if (_shippingMethod == null)
            {
                PopulateShippingMethods();
            }
        }

        public IEnumerable<ShippingMethod> GetAll()
        {
            return _shippingMethod;
        }

        public ShippingMethod GetItem(int id)
        {
            return _shippingMethod.FirstOrDefault(c => c.Id == id);
        }

        public ShippingMethod Create(ShippingMethod item)
        {
            throw new NotImplementedException();
        }

        public bool Update(ShippingMethod item)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        private static void PopulateShippingMethods()
        {
            _shippingMethod = new List<ShippingMethod>()
            {
                new ShippingMethod() { Id = 1, Description = "Standard Shipping", EstimatedTime = "5-8 business days", Cost = 7.65 },
                new ShippingMethod() { Id = 2, Description = "Expedited Shipping", EstimatedTime = "3-5 business days", Cost = 14.25 },
                new ShippingMethod() { Id = 3, Description = "Two-day Shipping", EstimatedTime = "2 business days", Cost = 26.45 },
                new ShippingMethod() { Id = 4, Description = "One-day Shipping", EstimatedTime = "1 business day", Cost = 37.32 }
            };
        }
    }
}