using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageBlockedCountry.Application.Dtos
{
    public class BlockedAttemptLogDto
    {
        public string IpAddress{ get; set; } = string.Empty;
        public string CountryCode { get; set; } = "";
        public bool BlockedStatus { get; set; }
        public string UserAgent { get; set; } = "";
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
