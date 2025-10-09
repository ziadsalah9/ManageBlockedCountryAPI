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

        Task<Response<FetchIPLookUPDto>> LookUp(string ip);
        Task<Response <FetchCountryLookupUsingIPGeolocation>> LookUPwithGeolocation(string ip);
    }
}
