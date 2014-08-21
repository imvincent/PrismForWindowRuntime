// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace HelloWorld.Services
{
    public interface IDataRepository
    {
        List<string> GetFeatures();
        string GetUserEnteredData();
        void SetUserEnteredData(string data);
    }
}
