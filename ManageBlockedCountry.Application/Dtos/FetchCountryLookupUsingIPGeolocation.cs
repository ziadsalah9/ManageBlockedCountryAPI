using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageBlockedCountry.Application.Dtos
{
    public class FetchCountryLookupUsingIPGeolocation
    {
        public string ip { get; set; }
        public string country { get; set; }

        public string country_code { get; set; }

        public bool success { get; set; }
        public bool IsBlocked { get; set; }

        // "Permanent" | "Temporary" | "None"
        public string BlockType { get; set; } = "None"; 
    }
}
