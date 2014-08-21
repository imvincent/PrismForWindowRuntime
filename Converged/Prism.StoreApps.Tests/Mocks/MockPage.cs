// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Practices.Prism.StoreApps.Tests.Mocks
{
    using Microsoft.Practices.Prism.Mvvm;

    public class MockPage : Page, IView
    {
        public object PageParameter { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e != null)
            {
                this.PageParameter = e.Parameter;
            }
        }
    }
}
