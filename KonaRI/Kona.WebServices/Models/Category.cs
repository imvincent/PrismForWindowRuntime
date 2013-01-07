// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;

namespace Kona.WebServices.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        public int ParentCategoryId { get; set; }

        public string Title { get; set; }

        public Uri ImageExternalUri { get; set; }

        public Uri ImageLocalUri { get; set; }

        public IEnumerable<Category> Subcategories { get; set; }
    }
}