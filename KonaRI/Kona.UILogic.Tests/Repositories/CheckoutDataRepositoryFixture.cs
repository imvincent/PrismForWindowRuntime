// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.Linq;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Kona.UILogic.Tests.Mocks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Kona.UILogic.Tests.Repositories
{
    [TestClass]
    public class CheckoutDataRepositoryFixture
    {
        [TestMethod]
        public void GetEntity_Returns_Entity()
        {
            var service = SetupService();
            var target = new CheckoutDataRepository(service);

            var shippingAddress = target.GetShippingAddress("3");
            var bilingAddress = target.GetBillingAddress("2");
            var paymentMethod = target.GetPaymentMethod("1");

            Assert.AreEqual(shippingAddress.FirstName, "Anne");
            Assert.AreEqual(bilingAddress.FirstName, "Jane");
            Assert.AreEqual(paymentMethod.CardholderName, "John Doe");
        }

        [TestMethod]
        public void GetDefaultValues_Returns_DefaultValues()
        {
            var service = SetupService();
            var target = new CheckoutDataRepository(service);

            var defaultShippingAddress = target.GetDefaultShippingAddress();
            var defaultBilingAddress = target.GetDefaultBillingAddress();
            var defaultPaymentMethod = target.GetDefaultPaymentMethod();

            Assert.IsNotNull(defaultShippingAddress);
            Assert.AreEqual(defaultShippingAddress.Id, "3");
            Assert.IsNotNull(defaultBilingAddress);
            Assert.AreEqual(defaultBilingAddress.Id, "2");
            Assert.IsNull(defaultPaymentMethod);
        }

        [TestMethod]
        public void GetAllEntities_Returns_AllEntities()
        {
            var service = SetupService();
            var target = new CheckoutDataRepository(service);

            var shippingAddresses = target.GetAllShippingAddresses();
            var bilingAddresses = target.GetAllBillingAddresses();
            var paymentMethods = target.GetAllPaymentMethods();

            Assert.AreEqual(3, shippingAddresses.Count());
            Assert.AreEqual(2, bilingAddresses.Count());
            Assert.AreEqual(1, paymentMethods.Count());
        }

        [TestMethod]
        public void SaveEntity_SavesEntity()
        {
            var service = SetupService();
            var target = new CheckoutDataRepository(service);

            var savedShippingAddress = target.SaveShippingAddress(new Address() { FirstName = "TestFirstName", LastName = "TestLastName" });
            var savedBillingAddress = target.SaveBillingAddress(new Address() { FirstName = "TestFirstName", LastName = "TestLastName" });
            var savedPaymentMethod = target.SavePaymentMethod(new PaymentMethod() { CardholderName = "TestCardholderName" });

            Assert.IsNotNull(savedShippingAddress);
            Assert.IsNotNull(savedBillingAddress);
            Assert.IsNotNull(savedPaymentMethod);

            var shippingAddress = target.GetShippingAddress(savedShippingAddress.Id);
            var billingAddress = target.GetBillingAddress(savedBillingAddress.Id);
            var paymentMethod = target.GetPaymentMethod(savedPaymentMethod.Id);
            
            Assert.AreEqual(savedShippingAddress.Id, shippingAddress.Id);
            Assert.AreEqual(savedBillingAddress.Id, billingAddress.Id);
            Assert.AreEqual(savedPaymentMethod.Id, paymentMethod.Id);
        }

        [TestMethod]
        public void SetDefaultEntity_SetsDefaultEntity()
        {
            var service = SetupService();
            var target = new CheckoutDataRepository(service);

            var defaultShippingAddress = target.GetDefaultShippingAddress();
            var defaultBillingAddress = target.GetDefaultBillingAddress();
            var defaultPaymentMethod = target.GetDefaultPaymentMethod();

            Assert.IsNotNull(defaultShippingAddress);
            Assert.AreEqual(defaultShippingAddress.Id, "3");
            Assert.IsNotNull(defaultBillingAddress);
            Assert.AreEqual(defaultBillingAddress.Id, "2");
            Assert.IsNull(defaultPaymentMethod);

            target.SetDefaultShippingAddress(target.GetShippingAddress("2"));
            target.SetDefaultBillingAddress(target.GetBillingAddress("1"));
            target.SetDefaultPaymentMethod(target.GetPaymentMethod("1"));

            defaultShippingAddress = target.GetDefaultShippingAddress();
            defaultBillingAddress = target.GetDefaultBillingAddress();
            defaultPaymentMethod = target.GetDefaultPaymentMethod();

            Assert.IsNotNull(defaultShippingAddress);
            Assert.AreEqual(defaultShippingAddress.Id, "2");
            Assert.IsNotNull(defaultBillingAddress);
            Assert.AreEqual(defaultBillingAddress.Id, "1");
            Assert.IsNotNull(defaultPaymentMethod);
            Assert.AreEqual(defaultPaymentMethod.Id, "1");
        }

        [TestMethod]
        public void RemoveDefaultEntity_RemovesDefaultEntity()
        {
            var service = SetupService();
            var target = new CheckoutDataRepository(service);
            target.SetDefaultPaymentMethod(target.GetPaymentMethod("1"));

            var defaultShippingAddress = target.GetDefaultShippingAddress();
            var defaultBillingAddress = target.GetDefaultBillingAddress();
            var defaultPaymentMethod = target.GetDefaultPaymentMethod();

            Assert.IsNotNull(defaultShippingAddress);
            Assert.IsNotNull(defaultBillingAddress);
            Assert.IsNotNull(defaultPaymentMethod);

            target.RemoveDefaultShippingAddress();
            target.RemoveDefaultBillingAddress();
            target.RemoveDefaultPaymentMethod();

            defaultShippingAddress = target.GetDefaultShippingAddress();
            defaultBillingAddress = target.GetDefaultBillingAddress();
            defaultPaymentMethod = target.GetDefaultPaymentMethod();

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
