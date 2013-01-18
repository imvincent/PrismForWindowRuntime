// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.Infrastructure;
using Kona.UILogic.Repositories;
using Microsoft.Practices.Prism.Events;
using Kona.UILogic.Events;

namespace Kona.UILogic.ViewModels
{
    public class ShoppingCartTabUserControlViewModel : BindableBase
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly INavigationService _navigationService;
        private int _itemCount;

        public ShoppingCartTabUserControlViewModel(IShoppingCartRepository shoppingCartRepository, IEventAggregator eventAggregator, INavigationService navigationService)
        {
            _itemCount = 0; //ItemCount will be set using async method call.

            _shoppingCartRepository = shoppingCartRepository;
            _navigationService = navigationService;

            if (eventAggregator != null)
            {
                eventAggregator.GetEvent<ShoppingCartUpdatedEvent>().Subscribe(s => UpdateItemCountAsync());
            }

            ShoppingCartTabCommand = new DelegateCommand(NavigateToShoppingCartPage);

            //Start process of updating item count.
            UpdateItemCountAsync();
        }

        private async void UpdateItemCountAsync()
        {
            var shoppingCart = await _shoppingCartRepository.GetShoppingCartAsync();

            if (shoppingCart == null) return;

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
