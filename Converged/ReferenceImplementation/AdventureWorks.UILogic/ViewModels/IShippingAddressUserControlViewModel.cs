// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Mvvm;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.ViewModels
{
    public interface IShippingAddressUserControlViewModel
    {
        [RestorableState]
        Address Address { get; set; }
        IReadOnlyCollection<ComboBoxItemValue> States { get; }
        void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState);
        void OnNavigatedFrom(Dictionary<string, object> viewState, bool suspending);
        Task ProcessFormAsync();
        bool ValidateForm();
        Task PopulateStatesAsync();
        event PropertyChangedEventHandler PropertyChanged;
        void SetLoadDefault(bool loadDefault);
    }
}