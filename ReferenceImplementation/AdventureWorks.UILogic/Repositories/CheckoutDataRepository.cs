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
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Services;

namespace AdventureWorks.UILogic.Repositories
{
    public class CheckoutDataRepository : ICheckoutDataRepository
    {
        private readonly ISettingsStoreService _settingsStoreService;
        private readonly IEncryptionService _encryptionService;

        public CheckoutDataRepository(ISettingsStoreService settingsStoreService, IEncryptionService encryptionService)
        {
            _settingsStoreService = settingsStoreService;
            _encryptionService = encryptionService;
        }

        public Address GetShippingAddress(string id)
        {
            return GetAllShippingAddresses().FirstOrDefault(s => s.Id == id);
        }

        public Address GetBillingAddress(string id)
        {
            return GetAllBillingAddresses().FirstOrDefault(b => b.Id == id);
        }

        public async Task<PaymentMethod> GetPaymentMethodAsync(string id)
        {
            return (await GetAllPaymentMethodsAsync()).FirstOrDefault(p => p.Id == id);
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

        public async Task<PaymentMethod> GetDefaultPaymentMethodAsync()
        {
            var defaultValueId = _settingsStoreService.GetValue<string>(Constants.Default, Constants.PaymentMethod);
            if (string.IsNullOrEmpty(defaultValueId)) return null;

            return await GetPaymentMethodAsync(defaultValueId);
        }

        public IReadOnlyCollection<Address> GetAllShippingAddresses()
        {
            var shippingAddresses = _settingsStoreService.GetAllEntities<Address>(Constants.ShippingAddress);
            return new ReadOnlyCollection<Address>(shippingAddresses.ToList());
        }

        // <snippet503>
        public IReadOnlyCollection<Address> GetAllBillingAddresses()
        {
            var billingAddresses = _settingsStoreService.GetAllEntities<Address>(Constants.BillingAddress).ToList();
            return new ReadOnlyCollection<Address>(billingAddresses.ToList());
        }
        // </snippet503>

        public async Task<IReadOnlyCollection<PaymentMethod>> GetAllPaymentMethodsAsync()
        {
            var paymentMethods = _settingsStoreService.GetAllEntities<PaymentMethod>(Constants.PaymentMethod);
            try
            {
                foreach (var paymentMethod in paymentMethods)
                {
                    // Decrypt the Card number
                    paymentMethod.CardNumber = await _encryptionService.DecryptMessage(paymentMethod.CardNumber);
                }
            }
            catch (Exception)
            {
                //Problem decrypting payment method. Remove from settings store
                _settingsStoreService.DeleteContainer(Constants.PaymentMethod);
            }

                    return new ReadOnlyCollection<PaymentMethod>(paymentMethods.ToList());
        }

        public void SaveShippingAddress(Address address)
        {
            if (address == null) throw new ArgumentNullException("address");

            address.Id = address.Id ?? Guid.NewGuid().ToString();

            // Save the value in the Settings store 
            _settingsStoreService.SaveEntity(Constants.ShippingAddress, address.Id, address);

            // If there's no default value stored, use this one
            if (GetDefaultShippingAddress() == null)
            {
                SetDefaultShippingAddress(address.Id);
            }
        }

        // <snippet502>
        public void SaveBillingAddress(Address address)
        {
            if (address == null) throw new ArgumentNullException("address");

            address.Id = address.Id ?? Guid.NewGuid().ToString();

            // Save the value in the Settings store 
            _settingsStoreService.SaveEntity(Constants.BillingAddress, address.Id, address);

            // If there's no default value stored, use this one
            if (GetDefaultBillingAddress() == null)
            {
                SetDefaultBillingAddress(address.Id);
            }
        }
        // </snippet502>

        public async Task SavePaymentMethodAsync(PaymentMethod paymentMethod)
        {
            if (paymentMethod == null) throw new ArgumentNullException("paymentMethod");

            paymentMethod.Id = paymentMethod.Id ?? Guid.NewGuid().ToString();

            // Create a new instance with sensitive data encrypted
            var paymentMethodToSave = new PaymentMethod()
                {
                    Id = paymentMethod.Id,
                    CardNumber = await _encryptionService.EncryptMessage(paymentMethod.CardNumber),
                    CardholderName = paymentMethod.CardholderName,
                    ExpirationMonth = paymentMethod.ExpirationMonth,
                    ExpirationYear = paymentMethod.ExpirationYear,
                    Phone = paymentMethod.Phone,
                    CardVerificationCode = paymentMethod.CardVerificationCode
                };

            // Save the value in the Settings store 
            _settingsStoreService.SaveEntity(Constants.PaymentMethod, paymentMethodToSave.Id, paymentMethodToSave);

            // If there's no default value stored, use this one
            if (await GetDefaultPaymentMethodAsync() == null)
            {
                SetDefaultPaymentMethod(paymentMethodToSave.Id);
            }
        }

        public void SetDefaultShippingAddress(string addressId)
        {
            if (string.IsNullOrWhiteSpace(addressId)) throw new ArgumentNullException("addressId", "addressId is null");
            
            _settingsStoreService.SaveValue(Constants.Default, Constants.ShippingAddress, addressId);
        }

        public void SetDefaultBillingAddress(string addressId)
        {
            if (string.IsNullOrWhiteSpace(addressId)) throw new ArgumentNullException("addressId", "addressId is null");

            _settingsStoreService.SaveValue(Constants.Default, Constants.BillingAddress, addressId);
        }

        public void SetDefaultPaymentMethod(string paymentMethodId)
        {
            if (string.IsNullOrWhiteSpace(paymentMethodId)) throw new ArgumentNullException("paymentMethodId", "paymentMethodId is null");

            _settingsStoreService.SaveValue(Constants.Default, Constants.PaymentMethod, paymentMethodId);
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
    }
}