// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Kona.UILogic.Services;
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
            //bool shippingVMValidationExecuted = false, billingVMValidationExecuted = false, paymentVMValidationExecuted = false;
            //var shippingAddressPageVM = new MockShippingAddressPageViewModel();
            //shippingAddressPageVM.ValidateFormDelegate = () => { shippingVMValidationExecuted = true; return false; };
            //var billingAddressPageVM = new MockBillingAddressPageViewModel();
            //billingAddressPageVM.ValidateFormDelegate = () => { billingVMValidationExecuted = true; return false; };
            //var paymentMethodPageVM = new MockPaymentMethodPageViewModel();
            //paymentMethodPageVM.ValidateFormDelegate = () => { paymentVMValidationExecuted = true; return false; };
            //var target = new CheckoutHubPageViewModel(new MockNavigationService(),
            //                                          new OrderServiceProxy(), 
            //                                          new MockShoppingCartRepository(),
            //                                          shippingAddressPageVM, 
            //                                          billingAddressPageVM,
            //                                          paymentMethodPageVM);

            //target.GoNextCommand.Execute();
            
            //Assert.IsTrue(shippingVMValidationExecuted);
            //Assert.IsTrue(billingVMValidationExecuted);
            //Assert.IsTrue(paymentVMValidationExecuted);
        }

        [TestMethod]
        public void ExecuteGoNextCommand_Processes3Forms_IfViewModelsValid()
        {
            //bool shippingVMProcessFormExecuted = false, billingVMProcessFormExecuted = false, paymentVMProcessFormExecuted = false;
            //var navigationService = new MockNavigationService();
            //navigationService.NavigateDelegate = (s, o) => true;
            //var shippingAddressPageVM = new MockShippingAddressPageViewModel();
            //shippingAddressPageVM.ValidateFormDelegate = () => true;
            //shippingAddressPageVM.ProcessFormDelegate = () => { shippingVMProcessFormExecuted = true; };
            //var billingAddressPageVM = new MockBillingAddressPageViewModel();
            //billingAddressPageVM.ValidateFormDelegate = () => true; ;
            //billingAddressPageVM.ProcessFormDelegate = () => { billingVMProcessFormExecuted = true; };
            //var paymentMethodPageVM = new MockPaymentMethodPageViewModel();
            //paymentMethodPageVM.ValidateFormDelegate = () => true;
            //paymentMethodPageVM.ProcessFormDelegate = () => { paymentVMProcessFormExecuted = true; };
            //var target = new CheckoutHubPageViewModel(navigationService,
            //                                          new MockShoppingCartRepository(), 
            //                                          shippingAddressPageVM,
            //                                          billingAddressPageVM,
            //                                          paymentMethodPageVM);

            //target.GoNextCommand.Execute();

            //Assert.IsTrue(shippingVMProcessFormExecuted);
            //Assert.IsTrue(billingVMProcessFormExecuted);
            //Assert.IsTrue(paymentVMProcessFormExecuted);
        }

        [TestMethod]
        public void SettingUseShippingAddressToTrue_CopiesValuesFromShippingAddressToBilling()
        {
            //var navigationService = new MockNavigationService();
            //navigationService.NavigateDelegate = (s, o) => true;
            //var shippingAddressPageVM = new MockShippingAddressPageViewModel();
            //shippingAddressPageVM.ValidateFormDelegate = () => true;
            //shippingAddressPageVM.ProcessFormDelegate = () => { };
            //var billingAddressPageVM = new MockBillingAddressPageViewModel();
            //var paymentMethodPageVM = new MockPaymentMethodPageViewModel();
            //paymentMethodPageVM.ValidateFormDelegate = () => true;
            //paymentMethodPageVM.ProcessFormDelegate = () => { };
            //var shoppingCartRepository = new MockShoppingCartRepository();
            //Address billingAddressParam = null;
            //shoppingCartRepository.AddAddressAndPurchaseInfoDelegate = (shippingAddress, billingAddress, paymentMethod) =>
            //                                                               {
            //                                                                   billingAddressParam = billingAddress;
            //                                                               };
            //var target = new CheckoutHubPageViewModel(navigationService,
            //                                          shoppingCartRepository, 
            //                                          shippingAddressPageVM,
            //                                          billingAddressPageVM,
            //                                          paymentMethodPageVM);

            //shippingAddressPageVM.Address = new Address
            //                                    {
            //                                        FirstName = "TestFirstName",
            //                                        LastName = "TestLastName",
            //                                        ZipCode = "TestZipCode"
            //                                    };

            //target.UseSameAddressAsShipping = true;

            //Assert.IsFalse(billingAddressPageVM.IsEnabled);

            //target.GoNextCommand.Execute();

            //Assert.AreEqual("TestFirstName", billingAddressParam.FirstName);
            //Assert.AreEqual("TestLastName", billingAddressParam.LastName);
            //Assert.AreEqual("TestZipCode", billingAddressParam.ZipCode);
        }
    }
}
