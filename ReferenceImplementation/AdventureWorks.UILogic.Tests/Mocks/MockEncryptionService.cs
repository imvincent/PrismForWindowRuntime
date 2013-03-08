// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Services;
using Windows.Storage.Streams;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockEncryptionService : IEncryptionService
    {
        public Func<string, Task<string>> EncryptMessageDelegate { get; set; }
        public Func<string, Task<string>> DecryptMessageDelegate { get; set; }

        public Task<string> EncryptMessage(string message)
        {
            return EncryptMessageDelegate(message);
        }

        public Task<string> DecryptMessage(string buffer)
        {
            return DecryptMessageDelegate(buffer);
        }
    }
}
