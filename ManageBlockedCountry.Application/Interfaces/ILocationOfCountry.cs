using ManageBlockedCountry.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageBlockedCountry.Application.Interfaces
{
    public interface ILocationOfCountry
    {
        // via ip 

        Task<FetchIPLookUPDto> LookUp(string ip);
        Task<FetchCountryLookupUsingIPGeolocation> LookUPwithGeolocation(string ip);
    }
}
