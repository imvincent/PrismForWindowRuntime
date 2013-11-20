// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventureWorks.WebServices.Repositories
{
    public class SearchSuggestionRepository : IRepository<string>
    {
        private static readonly IEnumerable<string> _searchSuggestions = PopulateSearchSuggestions();

        public IEnumerable<string> GetAll()
        {
            lock (_searchSuggestions)
            {
                // Return new collection so callers can iterate independently on separate threads
                return _searchSuggestions.ToArray();
            }
        }

        public string GetItem(int id)
        {
            throw new NotImplementedException();
        }

        public string Create(string item)
        {
            throw new NotImplementedException();
        }

        public bool Update(string item)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<string> PopulateSearchSuggestions()
        {
            return new List<string>
            {
                "Bib",
                "Bottle",
                "Brakes",
                "Cage",
                "Cap",
                "Chain",
                "Cleaner",
                "Crankset",
                "Derailleurs",
                "Feder",
                "Fork",
                "Frame",
                "Gloves",
                "Handlebar",
                "Headset",
                "Jersey",
                "Men",
                "Mountain",
                "Lock",
                "Pannier",
                "Pedal",
                "Pump",
                "Rack",
                "Road Bike",
                "Road",
                "Saddle",
                "Shorts",
                "Stand",
                "Tire",
                "Tights",
                "Touring",
                "Vest",
                "Wheel"
            };
        }
        
        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}