// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.Repositories
{
    public interface ICheckoutDataRepository
    {
        Address GetShippingAddress(string id);
        Address GetBillingAddress(string id);
        Task<PaymentMethod> GetPaymentMethodAsync(string id);

        Address GetDefaultShippingAddress();
        Address GetDefaultBillingAddress();
        Task<PaymentMethod> GetDefaultPaymentMethodAsync();

        IReadOnlyCollection<Address> GetAllShippingAddresses();
        IReadOnlyCollection<Address> GetAllBillingAddresses();
        Task<IReadOnlyCollection<PaymentMethod>> GetAllPaymentMethodsAsync();

        void SaveShippingAddress(Address address);
        void SaveBillingAddress(Address address);
        Task SavePaymentMethodAsync(PaymentMethod paymentMethod);

        void SetDefaultShippingAddress(string addressId);
        void SetDefaultBillingAddress(string addressId);
        void SetDefaultPaymentMethod(string paymentMethodId);

        void RemoveDefaultShippingAddress();
        void RemoveDefaultBillingAddress();
        void RemoveDefaultPaymentMethod();
    }
}