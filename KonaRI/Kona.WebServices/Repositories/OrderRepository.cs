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
    public class OrderRepository : IRepository<Order>
    {
        private static ICollection<Order> _orders;

        public OrderRepository()
        {
            if (_orders == null)
            {
                _orders = new List<Order>(); 
            }
        }

        public IEnumerable<Order> GetAll()
        {
            return _orders;
        }

        public Order GetItem(int id)
        {
            return _orders.FirstOrDefault(c => c.Id == id);
        }

        public Order Create(Order item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            item.Id = _orders.Any() ? _orders.Max(c => c.Id) : 1;
            _orders.Add(item);
            return item;    
        }

        public bool Update(Order item)
        {
            var order = _orders.FirstOrDefault(c => c.Id == item.Id);

            if (order != null)
            {
                order.ShoppingCart = item.ShoppingCart;
                order.ShippingAddress = item.ShippingAddress;
                order.BillingAddress = item.BillingAddress;
                order.PaymentMethod = item.PaymentMethod;
                order.ShippingMethod = item.ShippingMethod;

                return true;
            }

            return false;
        }

        public bool Delete(int id)
        {
            var order = _orders.FirstOrDefault(c => c.Id == id);
            return _orders.Remove(order);
        }
    }
}