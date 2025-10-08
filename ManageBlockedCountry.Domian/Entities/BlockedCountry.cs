using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageBlockedCountry.Domian.Entities
{
    public class BlockedCountry
    {
        public string Ip { get; set; } = default;
        public string CountryCode { get; set; }

        public DateTime? Date { get; set; } = DateTime.UtcNow;
        public string reason { get; set; }

    }
}
