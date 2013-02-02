// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
namespace Kona.UILogic.Tests.Mocks
{
    public class MockCheckoutDataRepository : ICheckoutDataRepository
    {
        public ICollection<Address> GetAllShippingAddresses()
        {
            throw new System.NotImplementedException();
        }

        public ICollection<Address> GetAllBillingAddresses()
        {
            throw new System.NotImplementedException();
        }

        public ICollection<PaymentMethod> GetAllPaymentMethods()
        {
            throw new System.NotImplementedException();
        }

        public Address SaveShippingAddress(Address address)
        {
            throw new System.NotImplementedException();
        }

        public Address SaveBillingAddress(Address address)
        {
            throw new System.NotImplementedException();
        }

        public PaymentMethod SavePaymentMethod(PaymentMethod paymentMethod)
        {
            throw new System.NotImplementedException();
        }

        public bool ContainsDefaultValue(string container)
        {
            throw new System.NotImplementedException();
        }

        public void SetAsDefaultShippingAddress(string id)
        {
            throw new System.NotImplementedException();
        }

        public void SetAsDefaultBillingAddress(string id)
        {
            throw new System.NotImplementedException();
        }

        public void SetAsDefaultPaymentMethod(string id)
        {
            throw new System.NotImplementedException();
        }

        public Address GetDefaultShippingAddressValue()
        {
            throw new System.NotImplementedException();
        }

        public Address GetDefaultBillingAddressValue()
        {
            throw new System.NotImplementedException();
        }

        public PaymentMethod GetDefaultPaymentMethodValue()
        {
            throw new System.NotImplementedException();
        }

        public void DeleteShippingAddressValue(string id)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteBillingAddressValue(string id)
        {
            throw new System.NotImplementedException();
        }

        public void DeletePaymentMethodValue(string id)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteContainer(string container)
        {
            throw new System.NotImplementedException();
        }

        public PaymentMethod GetPaymentMethod(string id)
        {
            throw new System.NotImplementedException();
        }

        public Address GetShippingAddress(string id)
        {
            throw new System.NotImplementedException();
        }

        public Address GetBillingAddress(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}
