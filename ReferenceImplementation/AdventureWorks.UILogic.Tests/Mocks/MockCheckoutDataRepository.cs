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
        public Func<string, Task<Address>> GetShippingAddressAsyncDelegate { get; set; }
        public Func<string, Task<Address>> GetBillingAddressAsyncDelegate { get; set; }
        public Func<string, Task<PaymentMethod>> GetPaymentMethodDelegate { get; set; }
        public Func<Task<Address>> GetDefaultShippingAddressAsyncDelegate { get; set; }
        public Func<Task<Address>> GetDefaultBillingAddressAsyncDelegate { get; set; }
        public Func<Task<PaymentMethod>> GetDefaultPaymentMethodAsyncDelegate { get; set; } 

        public Task<Address> GetShippingAddressAsync(string id)
        {
            return GetShippingAddressAsyncDelegate(id);
        }

        public Task<Address> GetBillingAddressAsync(string id)
        {
            return GetBillingAddressAsyncDelegate(id);
        }

        public Task<PaymentMethod> GetPaymentMethodAsync(string id)
        {
            return GetPaymentMethodDelegate(id);
        }

        public Task<Address> GetDefaultShippingAddressAsync()
        {
            return GetDefaultShippingAddressAsyncDelegate();
        }

        public Task<Address> GetDefaultBillingAddressAsync()
        {
            return GetDefaultBillingAddressAsyncDelegate();
        }

        public Task<PaymentMethod> GetDefaultPaymentMethodAsync()
        {
            return GetDefaultPaymentMethodAsyncDelegate();
        }

        public Task<IReadOnlyCollection<Address>> GetAllShippingAddressesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<Address>> GetAllBillingAddressesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<PaymentMethod>> GetAllPaymentMethodsAsync()
        {
            throw new NotImplementedException();
        }

        public Task SaveShippingAddressAsync(Address address)
        {
            throw new NotImplementedException();
        }

        public Task SaveBillingAddressAsync(Address address)
        {
            throw new NotImplementedException();
        }

        public Task SavePaymentMethodAsync(PaymentMethod paymentMethod)
        {
            throw new NotImplementedException();
        }

        public Task SetDefaultShippingAddressAsync(string addressId)
        {
            throw new NotImplementedException();
        }

        public Task SetDefaultBillingAddressAsync(string addressId)
        {
            throw new NotImplementedException();
        }

        public Task SetDefaultPaymentMethodAsync(string paymentMethodId)
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

        public Task RemoveDefaultShippingAddressAsync()
        {
            throw new NotImplementedException();
        }

        public Task RemoveDefaultBillingAddressAsync()
        {
            throw new NotImplementedException();
        }

        public Task RemoveDefaultPaymentMethodAsync()
        {
            throw new NotImplementedException();
        }
    }
}
