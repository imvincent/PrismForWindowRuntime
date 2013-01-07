// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Threading.Tasks;
using Kona.UILogic.Repositories;
using Kona.UILogic.Models;

namespace Kona.UILogic.Tests.Mocks
{
    public class MockShoppingCartRepository : IShoppingCartRepository
    {
        public Func<Task<ShoppingCart>> GetShoppingCartAsyncDelegate { get; set; }

        public Func<string, Task> AddProductToShoppingCartAsyncDelegate { get; set; }

        public Func<string, Task> RemoveShoppingCartItemAsyncDelegate { get; set; }

        public Action<Address, Address, PaymentInfo> AddAddressAndPurchaseInfoDelegate { get; set; }

        public Task ClearCartAsync()
        {
            return Task.Factory.StartNew(() => { });
        }

        public MockShoppingCartRepository()
        {
            AddAddressAndPurchaseInfoDelegate = (address, address1, arg3) => { };
        }

        public Task<ShoppingCart> GetShoppingCartAsync()
        {
            return this.GetShoppingCartAsyncDelegate();
        }

        public Task AddProductToShoppingCartAsync(string productId)
        {
            return this.AddProductToShoppingCartAsyncDelegate(productId);
        }

        public Task RemoveShoppingCartItemAsync(string itemId)
        {
            return this.RemoveShoppingCartItemAsyncDelegate(itemId);

        }

        public void AddAddressAndPurchaseInfo(Address shippingAddress, Address billingAddress, PaymentInfo paymentInfo)
        {
            AddAddressAndPurchaseInfoDelegate(shippingAddress, billingAddress, paymentInfo);
        }

        public AddressAndPaymentInfo AddressAndPurchaseInfo { get; set;}
        
        public void RaiseShoppingCartUpdatedEvent()
        {
            ShoppingCartUpdated(this, null);
        }

        public event EventHandler ShoppingCartUpdated;

        public UserInfo CurrentUser { get; set; }
    }
}
