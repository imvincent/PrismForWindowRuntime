// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using Kona.UILogic.Models;
using Kona.UILogic.Services;

namespace Kona.UILogic.Repositories
{
    public class CheckoutDataRepository : ICheckoutDataRepository
    {
        private readonly ISettingsStoreService _settingsStoreService;
        private ICollection<Address> _shippingAddresses;
        private ICollection<Address> _billingAddresses;
        private ICollection<PaymentInfo> _paymentInfos;

        public CheckoutDataRepository(ISettingsStoreService settingsStoreService)
        {
            _settingsStoreService = settingsStoreService;

        }

        public ICollection<Address> RetrieveAllShippingAddresses()
        {
            if (_shippingAddresses == null)
            {
                _shippingAddresses = _settingsStoreService.RetrieveAllValues<Address>("ShippingAddress");
            }
            return _shippingAddresses;
        }

        public ICollection<Address> RetrieveAllBillingAddresses()
        {
            if (_billingAddresses == null)
            {
                _billingAddresses = _settingsStoreService.RetrieveAllValues<Address>("BillingAddress");
            }
            return _billingAddresses;
        }

        public ICollection<PaymentInfo> RetrieveAllPaymentInformation()
        {
            if (_paymentInfos == null)
            {
                _paymentInfos = _settingsStoreService.RetrieveAllValues<PaymentInfo>("PaymentInfo");
            }

            return _paymentInfos;
        }

        public void SaveShippingAddress(Address address)
        {
            if (_shippingAddresses != null)
            {
                _shippingAddresses.Add(address);
            }

            _settingsStoreService.SaveValue("ShippingAddress", address);
        }

        public void SaveBillingAddress(Address address)
        {
            if (_billingAddresses != null)
            {
                _billingAddresses.Add(address);
            }

            _settingsStoreService.SaveValue("BillingAddress", address);
        }

        public void SavePaymentInfo(PaymentInfo paymentInfo)
        {
            if (_paymentInfos != null)
            {
                _paymentInfos.Add(paymentInfo);
            }
            _settingsStoreService.SaveValue("PaymentInfo", paymentInfo);
        }

        public bool ContainsDefaultValue(string container)
        {
            return _settingsStoreService.ContainsDefaultValue(container);
        }

        public void SetAsDefaultValue(string container, string id)
        {
            _settingsStoreService.SetAsDefaultValue(container, id);
        }

        public T GetDefaultValue<T>(string container) where T : new()
        {
            return _settingsStoreService.GetDefaultValue<T>(container);
        }

        public void DeleteValue(string container, string id)
        {
            _settingsStoreService.DeleteValue(container, id);
        }

        public void DeleteContainer(string container)
        {
            _settingsStoreService.DeleteContainer(container);
        }
    }
}