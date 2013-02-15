// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Kona.UILogic.Models;
using Kona.UILogic.Services;

namespace Kona.UILogic.Repositories
{
    public class CheckoutDataRepository : ICheckoutDataRepository
    {
        private readonly ISettingsStoreService _settingsStoreService;
        private IList<Address> _shippingAddresses;
        private IList<Address> _billingAddresses;
        private IList<PaymentMethod> _paymentMethods;

        public CheckoutDataRepository(ISettingsStoreService settingsStoreService)
        {
            _settingsStoreService = settingsStoreService;
        }

        public Address GetShippingAddress(string id)
        {
            return GetAllShippingAddresses().FirstOrDefault(s => s.Id == id);
        }

        public Address GetBillingAddress(string id)
        {
            return GetAllBillingAddresses().FirstOrDefault(b => b.Id == id);
        }

        public PaymentMethod GetPaymentMethod(string id)
        {
            return GetAllPaymentMethods().FirstOrDefault(p => p.Id == id);
        }

        public Address GetDefaultShippingAddress()
        {
            var defaultValueId = _settingsStoreService.GetValue<string>(Constants.Default, Constants.ShippingAddress);
            if (string.IsNullOrEmpty(defaultValueId)) return null;

            return GetShippingAddress(defaultValueId);
        }

        public Address GetDefaultBillingAddress()
        {
            var defaultValueId = _settingsStoreService.GetValue<string>(Constants.Default, Constants.BillingAddress);
            if (string.IsNullOrEmpty(defaultValueId)) return null;

            return GetBillingAddress(defaultValueId);
        }

        public PaymentMethod GetDefaultPaymentMethod()
        {
            var defaultValueId = _settingsStoreService.GetValue<string>(Constants.Default, Constants.PaymentMethod);
            if (string.IsNullOrEmpty(defaultValueId)) return null;

            return GetPaymentMethod(defaultValueId);
        }

        public IReadOnlyCollection<Address> GetAllShippingAddresses()
        {
            if (_shippingAddresses == null)
            {
                _shippingAddresses = _settingsStoreService.GetAllEntities<Address>(Constants.ShippingAddress).ToList();
            }

            return new ReadOnlyCollection<Address>(_shippingAddresses);
        }

        // <snippet503>
        public IReadOnlyCollection<Address> GetAllBillingAddresses()
        {
            if (_billingAddresses == null)
            {
                _billingAddresses = _settingsStoreService.GetAllEntities<Address>(Constants.BillingAddress).ToList();
            }

            return new ReadOnlyCollection<Address>(_billingAddresses);
        }
        // </snippet503>

        public IReadOnlyCollection<PaymentMethod> GetAllPaymentMethods()
        {
            if (_paymentMethods == null)
            {
                _paymentMethods = _settingsStoreService.GetAllEntities<PaymentMethod>(Constants.PaymentMethod).ToList();
            }

            return new ReadOnlyCollection<PaymentMethod>(_paymentMethods);
        }

        public Address SaveShippingAddress(Address address)
        {
            Address savedAddress = GetAllShippingAddresses().FirstOrDefault(c => IsMatchingAddress(c, address));

            if (savedAddress == null)
            {
                address.Id = Guid.NewGuid().ToString();
                _shippingAddresses.Add(address);
                _settingsStoreService.SaveEntity(Constants.ShippingAddress, address.Id, address);
                
                return address;
            }

            return savedAddress;
        }

        // <snippet502>
        public Address SaveBillingAddress(Address address)
        {
            Address savedAddress = GetAllBillingAddresses().FirstOrDefault(c => IsMatchingAddress(c, address));

            if (savedAddress == null)
            {
                address.Id = Guid.NewGuid().ToString();
                _billingAddresses.Add(address);
                _settingsStoreService.SaveEntity(Constants.BillingAddress, address.Id, address);
                
                return address;
            }

            return savedAddress;
        }
        // </snippet502>

        public PaymentMethod SavePaymentMethod(PaymentMethod paymentMethod)
        {
            PaymentMethod savedPaymentMethod = GetAllPaymentMethods().FirstOrDefault(c => IsMatchingPaymentMethod(c, paymentMethod));

            if (savedPaymentMethod == null)
            {
                paymentMethod.Id = Guid.NewGuid().ToString();
                _paymentMethods.Add(paymentMethod);
                _settingsStoreService.SaveEntity(Constants.PaymentMethod, paymentMethod.Id, paymentMethod);

                return paymentMethod;
            }

            return savedPaymentMethod;
        }

        public void SetDefaultShippingAddress(Address address)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address", "address is null");
            }

            _settingsStoreService.SaveValue<string>(Constants.Default, Constants.ShippingAddress, address.Id);
        }

        public void SetDefaultBillingAddress(Address address)
        {
            if (address == null)
            {
                throw new ArgumentNullException("address", "address is null");
            }

            _settingsStoreService.SaveValue<string>(Constants.Default, Constants.BillingAddress, address.Id);
        }

        public void SetDefaultPaymentMethod(PaymentMethod paymentMethod)
        {
            if (paymentMethod == null)
            {
                throw new ArgumentNullException("paymentMethod", "paymentMethod is null");
            }

            _settingsStoreService.SaveValue<string>(Constants.Default, Constants.PaymentMethod, paymentMethod.Id);
        }

        public void RemoveDefaultShippingAddress()
        {
            _settingsStoreService.DeleteSetting(Constants.Default, Constants.ShippingAddress);
        }

        public void RemoveDefaultBillingAddress()
        {
            _settingsStoreService.DeleteSetting(Constants.Default, Constants.BillingAddress);
        }

        public void RemoveDefaultPaymentMethod()
        {
            _settingsStoreService.DeleteSetting(Constants.Default, Constants.PaymentMethod);
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