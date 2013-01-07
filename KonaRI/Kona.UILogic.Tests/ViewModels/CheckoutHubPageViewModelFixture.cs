// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Kona.UILogic.Tests.Mocks;
using Kona.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
namespace Kona.UILogic.Tests.ViewModels
{
    [TestClass]
    public class CheckoutHubPageViewModelFixture
    {
        [TestMethod]
        public void ExecuteGoNextCommand_Validates3ViewModels()
        {
            bool shippingVMValidationExecuted = false, billingVMValidationExecuted = false, paymentVMValidationExecuted = false;
            var shippingAddressPageVM = new MockShippingAddressPageViewModel();
            shippingAddressPageVM.ValidateFormAsyncDelegate = () => { shippingVMValidationExecuted = true; return Task.FromResult(false); };
            var billingAddressPageVM = new MockBillingAddressPageViewModel();
            billingAddressPageVM.ValidateFormAsyncDelegate = () => { billingVMValidationExecuted = true; return Task.FromResult(false); };
            var paymentMethodPageVM = new MockPaymentMethodPageViewModel();
            paymentMethodPageVM.ValidateFormAsyncDelegate = () => { paymentVMValidationExecuted = true; return Task.FromResult(false); };
            var target = new CheckoutHubPageViewModel(new MockNavigationService(),
                                                      new MockShoppingCartRepository(),
                                                      shippingAddressPageVM, 
                                                      billingAddressPageVM,
                                                      paymentMethodPageVM);

            target.GoNextCommand.Execute();
            
            Assert.IsTrue(shippingVMValidationExecuted);
            Assert.IsTrue(billingVMValidationExecuted);
            Assert.IsTrue(paymentVMValidationExecuted);
        }

        [TestMethod]
        public void ExecuteGoNextCommand_Processes3Forms_IfViewModelsValid()
        {
            bool shippingVMProcessFormExecuted = false, billingVMProcessFormExecuted = false, paymentVMProcessFormExecuted = false;
            var navigationService = new MockNavigationService();
            navigationService.NavigateDelegate = (s, o) => true;
            var shippingAddressPageVM = new MockShippingAddressPageViewModel();
            shippingAddressPageVM.ValidateFormAsyncDelegate = () => Task.FromResult(true);
            shippingAddressPageVM.ProcessFormDelegate = () => { shippingVMProcessFormExecuted = true; };
            var billingAddressPageVM = new MockBillingAddressPageViewModel();
            billingAddressPageVM.ValidateFormAsyncDelegate = () => Task.FromResult(true); ;
            billingAddressPageVM.ProcessFormDelegate = () => { billingVMProcessFormExecuted = true; };
            var paymentMethodPageVM = new MockPaymentMethodPageViewModel();
            paymentMethodPageVM.ValidateFormAsyncDelegate = () => Task.FromResult(true); ;
            paymentMethodPageVM.ProcessFormDelegate = () => { paymentVMProcessFormExecuted = true; };
            var target = new CheckoutHubPageViewModel(navigationService,
                                                      new MockShoppingCartRepository(), 
                                                      shippingAddressPageVM,
                                                      billingAddressPageVM,
                                                      paymentMethodPageVM);

            target.GoNextCommand.Execute();

            Assert.IsTrue(shippingVMProcessFormExecuted);
            Assert.IsTrue(billingVMProcessFormExecuted);
            Assert.IsTrue(paymentVMProcessFormExecuted);
        }

        [TestMethod]
        public void ExecuteUseShippingAddressCommand_CopiesValuesFromShippingAddressToBilling()
        {
            var navigationService = new MockNavigationService();
            var shippingAddressPageVM = new MockShippingAddressPageViewModel();
            var billingAddressPageVM = new MockBillingAddressPageViewModel();
            var paymentMethodPageVM = new MockPaymentMethodPageViewModel();
            var target = new CheckoutHubPageViewModel(navigationService,
                                                      new MockShoppingCartRepository(), 
                                                      shippingAddressPageVM,
                                                      billingAddressPageVM,
                                                      paymentMethodPageVM);

            shippingAddressPageVM.Address = new Address
                                                {
                                                    FirstName = "TestFirstName",
                                                    LastName = "TestLastName",
                                                    ZipCode = "TestZipCode"
                                                };

            target.UseSameAsShippingAddressAction(null);

            Assert.AreEqual("TestFirstName", billingAddressPageVM.FirstName);
            Assert.AreEqual("TestLastName", billingAddressPageVM.LastName);
            Assert.AreEqual("TestZipCode", billingAddressPageVM.ZipCode);
        }
    }
}
