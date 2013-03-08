// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Services;
using AdventureWorks.UILogic.Tests.Mocks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.Storage.Streams;

namespace AdventureWorks.UILogic.Tests.Repositories
{
    [TestClass]
    public class CheckoutDataRepositoryFixture
    {
        [TestMethod]
        public async Task GetEntity_Returns_Entity()
        {
            var service = SetupService();
            var encryptionService = new MockEncryptionService();
            encryptionService.DecryptMessageDelegate = buffer => Task.FromResult("1234");
            var target = new CheckoutDataRepository(service, encryptionService);

            var shippingAddress = target.GetShippingAddress("3");
            var bilingAddress = target.GetBillingAddress("2");
            var paymentMethod = await target.GetPaymentMethodAsync("1");

            Assert.AreEqual(shippingAddress.FirstName, "Anne");
            Assert.AreEqual(bilingAddress.FirstName, "Jane");
            Assert.AreEqual(paymentMethod.CardholderName, "John Doe");
        }

        [TestMethod]
        public async Task GetDefaultValues_Returns_DefaultValues()
        {
            var service = SetupService();
            var encryptionService = new MockEncryptionService();
            encryptionService.DecryptMessageDelegate = buffer => Task.FromResult("1234");
            var target = new CheckoutDataRepository(service, encryptionService);

            var defaultShippingAddress = target.GetDefaultShippingAddress();
            var defaultBilingAddress = target.GetDefaultBillingAddress();
            var defaultPaymentMethod = await target.GetDefaultPaymentMethodAsync();

            Assert.IsNotNull(defaultShippingAddress);
            Assert.AreEqual(defaultShippingAddress.Id, "3");
            Assert.IsNotNull(defaultBilingAddress);
            Assert.AreEqual(defaultBilingAddress.Id, "2");
            Assert.IsNull(defaultPaymentMethod);
        }

        [TestMethod]
        public async Task GetAllEntities_Returns_AllEntities()
        {
            var service = SetupService();
            var encryptionService = new MockEncryptionService();
            encryptionService.DecryptMessageDelegate = buffer => Task.FromResult("1234");
            var target = new CheckoutDataRepository(service, encryptionService);

            var shippingAddresses = target.GetAllShippingAddresses();
            var bilingAddresses = target.GetAllBillingAddresses();
            var paymentMethods = await target.GetAllPaymentMethodsAsync();

            Assert.AreEqual(3, shippingAddresses.Count());
            Assert.AreEqual(2, bilingAddresses.Count());
            Assert.AreEqual(1, paymentMethods.Count());
        }

        [TestMethod]
        public async Task SaveEntity_SavesEntity()
        {
            var encryptedCardNumber = false;
            var settingsStoreService = SetupService();
            var encryptionService = new MockEncryptionService
                {
                    EncryptMessageDelegate = s =>
                        {
                            Assert.AreEqual("12345", s);
                            encryptedCardNumber = true;
                            return Task.FromResult("12345");
                        },
                    DecryptMessageDelegate = buffer => Task.FromResult("12345")
                };

            var target = new CheckoutDataRepository(settingsStoreService, encryptionService);

            target.SaveShippingAddress(new Address() { Id="test", FirstName = "TestFirstName", LastName = "TestLastName" });
            target.SaveBillingAddress(new Address() { Id = "test", FirstName = "TestFirstName", LastName = "TestLastName" });
            await target.SavePaymentMethodAsync(new PaymentMethod() { Id = "test", CardNumber = "12345", CardVerificationCode = "1234", ExpirationMonth = "10", ExpirationYear = "2010", CardholderName = "TestCardholderName" });

            var savedShippingAddress = target.GetShippingAddress("test");
            var savedBillingAddress = target.GetBillingAddress("test");
            var savedPaymentMethod = await target.GetPaymentMethodAsync("test");

            Assert.IsNotNull(savedShippingAddress);
            Assert.IsNotNull(savedBillingAddress);
            Assert.IsNotNull(savedPaymentMethod);

            var shippingAddress = target.GetShippingAddress(savedShippingAddress.Id);
            var billingAddress = target.GetBillingAddress(savedBillingAddress.Id);
            var paymentMethod = await target.GetPaymentMethodAsync(savedPaymentMethod.Id);
            
            Assert.AreEqual(savedShippingAddress.Id, shippingAddress.Id);
            Assert.AreEqual(savedBillingAddress.Id, billingAddress.Id);
            Assert.AreEqual(savedPaymentMethod.Id, paymentMethod.Id);

            Assert.IsTrue(encryptedCardNumber);
        }

        [TestMethod]
        public async Task SetDefaultEntity_SetsDefaultEntity()
        {
            var service = SetupService();
            var encryptionService = new MockEncryptionService();
            encryptionService.DecryptMessageDelegate = buffer => Task.FromResult("1234");
            var target = new CheckoutDataRepository(service, encryptionService);

            var defaultShippingAddress = target.GetDefaultShippingAddress();
            var defaultBillingAddress = target.GetDefaultBillingAddress();
            var defaultPaymentMethod = await target.GetDefaultPaymentMethodAsync();

            Assert.IsNotNull(defaultShippingAddress);
            Assert.AreEqual(defaultShippingAddress.Id, "3");
            Assert.IsNotNull(defaultBillingAddress);
            Assert.AreEqual(defaultBillingAddress.Id, "2");
            Assert.IsNull(defaultPaymentMethod);

            target.SetDefaultShippingAddress("2");
            target.SetDefaultBillingAddress("1");
            target.SetDefaultPaymentMethod("1");

            defaultShippingAddress = target.GetDefaultShippingAddress();
            defaultBillingAddress = target.GetDefaultBillingAddress();
            defaultPaymentMethod = await target.GetDefaultPaymentMethodAsync();

            Assert.IsNotNull(defaultShippingAddress);
            Assert.AreEqual(defaultShippingAddress.Id, "2");
            Assert.IsNotNull(defaultBillingAddress);
            Assert.AreEqual(defaultBillingAddress.Id, "1");
            Assert.IsNotNull(defaultPaymentMethod);
            Assert.AreEqual(defaultPaymentMethod.Id, "1");
        }

        [TestMethod]
        public async Task RemoveDefaultEntity_RemovesDefaultEntity()
        {
            var service = SetupService();
            var encryptionService = new MockEncryptionService();
            encryptionService.DecryptMessageDelegate = buffer => Task.FromResult("1234");
            var target = new CheckoutDataRepository(service, encryptionService);
            target.SetDefaultPaymentMethod("1");

            var defaultShippingAddress = target.GetDefaultShippingAddress();
            var defaultBillingAddress = target.GetDefaultBillingAddress();
            var defaultPaymentMethod = await target.GetDefaultPaymentMethodAsync();

            Assert.IsNotNull(defaultShippingAddress);
            Assert.IsNotNull(defaultBillingAddress);
            Assert.IsNotNull(defaultPaymentMethod);

            target.RemoveDefaultShippingAddress();
            target.RemoveDefaultBillingAddress();
            target.RemoveDefaultPaymentMethod();

            defaultShippingAddress = target.GetDefaultShippingAddress();
            defaultBillingAddress = target.GetDefaultBillingAddress();
            defaultPaymentMethod = await target.GetDefaultPaymentMethodAsync();

            Assert.IsNull(defaultShippingAddress);
            Assert.IsNull(defaultBillingAddress);
            Assert.IsNull(defaultPaymentMethod);
        }

        private static MockSettingsStoreService SetupService()
        {
            var service = new MockSettingsStoreService();

            service.SaveEntity(Constants.ShippingAddress, "1", new Address() { Id = "1", FirstName = "Bill", MiddleInitial = "B", LastName = "Doe", City = "Redmond", State = "Washington" });
            service.SaveEntity(Constants.ShippingAddress, "2", new Address() { Id = "2", FirstName = "Jack", MiddleInitial = "B", LastName = "Doe", City = "Redmond", State = "Washington" });
            service.SaveEntity(Constants.ShippingAddress, "3", new Address() { Id = "3", FirstName = "Anne", MiddleInitial = "B", LastName = "Doe", City = "Redmond", State = "Washington" });

            service.SaveEntity(Constants.BillingAddress, "1", new Address() { Id = "1", FirstName = "John", MiddleInitial = "B", LastName = "Doe", City = "Redmond", State = "Washington" });
            service.SaveEntity(Constants.BillingAddress, "2", new Address() { Id = "2", FirstName = "Jane", MiddleInitial = "B", LastName = "Doe", City = "Redmond", State = "Washington" });

            service.SaveEntity(Constants.PaymentMethod, "1",  new PaymentMethod() { Id = "1", CardholderName = "John Doe", CardNumber = "123512523123", CardVerificationCode = "123" });

            service.SaveValue(Constants.Default, Constants.ShippingAddress, "3");
            service.SaveValue(Constants.Default, Constants.BillingAddress, "2");

            return service;
        }
    }
}
