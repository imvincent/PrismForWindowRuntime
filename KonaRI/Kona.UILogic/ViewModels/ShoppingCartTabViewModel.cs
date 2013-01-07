// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using Kona.Infrastructure;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Windows.UI.Xaml.Navigation;

namespace Kona.UILogic.ViewModels
{
    public class ShoppingCartTabViewModel : BindableBase
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private int _itemCount;

        public ShoppingCartTabViewModel(IShoppingCartRepository shoppingCartRepository)
        {
            _itemCount = 0; //ItemCount will be set using async method call.

            _shoppingCartRepository = shoppingCartRepository;

            //TODO: No opportunity to unregister. Replace with weak reference eventing.
            if (_shoppingCartRepository != null)
            {
                _shoppingCartRepository.ShoppingCartUpdated += _shoppingCartRepository_ShoppingCartUpdated;
            }

            ShoppingCartTabCommand = new DelegateCommand(OpenAppBar);

            //Start process of updating item count.
            UpdateItemCountAsync();
        }

        void _shoppingCartRepository_ShoppingCartUpdated(object sender, System.EventArgs e)
        {
            UpdateItemCountAsync();
        }

        private async void UpdateItemCountAsync()
        {
            var shoppingCart = await _shoppingCartRepository.GetShoppingCartAsync();

            if (shoppingCart == null) return;

            var itemCount = 0;
            foreach (var shoppingCartItem in shoppingCart.ShoppingCartItems)
            {
                itemCount += shoppingCartItem.Quantity;
            }
            ItemCount = itemCount;
        }

        public int ItemCount
        {
            get { return _itemCount; }
            set { SetProperty(ref _itemCount, value); }
        }

        public DelegateCommand ShoppingCartTabCommand { get; set; }

        private void OpenAppBar()
        {
            //TODO: Need to open the current page's app bar.
        }
    }
}
