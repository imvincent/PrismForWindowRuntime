// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kona.UILogic.Models
{
    public class ModelValidationResult
    {
        public ModelValidationResult()
        {
            ModelState = new Dictionary<string, List<string>>();
        }
        public string Message { get; set; }
        public Dictionary<string,List<string>> ModelState { get; set; }
    }
}
