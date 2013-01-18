// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;

namespace Microsoft.Practices.Prism.Events
{
    /// <summary>
    /// Defines the interface for invoking methods through a Dispatcher Facade
    /// </summary>
    public interface IDispatcherFacade
    {
        /// <summary>
        /// Dispatches an invocation to the method received as parameter.
        /// </summary>
        /// <param name="method">Method to be invoked.</param>
        /// <param name="arg">Arguments to pass to the invoked method.</param>
        void BeginInvoke(Delegate method, object arg);
    }
}