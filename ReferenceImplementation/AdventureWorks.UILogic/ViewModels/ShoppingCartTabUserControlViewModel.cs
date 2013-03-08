// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Services;
using Microsoft.Practices.PubSubEvents;
using System.Net.Http;
using System.IO;
using Microsoft.Practices.StoreApps.Infrastructure;
using Microsoft.Practices.StoreApps.Infrastructure.Interfaces;

namespace AdventureWorks.UILogic.ViewModels
{
    public class ShoppingCartTabUserControlViewModel : BindableBase
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly INavigationService _navigationService;
        private readonly IAlertMessageService _alertMessageService;
        private readonly IResourceLoader _resourceLoader;
        private readonly IAccountService _accountService;
        private int _itemCount;

        public ShoppingCartTabUserControlViewModel(IShoppingCartRepository shoppingCartRepository, IEventAggregator eventAggregator, INavigationService navigationService, IAlertMessageService alertMessageService, IResourceLoader resourceLoader, IAccountService accountService)
        {
            _itemCount = 0; //ItemCount will be set using async method call.

            _shoppingCartRepository = shoppingCartRepository;
            _navigationService = navigationService;
            _alertMessageService = alertMessageService;
            _resourceLoader = resourceLoader;
            _accountService = accountService;

            if (eventAggregator != null)
            {
                eventAggregator.GetEvent<ShoppingCartUpdatedEvent>().Subscribe(UpdateItemCountAsync);
                eventAggregator.GetEvent<ShoppingCartItemUpdatedEvent>().Subscribe(UpdateItemCountAsync);
            }

            ShoppingCartTabCommand = new DelegateCommand(NavigateToShoppingCartPage);

            //Start process of updating item count.
            UpdateItemCountAsync(null);
        }

        private async void UpdateItemCountAsync(object notUsed)
        {
            ShoppingCart shoppingCart = null;
            var serviceCallFailed = false;

            try
            {
                // Trigger auto-login if credentials are saved.
                await _accountService.VerifyUserAuthenticationAsync();
                shoppingCart = await _shoppingCartRepository.GetShoppingCartAsync();
            }
            catch (HttpRequestException)
            {
                serviceCallFailed = true;
            }
            catch(FileNotFoundException){}
            catch(UnauthorizedAccessException){}
            catch(Exception){}

            if (serviceCallFailed)
            {
                await _alertMessageService.ShowAsync(_resourceLoader.GetString("ErrorServiceUnreachable"), _resourceLoader.GetString("Error"));
            }

            if (shoppingCart == null)
            {
                ItemCount = 0;
                return;
            }

            var itemCount = 0;
            if (shoppingCart.ShoppingCartItems != null)
            {
                foreach (var shoppingCartItem in shoppingCart.ShoppingCartItems)
                {
                    itemCount += shoppingCartItem.Quantity;
                }
            }
            ItemCount = itemCount;
        }

        public int ItemCount
        {
            get { return _itemCount; }
            set { SetProperty(ref _itemCount, value); }
        }

        public DelegateCommand ShoppingCartTabCommand { get; set; }

        private void NavigateToShoppingCartPage()
        {
            _navigationService.Navigate("ShoppingCart", null);
        }
    }
}
