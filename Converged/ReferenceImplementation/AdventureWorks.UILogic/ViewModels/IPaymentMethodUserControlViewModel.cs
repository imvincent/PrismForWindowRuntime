// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel;
using AdventureWorks.UILogic.Models;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;

namespace AdventureWorks.UILogic.ViewModels
{
    public interface IPaymentMethodUserControlViewModel
    {
        [RestorableState]
        PaymentMethod PaymentMethod { get; set; }
        void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState);
        void OnNavigatedFrom(Dictionary<string, object> viewState, bool suspending);
        Task ProcessFormAsync();
        bool ValidateForm();
        event PropertyChangedEventHandler PropertyChanged;
        void SetLoadDefault(bool loadDefault);
    }
}