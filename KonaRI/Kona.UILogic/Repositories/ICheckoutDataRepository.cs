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
        Address GetShippingAddress(string id);
        Address GetBillingAddress(string id);
        PaymentMethod GetPaymentMethod(string id);

        ICollection<Address> GetAllShippingAddresses();
        ICollection<Address> GetAllBillingAddresses();
        ICollection<PaymentMethod> GetAllPaymentMethods();

        Address GetDefaultShippingAddressValue();
        Address GetDefaultBillingAddressValue();
        PaymentMethod GetDefaultPaymentMethodValue();

        Address SaveShippingAddress(Address address);
        Address SaveBillingAddress(Address address);
        PaymentMethod SavePaymentMethod(PaymentMethod paymentMethod);
        
        void SetAsDefaultShippingAddress(string id);
        void SetAsDefaultBillingAddress(string id);
        void SetAsDefaultPaymentMethod(string id);
        
        void DeleteShippingAddressValue(string id);
        void DeleteBillingAddressValue(string id);
        void DeletePaymentMethodValue(string id);
    }
}