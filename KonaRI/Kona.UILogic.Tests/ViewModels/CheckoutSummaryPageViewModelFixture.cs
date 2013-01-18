// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.Tests.Mocks;
using Kona.UILogic.ViewModels;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.UI.Xaml.Navigation;

namespace Kona.UILogic.Tests.ViewModels
{
    [TestClass]
    public class CheckoutSummaryPageViewModelFixture
    {
        [TestMethod]
        public void UnselectingShippingMethod_CalculatesTotalCostWithZeroShipping()
        {
            //var shoppingCartRepository = new MockShoppingCartRepository();
            //shoppingCartRepository.GetShoppingCartAsyncDelegate = () =>
            //                                                          {
            //                                                              var shoppingCartItems =
            //                                                                  new List<ShoppingCartItem>
            //                                                                      {
            //                                                                          new ShoppingCartItem
            //                                                                              {
            //                                                                                  Quantity = 1,
            //                                                                                  Currency = "USD",
            //                                                                                  Product = new Product()
            //                                                                              }
            //                                                                      };
            //                                                              var shoppingCart =
            //                                                                  new ShoppingCart(shoppingCartItems){Currency = "USD", FullPrice = 100};
            //                                                              return Task.FromResult(shoppingCart);
            //                                                          };
            //shoppingCartRepository.AddressAndPurchaseInfo = new AddressAndPaymentInfo
            //                                                    {
            //                                                        BillingAddress = new Address(),
            //                                                        ShippingAddress = new Address(),
            //                                                        PaymentInfo = new PaymentInfo{ExpirationMonth = "11", ExpirationYear = "2222", CardNumber = "3333333"}
            //                                                    };

            //var target = new CheckoutSummaryPageViewModel(shoppingCartRepository, new MockNavigationService(),
            //                                              new MockOrderService(), new MockCheckoutDataRepository(),
            //                                              new MockAccountService(), new MockSettingsCharmService(),
            //                                              new MockResourceLoader());
            //target.OnNavigatedTo(null, NavigationMode.New, null);
            //target.SelectedShippingMethodViewModel = new ShippingMethodViewModel(new ShippingMethod());

            //target.SelectedShippingMethodViewModel = null;

            //Assert.AreEqual("$0.00", target.ShippingCost);
            //Assert.AreEqual("$100.00", target.OrderSubtotal);
            //Assert.AreEqual("$100.00", target.GrandTotal);
        }
    }
}
