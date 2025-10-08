using ManageBlockedCountry.Application.Dtos;
using ManageBlockedCountry.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ManageBlockedCountry.Infrastructure.ExternalApiIntegration
{
    public class LocationOfCountryService : ILocationOfCountry
    {

        private readonly HttpClient _httpClient;
        private readonly IBlockedCountry _blockedService;

        public LocationOfCountryService( IBlockedCountry blockedService,
HttpClient httpClient)
        {
            _httpClient = httpClient;
            _blockedService = blockedService;


        }
        public async Task<FetchIPLookUPDto> LookUp(string ip)
        {
            // GET https://ipapi.co/{ip}/{format}/

            var result = await _httpClient.GetFromJsonAsync<Dictionary<string, object>>($"https://ipapi.co/{ip}/json/");


            var countryCode = result?["country_code"]?.ToString();

            var isblocked = _blockedService.IsBlocked(countryCode);

            return new FetchIPLookUPDto
            {
                Ip = result?["ip"]?.ToString() ?? ip,
                Country = countryCode,
                CountryCode = result?["country_code"]?.ToString(),
                City = result?["city"]?.ToString(),
                Isp = result?["org"]?.ToString(),
                IsBlocked = isblocked

            };
         

        }


        public async Task <FetchCountryLookupUsingIPGeolocation> LookUPwithGeolocation(string ip)
        {

            var result = await _httpClient.GetFromJsonAsync<FetchCountryLookupUsingIPGeolocation>($"https://ipwho.is/{ip}");

            if (result == null)
                throw new Exception("Failed to fetch IP details");
            //var isblocked = _blockedService?.IsBlocked(result.country_code);
            var countryCode = result.country_code?.Trim()?.ToUpperInvariant();

            var isBlocked = _blockedService.IsBlocked(countryCode);

            result.IsBlocked = isBlocked;
            Console.WriteLine($"Checking block for country code: {countryCode}");
            return result; 

            

        }

    }
}
