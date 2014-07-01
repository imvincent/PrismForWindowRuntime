// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AdventureWorks.UILogic.Models
{
    [DataContract]
    public class Category
    {
        // Needed only for Serialization
        public Category() {}

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ParentId { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public Uri ImageUri { get; set; }

        [DataMember]
        public IEnumerable<Category> Subcategories { get; set; }

        [DataMember]
        public IEnumerable<Product> Products { get; set; }

        [DataMember]
        public int TotalNumberOfItems { get; set; }

        [DataMember]
        public bool HasSubcategories { get; set; }
    }
}