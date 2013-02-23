// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Kona.Infrastructure.Interfaces;
using System.Collections.Generic;

namespace HelloWorldWithContainer.Services
{
    public class DataRepository : IDataRepository
    {
        private const string UserEnteredData = "UserEnteredData";
        ISuspensionManagerState _suspensionManagerState;

        public DataRepository(ISuspensionManagerState suspensionManagerState)
        {
            _suspensionManagerState = suspensionManagerState;
        }

        public List<string> GetKonaFeatures()
        {
            return new List<string>
            {
                "Application structuring with MVVM and dependencies",
                "Page navigation with ViewModel participation and navigation commanding",
                "Application state management through suspend, terminate, and resume",
                "User input validation on client and server side with validation error displays",
                "Loosely coupled communications with Commands and Pub/Sub events"
            };
        }

        public string GetUserEnteredData()
        {
            return _suspensionManagerState.SessionState.ContainsKey(UserEnteredData)
                ? _suspensionManagerState.SessionState[UserEnteredData] as string
                : string.Empty;
        }

        public void SetUserEnteredData(string data)
        {
            _suspensionManagerState.SessionState[UserEnteredData] = data;
        }
    }
}
