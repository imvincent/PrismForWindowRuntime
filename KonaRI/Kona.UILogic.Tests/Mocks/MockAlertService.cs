// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Threading.Tasks;
using Kona.UILogic.Services;

namespace Kona.UILogic.Tests.Mocks
{
    public class MockAlertMessageService : IAlertMessageService
    {
        public Func<string, string, Task> ShowAsyncDelegate { get; set; }
        public Task ShowAsync(string message, string title)
        {
            return ShowAsyncDelegate(message, title);
        }
    }
}
