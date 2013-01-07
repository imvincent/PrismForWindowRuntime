// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;

namespace Kona.UILogic.Models
{
    public class UserChangedEventArgs : EventArgs
    {
        public UserChangedEventArgs() { }

        public UserChangedEventArgs(UserInfo userInfo)
        {
            UserInfo = userInfo;
        }

        public UserInfo UserInfo { get; set; }
    }
}
