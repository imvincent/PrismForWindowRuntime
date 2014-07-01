// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm.Interfaces;

namespace Microsoft.Practices.Prism.StoreApps.Tests.Mocks
{
    public class MockSessionStateService : ISessionStateService
    {
        public Func<IFrameFacade, Dictionary<string, object>> GetSessionStateForFrameDelegate { get; set; }

        public MockSessionStateService()
        {
            SessionState = new Dictionary<string, object>();
        }

        public Dictionary<string, object> SessionState { get; private set; }

        public void RegisterKnownType(Type type)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }

        public Task RestoreSessionStateAsync()
        {
            throw new NotImplementedException();
        }

        public void RestoreFrameState()
        {
            throw new NotImplementedException();
        }

        public void RegisterFrame(IFrameFacade frame, string sessionStateKey)
        {
            throw new NotImplementedException();
        }

        public void UnregisterFrame(IFrameFacade frame)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> GetSessionStateForFrame(IFrameFacade frame)
        {
            return GetSessionStateForFrameDelegate(frame);
        }
    }
}
