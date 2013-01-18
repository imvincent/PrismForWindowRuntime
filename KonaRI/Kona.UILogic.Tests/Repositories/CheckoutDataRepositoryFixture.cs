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
            var bilingAddresses = target.RetrieveAllBillingAddresses();
            var shippingAddresses = target.RetrieveAllShippingAddresses();
            var paymentInfos = target.RetrieveAllPaymentInformation();
            Assert.AreEqual(3, shippingAddresses.Count());
            Assert.AreEqual(2, bilingAddresses.Count());
            Assert.AreEqual(1, paymentInfos.Count());
        }

        [TestMethod]
        public void RetrieveSpecificCheckoutData()
        {
            var service = SetupService();
            var target = new CheckoutDataRepository(service);
            var billingAddress = target.RetrieveBillingAddress("1");
            var shippingAddress = target.RetrieveShippingAddress("1");
            var paymentInfo = target.RetrievePaymentInformation("1");
            Assert.IsNotNull(billingAddress);
            Assert.IsNotNull(shippingAddress);
            Assert.IsNotNull(paymentInfo);
            Assert.AreEqual("John", billingAddress.FirstName);
            Assert.AreEqual("John", shippingAddress.FirstName);
            Assert.AreEqual("John Doe", paymentInfo.CardholderName);
        }

        [TestMethod]
        public void SaveAllTypesOfCheckoutData()
        {
            var service = SetupService();
            var target = new CheckoutDataRepository(service);
            var billingAddress = new Address(){Id = "3", FirstName = "Jack", LastName = "Doe", City = "Springfield", State = "Illinois"};
            var shippingAddress = new Address() { Id = "4", FirstName = "Jill", LastName = "Doe", City = "Bellevue", State = "Washington" };
            var paymentInfo = new PaymentInfo() {Id = "2", CardholderName = "Jill Doe"};
            target.SaveShippingAddress(shippingAddress);
            target.SaveBillingAddress(billingAddress);
            target.SavePaymentInfo(paymentInfo);
            Assert.AreEqual(3, target.RetrieveAllBillingAddresses().Count);
            Assert.AreEqual(4, target.RetrieveAllShippingAddresses().Count);
            Assert.AreEqual(2, target.RetrieveAllPaymentInformation().Count);
        }

        [TestMethod]
        public void DeleteAllTypesOfCheckoutData()
        {
            var service = SetupService();
            var target = new CheckoutDataRepository(service);
            target.DeleteBillingAddressValue("1");
            target.DeleteShippingAddressValue("1");
            target.DeletePaymentInfoValue("1");
            var billingAddresses = target.RetrieveAllBillingAddresses();
            var shippingAddresses = target.RetrieveAllShippingAddresses();
            var paymentInfos = target.RetrieveAllPaymentInformation();
           
            Assert.AreEqual(1, billingAddresses.Count);
            Assert.AreEqual(2, shippingAddresses.Count);
            Assert.AreEqual(0, paymentInfos.Count);
            Assert.IsNull(billingAddresses.FirstOrDefault(b => b.Id == "1"));
            Assert.IsNull(shippingAddresses.FirstOrDefault(s => s.Id == "1"));
            Assert.IsNull(paymentInfos.FirstOrDefault(p => p.Id == "1"));
        }

        [TestMethod] public void DefaultValuesForAllTypesOfCheckoutDataAreRetrievedAndSavedSuccessfully()
        {
            var service = SetupService();
            var target = new CheckoutDataRepository(service);
            var defaultBillingAddress = target.GetDefaultBillingAddressValue();
            var defaultPaymentInfo = target.GetDefaultPaymentInfoValue();
            var defaultShippingAddress = target.GetDefaultShippingAddressValue();

            var isDefaultPaymentInfo = target.ContainsDefaultValue("PaymentInfo");
            Assert.IsFalse(isDefaultPaymentInfo);
            Assert.IsNull(defaultBillingAddress);
            Assert.IsNull(defaultShippingAddress);
            Assert.IsNull(defaultPaymentInfo);

            target.SetAsDefaultShippingAddress("1");
            target.SetAsDefaultBillingAddress("1");
            target.SetAsDefaultPaymentInfo("1");
            isDefaultPaymentInfo = target.ContainsDefaultValue("PaymentInfo");
            defaultBillingAddress = target.GetDefaultBillingAddressValue();
            defaultPaymentInfo = target.GetDefaultPaymentInfoValue();
            defaultShippingAddress = target.GetDefaultShippingAddressValue();

            Assert.IsTrue(isDefaultPaymentInfo);
            Assert.IsNotNull(defaultBillingAddress);
            Assert.IsNotNull(defaultShippingAddress);
            Assert.IsNotNull(defaultPaymentInfo);
            Assert.AreEqual("John", defaultShippingAddress.FirstName);
            Assert.AreEqual("John", defaultBillingAddress.FirstName);
            Assert.AreEqual("John Doe", defaultPaymentInfo.CardholderName);
        }



        private static MockSettingsStoreService SetupService()
        {
            return new MockSettingsStoreService
                {
                    RetrieveAllValuesDelegate = container =>
                        {
                            if (container == "PaymentInfo")
                            {
                                return new List<object>()
                                    {
                                        new PaymentInfo()
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
