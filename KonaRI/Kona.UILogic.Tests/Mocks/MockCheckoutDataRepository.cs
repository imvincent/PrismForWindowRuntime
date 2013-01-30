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
        public ICollection<Address> RetrieveAllShippingAddresses()
        {
            throw new System.NotImplementedException();
        }

        public ICollection<Address> RetrieveAllBillingAddresses()
        {
            throw new System.NotImplementedException();
        }

        public ICollection<PaymentInfo> RetrieveAllPaymentInformation()
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

        public PaymentInfo SavePaymentInfo(PaymentInfo paymentInfo)
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

        public void SetAsDefaultPaymentInfo(string id)
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

        public PaymentInfo GetDefaultPaymentInfoValue()
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

        public void DeletePaymentInfoValue(string id)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteContainer(string container)
        {
            throw new System.NotImplementedException();
        }

        public PaymentInfo RetrievePaymentInformation(string id)
        {
            throw new System.NotImplementedException();
        }

        public Address RetrieveShippingAddress(string id)
        {
            throw new System.NotImplementedException();
        }

        public Address RetrieveBillingAddress(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}
