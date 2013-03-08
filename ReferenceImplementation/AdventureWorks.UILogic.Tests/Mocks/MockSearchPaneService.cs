// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Microsoft.Practices.StoreApps.Infrastructure.Interfaces;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockSearchPaneService : ISearchPaneService
    {
        public void Show()
        {
        }

        public void ShowOnKeyboardInput(bool enable)
        {
        }

        public bool IsShowOnKeyboardInputEnabled()
        {
            return true;
        }
    }
}
