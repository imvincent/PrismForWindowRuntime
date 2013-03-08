// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockCheckoutDataRepository : ICheckoutDataRepository
    {
        public Func<string, Address> GetShippingAddressDelegate { get; set; }
        public Func<string, Address> GetBillingAddresDelegate { get; set; }
        public Func<string, Task<PaymentMethod>> GetPaymentMethodDelegate { get; set; }
        public Func<Address> GetDefaultShippingAddressDelegate { get; set; }
        public Func<Address> GetDefaultBillingAddresDelegate { get; set; }
        public Func<Task<PaymentMethod>> GetDefaultPaymentMethodDelegate { get; set; } 

        public Address GetShippingAddress(string id)
        {
            return GetShippingAddressDelegate(id);
        }

        public Address GetBillingAddress(string id)
        {
            return GetBillingAddresDelegate(id);
        }

        public Task<PaymentMethod> GetPaymentMethodAsync(string id)
        {
            return GetPaymentMethodDelegate(id);
        }

        public Address GetDefaultShippingAddress()
        {
            return GetDefaultShippingAddressDelegate();
        }

        public Address GetDefaultBillingAddress()
        {
            return GetDefaultShippingAddressDelegate();
        }

        public Task<PaymentMethod> GetDefaultPaymentMethodAsync()
        {
            return GetDefaultPaymentMethodDelegate();
        }

        public IReadOnlyCollection<Address> GetAllShippingAddresses()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<Address> GetAllBillingAddresses()
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<PaymentMethod>> GetAllPaymentMethodsAsync()
        {
            throw new NotImplementedException();
        }

        public void SaveShippingAddress(Address address)
        {
            throw new NotImplementedException();
        }

        public void SaveBillingAddress(Address address)
        {
            throw new NotImplementedException();
        }

        public Task SavePaymentMethodAsync(PaymentMethod paymentMethod)
        {
            throw new NotImplementedException();
        }

        public void SetDefaultShippingAddress(string address)
        {
            throw new NotImplementedException();
        }

        public void SetDefaultBillingAddress(string address)
        {
            throw new NotImplementedException();
        }

        public void SetDefaultPaymentMethod(string paymentMethod)
        {
            throw new NotImplementedException();
        }

        public void RemoveDefaultShippingAddress()
        {
            throw new NotImplementedException();
        }

        public void RemoveDefaultBillingAddress()
        {
            throw new NotImplementedException();
        }

        public void RemoveDefaultPaymentMethod()
        {
            throw new NotImplementedException();
        }
    }
}
