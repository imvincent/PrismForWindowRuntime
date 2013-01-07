// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace Kona.Infrastructure
{
    public class FrameSessionStateWrapper : IFrameSessionState
    {
        public Dictionary<string, object> GetSessionStateForFrame(Frame frame)
        {
            return SuspensionManager.SessionStateForFrame(frame);
        }
    }
}
