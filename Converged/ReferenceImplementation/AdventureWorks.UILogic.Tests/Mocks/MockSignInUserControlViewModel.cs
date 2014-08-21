// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

using AdventureWorks.UILogic.ViewModels;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    using Windows.UI.Xaml.Navigation;

    public class MockSignInUserControlViewModel : ISignInUserControlViewModel
    {
        public Action<Action> OpenDelegate { get; set; }
        public Action<object, NavigationMode,Dictionary<string, object>> OnNavigatedToDelegate { get; set; }
        public Action<Dictionary<string, object>,bool> OnNavigatedFromDelegate { get; set; }

        public void Open(Action successAction)
        {
            OpenDelegate(successAction);
        }

        public void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState)
        {
            OnNavigatedToDelegate(navigationParameter, navigationMode, viewState);
        }

        public void OnNavigatedFrom(Dictionary<string, object> viewState, bool suspending)
        {
            OnNavigatedFromDelegate(viewState, suspending);
        }
    }
}
