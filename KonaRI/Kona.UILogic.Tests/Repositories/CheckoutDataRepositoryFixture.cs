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
        public void RetrieveAllTypesOfCheckoutData()
        {
            var service = SetupService();
            var target = new CheckoutDataRepository(service);
            var bilingAddresses = target.GetAllBillingAddresses();
            var shippingAddresses = target.GetAllShippingAddresses();
            var paymentMethods = target.GetAllPaymentMethods();
            Assert.AreEqual(3, shippingAddresses.Count());
            Assert.AreEqual(2, bilingAddresses.Count());
            Assert.AreEqual(1, paymentMethods.Count());
        }

        [TestMethod]
        public void RetrieveSpecificCheckoutData()
        {
            var service = SetupService();
            var target = new CheckoutDataRepository(service);
            var billingAddress = target.GetBillingAddress("1");
            var shippingAddress = target.GetShippingAddress("1");
            var paymentMethod = target.GetPaymentMethod("1");
            Assert.IsNotNull(billingAddress);
            Assert.IsNotNull(shippingAddress);
            Assert.IsNotNull(paymentMethod);
            Assert.AreEqual("John", billingAddress.FirstName);
            Assert.AreEqual("John", shippingAddress.FirstName);
            Assert.AreEqual("John Doe", paymentMethod.CardholderName);
        }

        [TestMethod]
        public void SaveAllTypesOfCheckoutData()
        {
            var service = SetupService();
            var target = new CheckoutDataRepository(service);
            var billingAddress = new Address(){Id = "3", FirstName = "Jack", LastName = "Doe", City = "Springfield", State = "Illinois"};
            var shippingAddress = new Address() { Id = "4", FirstName = "Jill", LastName = "Doe", City = "Bellevue", State = "Washington" };
            var paymentMethod = new PaymentMethod() {Id = "2", CardholderName = "Jill Doe"};
            target.SaveShippingAddress(shippingAddress);
            target.SaveBillingAddress(billingAddress);
            target.SavePaymentMethod(paymentMethod);
            Assert.AreEqual(3, target.GetAllBillingAddresses().Count);
            Assert.AreEqual(4, target.GetAllShippingAddresses().Count);
            Assert.AreEqual(2, target.GetAllPaymentMethods().Count);
        }

        [TestMethod]
        public void DeleteAllTypesOfCheckoutData()
        {
            var service = SetupService();
            var target = new CheckoutDataRepository(service);
            target.DeleteBillingAddressValue("1");
            target.DeleteShippingAddressValue("1");
            target.DeletePaymentMethodValue("1");
            var billingAddresses = target.GetAllBillingAddresses();
            var shippingAddresses = target.GetAllShippingAddresses();
            var paymentMethods = target.GetAllPaymentMethods();
           
            Assert.AreEqual(1, billingAddresses.Count);
            Assert.AreEqual(2, shippingAddresses.Count);
            Assert.AreEqual(0, paymentMethods.Count);
            Assert.IsNull(billingAddresses.FirstOrDefault(b => b.Id == "1"));
            Assert.IsNull(shippingAddresses.FirstOrDefault(s => s.Id == "1"));
            Assert.IsNull(paymentMethods.FirstOrDefault(p => p.Id == "1"));
        }

        [TestMethod] public void DefaultValuesForAllTypesOfCheckoutDataAreRetrievedAndSavedSuccessfully()
        {
            var service = SetupService();
            var target = new CheckoutDataRepository(service);
            var defaultBillingAddress = target.GetDefaultBillingAddressValue();
            var defaultPaymentMethod = target.GetDefaultPaymentMethodValue();
            var defaultShippingAddress = target.GetDefaultShippingAddressValue();

            var defaultPaymentMethodExists = target.GetDefaultPaymentMethodValue() != null;
            Assert.IsFalse(defaultPaymentMethodExists);
            Assert.IsNull(defaultBillingAddress);
            Assert.IsNull(defaultShippingAddress);
            Assert.IsNull(defaultPaymentMethod);

            target.SetAsDefaultShippingAddress("1");
            target.SetAsDefaultBillingAddress("1");
            target.SetAsDefaultPaymentMethod("1");
            defaultPaymentMethodExists = target.GetDefaultPaymentMethodValue() != null;
            defaultBillingAddress = target.GetDefaultBillingAddressValue();
            defaultPaymentMethod = target.GetDefaultPaymentMethodValue();
            defaultShippingAddress = target.GetDefaultShippingAddressValue();

            Assert.IsTrue(defaultPaymentMethodExists);
            Assert.IsNotNull(defaultBillingAddress);
            Assert.IsNotNull(defaultShippingAddress);
            Assert.IsNotNull(defaultPaymentMethod);
            Assert.AreEqual("John", defaultShippingAddress.FirstName);
            Assert.AreEqual("John", defaultBillingAddress.FirstName);
            Assert.AreEqual("John Doe", defaultPaymentMethod.CardholderName);
        }

        [TestMethod]
        public void SaveShippingAddress_SavesIfNew()
        {
            var service = SetupService();
            var target = new CheckoutDataRepository(service);

            var savedAddress = target.SaveShippingAddress(new Address());
            var address = target.GetShippingAddress(savedAddress.Id);

            Assert.IsNotNull(address);
        }

        [TestMethod]
        public void SaveShippingAddress_DoesNotSaveAndReturnsSavedId()
        {
            var service = SetupService();
            var target = new CheckoutDataRepository(service);

            var savedAddress = target.SaveShippingAddress(new Address
            {
                Id = "NewShippingAddressId",
                FirstName = "John",
                MiddleInitial = "S",
                LastName = "Doe",
                City = "Redmond",
                State = "Washington"
            });

            Assert.AreEqual("1", savedAddress.Id);
        }



        private static MockSettingsStoreService SetupService()
        {
            return new MockSettingsStoreService
                {
                    RetrieveAllValuesDelegate = container =>
                        {
                            if (container == "PaymentMethod")
                            {
                                return new List<object>()
                                    {
                                        new PaymentMethod()
                                            {
                                                Id = "1",
                                                CardholderName = "John Doe",
                                                CardNumber = "123512523123",
                                                CardVerificationCode = "123"
                                            },
                                    };
                            }
                            else if (container == "BillingAddress")
                            {
                                return new List<object>()
                                    {
                                        new Address()
                                            {
                                                Id = "1",
                                                FirstName = "John",
                                                MiddleInitial = "B",
                                                LastName = "Doe",
                                                City = "Redmond",
                                                State = "Washington"
                                            },
                                        new Address()
                                            {
                                                Id = "2",
                                                FirstName = "Jane",
                                                MiddleInitial = "B",
                                                LastName = "Doe",
                                                City = "Redmond",
                                                State = "Washington"
                                            },
                                    };
                            }
                            else
                            {
                                return new List<object>()
                                    {
                                        new Address()
                                            {
                                                Id = "1",
                                                FirstName = "John",
                                                MiddleInitial = "S",
                                                LastName = "Doe",
                                                City = "Redmond",
                                                State = "Washington"
                                            },
                                        new Address()
                                            {
                                                Id = "2",
                                                FirstName = "Jane",
                                                MiddleInitial = "S",
                                                LastName = "Doe",
                                                City = "Redmond",
                                                State = "Washington"
                                            },
                                        new Address()
                                            {
                                                Id = "3",
                                                FirstName = "Jean",
                                                MiddleInitial = "S",
                                                LastName = "Doe",
                                                City = "Redmond",
                                                State = "Washington"
                                            },
                                    };
                            }
                        }
                };
        }
    }
}
