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
        void SaveShippingAddress(Address address);
        void SaveBillingAddress(Address address);
        void SavePaymentInfo(PaymentInfo paymentInfo);
        bool ContainsDefaultValue(string container);
        void SetAsDefaultValue(string container, string id);
        T GetDefaultValue<T>(string container) where T : new();
        void DeleteValue(string container, string id);
        void DeleteContainer(string container);
    }
}