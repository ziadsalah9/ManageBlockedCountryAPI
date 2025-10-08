using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageBlockedCountry.Application.Dtos
{
    public class FetchIPLookUPDto
    {
        public string Ip { get; set; } = "";
        public string? CountryCode { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Isp { get; set; }
        public bool IsBlocked { get; set; }
        // "Permanent" | "Temporary" | "None"
        public string BlockType { get; set; } = "None";

    }
}
