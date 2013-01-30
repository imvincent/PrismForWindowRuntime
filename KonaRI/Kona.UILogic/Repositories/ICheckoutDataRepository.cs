// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using Kona.UILogic.Models;

namespace Kona.UILogic.Repositories
{
    public interface ICheckoutDataRepository
    {
        ICollection<Address> RetrieveAllShippingAddresses();
        ICollection<Address> RetrieveAllBillingAddresses();
        ICollection<PaymentInfo> RetrieveAllPaymentInformation();
        Address SaveShippingAddress(Address address);
        Address SaveBillingAddress(Address address);
        PaymentInfo SavePaymentInfo(PaymentInfo paymentInfo);
        bool ContainsDefaultValue(string container);
        void SetAsDefaultShippingAddress(string id);
        void SetAsDefaultBillingAddress(string id);
        void SetAsDefaultPaymentInfo(string id);
        Address GetDefaultShippingAddressValue();
        Address GetDefaultBillingAddressValue();
        PaymentInfo GetDefaultPaymentInfoValue();
        void DeleteShippingAddressValue(string id);
        void DeleteBillingAddressValue(string id);
        void DeletePaymentInfoValue(string id);
        void DeleteContainer(string container);
        PaymentInfo RetrievePaymentInformation(string id);
        Address RetrieveShippingAddress(string id);
        Address RetrieveBillingAddress(string id);
    }
}