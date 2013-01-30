// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kona.UILogic.Tests.Mocks
{
    public class MockRestorableStateService : IRestorableStateService
    {
        Dictionary<string, object> _stateBag = new Dictionary<string, object>();

        public void SaveState(string key, object state)
        {
            _stateBag[key] = state;
            AppRestored(this,null);
        }

        public event EventHandler AppRestored = delegate { };

        public object GetState(string key)
        {
            if (_stateBag.ContainsKey(key))
                return _stateBag[key];
            else
                return null;
        }

        public void RaiseAppRestored()
        {
            AppRestored(this, null);
        }

        public void SetFrameState(Dictionary<string, object> frameState)
        {
            _stateBag = frameState;
        }
    }
}
