// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.Practices.Prism.StoreApps.Tests.Mocks
{
    public class MockViewModelWithNoRestorableStateAttributes : Microsoft.Practices.Prism.Mvvm.ViewModel
    {
        private string title;
        public string Title
        {
            get { return this.title; }
            set { this.SetProperty(ref this.title, value); }

        }

        private string description;
        public string Description
        {
            get { return this.description; }
            set { this.SetProperty(ref this.description, value); }

        }

        private ICollection<Microsoft.Practices.Prism.Mvvm.ViewModel> childViewModels;

        public ICollection<Microsoft.Practices.Prism.Mvvm.ViewModel> ChildViewModels
        {
            get { return this.childViewModels; }
            set { this.SetProperty(ref this.childViewModels, value); }

        }
    }
}
