// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.Linq;
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

        public PaymentInfo RetrievePaymentInformation(string id)
        {
            if (_paymentInfos == null)
            {
                _paymentInfos = _settingsStoreService.RetrieveAllValues<PaymentInfo>("PaymentInfo");
            }
            var paymentInfo = _paymentInfos.FirstOrDefault(p => p.Id == id);
            return paymentInfo;
        }

        public Address RetrieveShippingAddress(string id)
        {
            if (_shippingAddresses == null)
            {
                _shippingAddresses = _settingsStoreService.RetrieveAllValues<Address>("ShippingAddress");
            }
            var shippingAdress = _shippingAddresses.FirstOrDefault(s => s.Id == id);
            return shippingAdress;
        }

        // <snippet503>
        public Address RetrieveBillingAddress(string id)
        {
            if (_billingAddresses == null)
            {
               _billingAddresses = _settingsStoreService.RetrieveAllValues<Address>("BillingAddress");
            }
            var billingAddress = _billingAddresses.FirstOrDefault(b => b.Id == id);
            return billingAddress;
        }
        // </snippet503>

        public void SaveShippingAddress(Address address)
        {
            if (_shippingAddresses == null)
            {
                _shippingAddresses = _settingsStoreService.RetrieveAllValues<Address>("ShippingAddress");
            }

            _shippingAddresses.Add(address);
            _settingsStoreService.SaveValue("ShippingAddress", address);
        }

        // <snippet502>
        public void SaveBillingAddress(Address address)
        {
            if (_billingAddresses == null)
            {
                _billingAddresses = _settingsStoreService.RetrieveAllValues<Address>("BillingAddress");
            }
            
            _billingAddresses.Add(address);
            _settingsStoreService.SaveValue("BillingAddress", address);
        }
        // </snippet502>

        public void SavePaymentInfo(PaymentInfo paymentInfo)
        {
            if (_paymentInfos == null)
            {
                _paymentInfos = _settingsStoreService.RetrieveAllValues<PaymentInfo>("PaymentInfo");
            }

            _paymentInfos.Add(paymentInfo);
            _settingsStoreService.SaveValue("PaymentInfo", paymentInfo);
        }

        public bool ContainsDefaultValue(string container)
        {
            return _settingsStoreService.ContainsDefaultValue(container);
        }

        public void SetAsDefaultShippingAddress(string id)
        {
            _settingsStoreService.SetAsDefaultValue("ShippingAddress", id);
        }

        public void SetAsDefaultBillingAddress(string id)
        {
            _settingsStoreService.SetAsDefaultValue("BillingAddress", id);
        }

        public void SetAsDefaultPaymentInfo(string id)
        {
            _settingsStoreService.SetAsDefaultValue("PaymentInfo", id);
        }

        public Address GetDefaultBillingAddressValue()
        {
            return _settingsStoreService.GetDefaultValue<Address>("BillingAddress");
        }

        public Address GetDefaultShippingAddressValue()
        {
            return _settingsStoreService.GetDefaultValue<Address>("ShippingAddress");
        }

        public PaymentInfo GetDefaultPaymentInfoValue()
        {
            return _settingsStoreService.GetDefaultValue<PaymentInfo>("PaymentInfo");
        }

        public void DeleteShippingAddressValue(string id)
        {
            if (_shippingAddresses == null)
            {
                _shippingAddresses = _settingsStoreService.RetrieveAllValues<Address>("ShippingAddress");
            }

            var elementToRemove = _shippingAddresses.FirstOrDefault(p => p.Id == id);
            if (elementToRemove == null) return;
            _shippingAddresses.Remove(elementToRemove);
            _settingsStoreService.DeleteValue("ShippingAddress", id);
        }

        public void DeleteBillingAddressValue(string id)
        {
            if (_billingAddresses == null)
            {
                _billingAddresses = _settingsStoreService.RetrieveAllValues<Address>("BillingAddress");
            }

            var elementToRemove = _billingAddresses.FirstOrDefault(p => p.Id == id);
            if (elementToRemove == null) return;
            _billingAddresses.Remove(elementToRemove);
            _settingsStoreService.DeleteValue("BillingAddress", id);
        }

        public void DeletePaymentInfoValue(string id)
        {
            if (_paymentInfos == null)
            {
                _paymentInfos = _settingsStoreService.RetrieveAllValues<PaymentInfo>("PaymentInfo");
            }

            var elementToRemove = _paymentInfos.FirstOrDefault(p => p.Id == id);
            if (elementToRemove == null) return;
            _paymentInfos.Remove(elementToRemove);
            _settingsStoreService.DeleteValue("PaymentInfo", id);
        }

        public void DeleteContainer(string container)
        {
            _settingsStoreService.DeleteContainer(container);
        }
    }
}