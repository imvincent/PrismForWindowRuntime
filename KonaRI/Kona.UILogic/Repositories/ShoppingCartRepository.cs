// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.UILogic.Events;
using Kona.UILogic.Models;
using Kona.UILogic.Services;
using Microsoft.Practices.Prism.Events;
using System;
using System.Threading.Tasks;

namespace Kona.UILogic.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        public static readonly string HighPriorityRoamingSettingKey = "HighPriority";
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IAccountService _accountService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IProductCatalogRepository _productCatalogRepository;

        private string _shoppingCartId;
        
        public ShoppingCartRepository(IShoppingCartService shoppingCartService, IAccountService accountService, IEventAggregator eventAggregator, IProductCatalogRepository productCatalogRepository)
        {
            _shoppingCartService = shoppingCartService;
            _accountService = accountService;
            _eventAggregator = eventAggregator;
            _productCatalogRepository = productCatalogRepository;

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
            var task = _shoppingCartService.DeleteShoppingCartAsync(_shoppingCartId);
            RaiseShoppingCartUpdated();
            return task;
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
            ShoppingCart cart = await _shoppingCartService.GetShoppingCartAsync(_shoppingCartId);
            if (cart == null) return null;

            foreach (var shoppingCartItem in cart.ShoppingCartItems)
            {
                //Update ImageName with path to local instance of image.
                var product = await _productCatalogRepository.GetProductAsync(shoppingCartItem.Product.ProductNumber);
                shoppingCartItem.Product.ImageName = product.ImageName;
            }
            return cart;
        }

        public async void AddProductToShoppingCartAsync(string productId)
        {
            await _shoppingCartService.AddProductToShoppingCartAsync(_shoppingCartId, productId);
            RaiseShoppingCartUpdated();
        }

        public void RemoveShoppingCartItemAsync(int itemId)
        {
            _shoppingCartService.RemoveShoppingCartItemAsync(_shoppingCartId, itemId);
            RaiseShoppingCartUpdated();
        }

        private void RaiseShoppingCartUpdated()
        {
            _eventAggregator.GetEvent<ShoppingCartUpdatedEvent>().Publish("payload");
        }
    }
}
