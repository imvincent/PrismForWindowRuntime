// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.ObjectModel;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Services;
using Microsoft.Practices.PubSubEvents;
using System;
using System.Threading.Tasks;
using Microsoft.Practices.StoreApps.Infrastructure;
using Microsoft.Practices.StoreApps.Infrastructure.Interfaces;
using Windows.ApplicationModel.Resources;

namespace AdventureWorks.UILogic.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        public const string ShoppingCartIdKey = "ShoppingCartId";
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IAccountService _accountService;
        private readonly IEventAggregator _eventAggregator;
        private readonly ISessionStateService _sessionStateService;
        private readonly IAlertMessageService _alertMessageService;

        private string _shoppingCartId;
        private ShoppingCart _cachedShoppingCart = null;
        
        public ShoppingCartRepository(IShoppingCartService shoppingCartService, IAccountService accountService, IEventAggregator eventAggregator, ISessionStateService sessionStateService, IAlertMessageService alertMessageService)
        {
            _shoppingCartService = shoppingCartService;
            _accountService = accountService;
            _eventAggregator = eventAggregator;
            _sessionStateService = sessionStateService;
            _alertMessageService = alertMessageService;
            
            if (accountService != null)
            {
                _accountService.UserChanged += _accountService_UserChanged;
            }

            if (_sessionStateService != null && _sessionStateService.SessionState.ContainsKey(ShoppingCartIdKey))
            {
                _shoppingCartId = _sessionStateService.SessionState[ShoppingCartIdKey].ToString();
            }
            else
            {
                _shoppingCartId = Guid.NewGuid().ToString();
                _sessionStateService.SessionState[ShoppingCartIdKey] = _shoppingCartId;
            }
        }

        public async Task ClearCartAsync()
        {
            _cachedShoppingCart = null;
            await _shoppingCartService.DeleteShoppingCartAsync(_shoppingCartId);
            RaiseShoppingCartUpdated();
        }

        async void _accountService_UserChanged(object sender, UserChangedEventArgs e)
        {
            var shoppingCartMerged = false;
            _cachedShoppingCart = null;
            if (e.NewUserInfo != null)
            {
                // User successfully signed in.
                if (e.OldUserInfo == null)
                {
                    shoppingCartMerged = await _shoppingCartService.MergeShoppingCartsAsync(_shoppingCartId, e.NewUserInfo.UserName);
                }
                _shoppingCartId = e.NewUserInfo.UserName;
            }
            else
            {
                // User signed out.
                _shoppingCartId = Guid.NewGuid().ToString();
            }

            _sessionStateService.SessionState[ShoppingCartIdKey] = _shoppingCartId;
            RaiseShoppingCartUpdated();
            
            if (shoppingCartMerged)
            {
                var resourceLoader = new ResourceLoader(Constants.AdventureWorksUILogicResourceMapId);
                await _alertMessageService.ShowAsync(resourceLoader.GetString("MergedShoppingCartMessage"), string.Empty);
            }
        }

        public async Task<ShoppingCart> GetShoppingCartAsync()
        {
            if (_cachedShoppingCart != null) return _cachedShoppingCart;

            _cachedShoppingCart = await _shoppingCartService.GetShoppingCartAsync(_shoppingCartId);

            return _cachedShoppingCart;
        }

        public async Task AddProductToShoppingCartAsync(string productId)
        {
            _cachedShoppingCart = null;
            await _shoppingCartService.AddProductToShoppingCartAsync(_shoppingCartId, productId);
            RaiseShoppingCartItemUpdated();
        }

        public async Task RemoveProductFromShoppingCartAsync(string productId)
        {
            _cachedShoppingCart = null;
            await _shoppingCartService.RemoveProductFromShoppingCartAsync(_shoppingCartId, productId);
            RaiseShoppingCartItemUpdated();
        }

        public async Task RemoveShoppingCartItemAsync(string itemId)
        {
            _cachedShoppingCart = null;
            await _shoppingCartService.RemoveShoppingCartItemAsync(_shoppingCartId, itemId);
            RaiseShoppingCartItemUpdated();
        }

        // <snippet1501>
        private void RaiseShoppingCartUpdated()
        {
            _eventAggregator.GetEvent<ShoppingCartUpdatedEvent>().Publish(null);
        }
        // </snippet1501>

        private void RaiseShoppingCartItemUpdated()
        {
            _eventAggregator.GetEvent<ShoppingCartItemUpdatedEvent>().Publish(null);
        }
    }
}
