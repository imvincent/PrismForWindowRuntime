// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Windows.Input;
using Kona.Infrastructure;
using Windows.ApplicationModel.Resources;

namespace Kona.UILogic.ViewModels
{
    public class CheckoutDataViewModel : ViewModel
    {
        private string _name;
        private string _dataType;
        private string _firstLine;
        private string _secondLine;
        private ISettingsCharmService _settingsCharmService;

        public CheckoutDataViewModel()
        {
            EditDataCommand = new DelegateCommand(EditData);
        }

        public CheckoutDataViewModel(dynamic checkoutData, ISettingsCharmService settingsCharmService)
            : this()
        {
            EntityId = checkoutData.EntityId;
            DataType = checkoutData.DataType;
            FirstLine = checkoutData.FirstLine;
            SecondLine = checkoutData.SecondLine;
            Name = checkoutData.Name;
            _settingsCharmService = settingsCharmService;
        }

        public string DataType
        {
            get { return _dataType; }
            set { SetProperty(ref _dataType, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string FirstLine
        {
            get { return _firstLine; }
            set { SetProperty(ref _firstLine, value); }
        }

        public string SecondLine
        {
            get { return _secondLine; }
            set { SetProperty(ref _secondLine, value); }
        }

        public ICommand EditDataCommand { get; set; }

        public void EditData()
        {
            if (DataType == null) return;

            var resourceLoader = new ResourceLoader();
            string flyoutName = string.Empty;
            
            // We cannot use a switch here because we're comparing the DataType with a dynamic value
            if (DataType == resourceLoader.GetString("ShippingAddress"))
            {
                flyoutName = "editShippingAddress";
            }

            if (DataType == resourceLoader.GetString("BillingAddress"))
            {
               flyoutName = "editBillingAddress";
            }

            if (DataType == resourceLoader.GetString("PaymentInfo"))
            {
                flyoutName = "editPaymentMethod";
            }

            // Display EditFlyout
            if (string.IsNullOrEmpty(flyoutName))
            {
                _settingsCharmService.ShowFlyout(flyoutName, EntityId, null);
            }
        }
    }
}
