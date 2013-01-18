// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Windows.Input;
using Kona.Infrastructure;

namespace Kona.UILogic.ViewModels
{
    // DP: The content property is not used yet, we have to determine if it is needed
    public class CheckoutDataViewModel : ViewModel
    {
        private string _name;
        private string _dataType;
        private string _firstLine;
        private string _secondLine;
        private object _content;
        private ISettingsCharmService _settingsCharmService;

        public CheckoutDataViewModel()
        {
            this.EditDataCommand = new DelegateCommand(EditData);
        }

        public CheckoutDataViewModel(dynamic checkoutData, ISettingsCharmService settingsCharmService)
            : this()
        {
            this.EntityId = checkoutData.EntityId;
            this.DataType = checkoutData.DataType;
            this.FirstLine = checkoutData.FirstLine;
            this.SecondLine = checkoutData.SecondLine;
            this.Name = checkoutData.Name;
            this.Content = checkoutData.Content;
            this._settingsCharmService = settingsCharmService;
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

        public object Content
        {
            get { return _content; }
            set { SetProperty(ref _content, value); }
        }

        public Uri ImageUrl
        {
            get
            {
                switch (DataType)
                {
                    case "Shipping Address":
                        return null;
                    case "Billing Address":
                        return null;
                    case "PaymentInfo":
                        return null;
                    default:
                        return null;
                }
            }
        }

        public ICommand EditDataCommand { get; set; }

        public void EditData()
        {
            // Navigate to edit page{

            if (DataType == null) return;

            switch (DataType)
            {
                case "Billing Address": _settingsCharmService.ShowFlyout("editBillingAddress", EntityId, null);
                    break;
                case "Shipping Address": _settingsCharmService.ShowFlyout("editShippingAddress", EntityId, null);
                    break;
                case "Payment": _settingsCharmService.ShowFlyout("editPaymentMethod", EntityId, null);
                    break;
            }
        
        }
    }
}
