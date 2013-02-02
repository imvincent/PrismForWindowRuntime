// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
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
        private ICollection<PaymentMethod> _paymentMethods;

        public CheckoutDataRepository(ISettingsStoreService settingsStoreService)
        {
            _settingsStoreService = settingsStoreService;
        }

        public Address GetShippingAddress(string id)
        {
            if (_shippingAddresses == null)
            {
                _shippingAddresses = _settingsStoreService.RetrieveAllValues<Address>("ShippingAddress");
            }
            var shippingAdress = _shippingAddresses.FirstOrDefault(s => s.Id == id);
            return shippingAdress;
        }

        // <snippet503>
        public Address GetBillingAddress(string id)
        {
            if (_billingAddresses == null)
            {
                _billingAddresses = _settingsStoreService.RetrieveAllValues<Address>("BillingAddress");
            }
            var billingAddress = _billingAddresses.FirstOrDefault(b => b.Id == id);
            return billingAddress;
        }
        // </snippet503>

        public PaymentMethod GetPaymentMethod(string id)
        {
            if (_paymentMethods == null)
            {
                _paymentMethods = _settingsStoreService.RetrieveAllValues<PaymentMethod>("PaymentMethod");
            }
            var paymentMethod = _paymentMethods.FirstOrDefault(p => p.Id == id);
            return paymentMethod;
        }

        public ICollection<Address> GetAllShippingAddresses()
        {
            if (_shippingAddresses == null)
            {
                _shippingAddresses = _settingsStoreService.RetrieveAllValues<Address>("ShippingAddress");
            }
            return _shippingAddresses;
        }

        public ICollection<Address> GetAllBillingAddresses()
        {
            if (_billingAddresses == null)
            {
               _billingAddresses = _settingsStoreService.RetrieveAllValues<Address>("BillingAddress");
            }
            return _billingAddresses;
        }

        public ICollection<PaymentMethod> GetAllPaymentMethods()
        {
            if (_paymentMethods == null)
            {
                _paymentMethods = _settingsStoreService.RetrieveAllValues<PaymentMethod>("PaymentMethod");
            }

            return _paymentMethods;
        }

        public Address GetDefaultShippingAddressValue()
        {
            return _settingsStoreService.GetDefaultValue<Address>("ShippingAddress");
        }

        public Address GetDefaultBillingAddressValue()
        {
            return _settingsStoreService.GetDefaultValue<Address>("BillingAddress");
        }

        public PaymentMethod GetDefaultPaymentMethodValue()
        {
            return _settingsStoreService.GetDefaultValue<PaymentMethod>("PaymentMethod");
        }

        public Address SaveShippingAddress(Address address)
        {
            if (_shippingAddresses == null)
            {
                _shippingAddresses = _settingsStoreService.RetrieveAllValues<Address>("ShippingAddress");
            }

            Address savedAddress = savedAddress = _shippingAddresses.FirstOrDefault(c => IsMatchingAddress(c, address));
            if (savedAddress != null) return savedAddress;

            address.Id = Guid.NewGuid().ToString();
            _shippingAddresses.Add(address);
            _settingsStoreService.SaveValue("ShippingAddress", address);
            return address;
        }

        // <snippet502>
        public Address SaveBillingAddress(Address address)
        {
            if (_billingAddresses == null)
            {
                _billingAddresses = _settingsStoreService.RetrieveAllValues<Address>("BillingAddress");
            }

            Address savedAddress = savedAddress = _billingAddresses.FirstOrDefault(c => IsMatchingAddress(c, address));
            if (savedAddress != null) return savedAddress;

            address.Id = Guid.NewGuid().ToString();
            _billingAddresses.Add(address);
            _settingsStoreService.SaveValue("BillingAddress", address);
            return address;
        }
        // </snippet502>

        public PaymentMethod SavePaymentMethod(PaymentMethod paymentMethod)
        {
            if (_paymentMethods == null)
            {
                _paymentMethods = _settingsStoreService.RetrieveAllValues<PaymentMethod>("PaymentMethod");
            }

            PaymentMethod savedPaymentMethod = _paymentMethods.FirstOrDefault(c => IsMatchingPaymentMethod(c, paymentMethod));
            if (savedPaymentMethod != null) return savedPaymentMethod;

            paymentMethod.Id = Guid.NewGuid().ToString();
            _paymentMethods.Add(paymentMethod);
            _settingsStoreService.SaveValue("PaymentMethod", paymentMethod);
            return paymentMethod;
        }

        public void SetAsDefaultShippingAddress(string id)
        {
            _settingsStoreService.SetAsDefaultValue("ShippingAddress", id);
        }

        public void SetAsDefaultBillingAddress(string id)
        {
            _settingsStoreService.SetAsDefaultValue("BillingAddress", id);
        }

        public void SetAsDefaultPaymentMethod(string id)
        {
            _settingsStoreService.SetAsDefaultValue("PaymentMethod", id);
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

        public void DeletePaymentMethodValue(string id)
        {
            if (_paymentMethods == null)
            {
                _paymentMethods = _settingsStoreService.RetrieveAllValues<PaymentMethod>("PaymentMethod");
            }

            var elementToRemove = _paymentMethods.FirstOrDefault(p => p.Id == id);
            if (elementToRemove == null) return;
            _paymentMethods.Remove(elementToRemove);
            _settingsStoreService.DeleteValue("PaymentMethod", id);
        }

        private static bool IsMatchingAddress(Address firstAddress, Address secondAddress)
        {
            if (firstAddress.FirstName != secondAddress.FirstName) return false;
            if (firstAddress.MiddleInitial != secondAddress.MiddleInitial) return false;
            if (firstAddress.LastName != secondAddress.LastName) return false;
            if (firstAddress.StreetAddress != secondAddress.StreetAddress) return false;
            if (firstAddress.OptionalAddress != secondAddress.OptionalAddress) return false;
            if (firstAddress.City != secondAddress.City) return false;
            if (firstAddress.State != secondAddress.State) return false;
            if (firstAddress.ZipCode != secondAddress.ZipCode) return false;
            if (firstAddress.Phone != secondAddress.Phone) return false;

            return true;
        }

        private static bool IsMatchingPaymentMethod(PaymentMethod firstPaymentMethod, PaymentMethod secondPaymentMethod)
        {
            if (firstPaymentMethod.CardNumber != secondPaymentMethod.CardNumber) return false;
            if (firstPaymentMethod.CardVerificationCode != secondPaymentMethod.CardVerificationCode) return false;
            if (firstPaymentMethod.CardholderName != secondPaymentMethod.CardholderName) return false;
            if (firstPaymentMethod.ExpirationMonth != secondPaymentMethod.ExpirationMonth) return false;
            if (firstPaymentMethod.ExpirationYear != secondPaymentMethod.ExpirationYear) return false;
            if (firstPaymentMethod.Phone != secondPaymentMethod.Phone) return false;

            return true;
        }
    }
}