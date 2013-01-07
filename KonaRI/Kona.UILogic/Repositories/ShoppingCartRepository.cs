// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.UILogic.Models;
using Kona.UILogic.Services;
using System;
using System.Threading.Tasks;

namespace Kona.UILogic.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        public static readonly string HighPriorityRoamingSettingKey = "HighPriority";
        IShoppingCartService _cartService;
        private readonly IAccountService _accountService;

        string _shoppingCartId;
        private AddressAndPaymentInfo _addressAndPaymentInfo;

        public ShoppingCartRepository(IShoppingCartService service, IAccountService accountService)
        {
            _cartService = service;
            _accountService = accountService;

            if (accountService != null)
            {
                _accountService.UserChanged += _accountService_UserChanged;
            }
            Windows.Storage.ApplicationData.Current.DataChanged += Current_DataChanged;
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            if (roamingSettings.Values.ContainsKey(HighPriorityRoamingSettingKey))
                _shoppingCartId = roamingSettings.Values[HighPriorityRoamingSettingKey].ToString();
            else
            {
                _shoppingCartId = Guid.NewGuid().ToString();
                roamingSettings.Values[HighPriorityRoamingSettingKey] = _shoppingCartId;
            }
        }

        public Task ClearCartAsync()
        {
            // TODO - need a DELETE method on ShoppingCartController that deletes the whole cart
            // current DELETE method could be used and just check for a null itemId, but need a server repository method to 
            // remove the whole cart
            RaiseShoppingCartUpdated();
            return Task.Factory.StartNew(() => { });
        }

        void _accountService_UserChanged(object sender, UserChangedEventArgs e)
        {
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            _shoppingCartId = e.UserInfo.UserName;
            roamingSettings.Values[HighPriorityRoamingSettingKey] = _shoppingCartId;
        }

        void Current_DataChanged(Windows.Storage.ApplicationData sender, object args)
        {
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            if (roamingSettings.Values.ContainsKey(HighPriorityRoamingSettingKey))
                _shoppingCartId = roamingSettings.Values[HighPriorityRoamingSettingKey].ToString();
        }

        public async Task<ShoppingCart> GetShoppingCartAsync()
        {
            ShoppingCart cart = null;
            try
            {
                return await _cartService.GetShoppingCartAsync(_shoppingCartId);
            }
            catch (Exception)
            {
                return cart;
            }
        }

        public Task AddProductToShoppingCartAsync(string productId)
        {
            var task = _cartService.AddProductToShoppingCartAsync(_shoppingCartId, productId);
            RaiseShoppingCartUpdated();
            return task;
        }

        public Task RemoveShoppingCartItemAsync(string itemId)
        {
            var task = _cartService.RemoveShoppingCartItemAsync(_shoppingCartId, itemId);
            RaiseShoppingCartUpdated();
            return task;
        }


        public void AddAddressAndPurchaseInfo(Address shippingAddress, Address billingAddress, PaymentInfo paymentInfo)
        {
            _addressAndPaymentInfo = new AddressAndPaymentInfo
                                         {
                                             ShippingAddress = shippingAddress,
                                             BillingAddress = billingAddress,
                                             PaymentInfo = paymentInfo
                                         };
        }

        public AddressAndPaymentInfo AddressAndPurchaseInfo
        {
            get { return _addressAndPaymentInfo; }
        }

        private void RaiseShoppingCartUpdated()
        {
            var handler = ShoppingCartUpdated;
            if (handler != null)
            {
                ShoppingCartUpdated(this, null);
            }
        }

        public event EventHandler ShoppingCartUpdated;
    }
}
