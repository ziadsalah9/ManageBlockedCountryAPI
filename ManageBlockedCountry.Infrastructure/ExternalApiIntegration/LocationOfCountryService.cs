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
        private readonly ITemporaryBlockedCountry _tempBlockedService;

        public LocationOfCountryService( IBlockedCountry blockedService,
HttpClient httpClient,
ITemporaryBlockedCountry tempBlockedService)
        {
            _httpClient = httpClient;
            _blockedService = blockedService;
            _tempBlockedService = tempBlockedService;
        }
        public async Task<FetchIPLookUPDto> LookUp(string ip)
        {
            // GET https://ipapi.co/{ip}/{format}/

            var result = await _httpClient.GetFromJsonAsync<Dictionary<string, object>>($"https://ipapi.co/{ip}/json/");


            var countryCode = result?["country_code"]?.ToString();

            var isPermanentlyBlocked = _blockedService.IsBlocked(countryCode);

            var isTemporarilyBlocked = _tempBlockedService.isTemporaryBlocked(countryCode);

            var isBlocked = isPermanentlyBlocked || isTemporarilyBlocked;

            string blockType = "None";
            bool Blockedornot = false;

            if (isPermanentlyBlocked)
            {
                Blockedornot = true;
                blockType = "Permanent";
            }
            else if (isTemporarilyBlocked)
            {
                Blockedornot = true;
                blockType = "Temporary";
            }

            return new FetchIPLookUPDto
            {
                Ip = result?["ip"]?.ToString() ?? ip,
                Country = countryCode,
                CountryCode = result?["country_code"]?.ToString(),
                City = result?["city"]?.ToString(),
                Isp = result?["org"]?.ToString(),
                IsBlocked = isBlocked
                ,
                BlockType = blockType

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
            var isTemporarilyBlocked = !string.IsNullOrEmpty(countryCode) && _tempBlockedService.isTemporaryBlocked(countryCode);

            result.IsBlocked = isBlocked || isTemporarilyBlocked;
            if (isBlocked)
            {
                result.BlockType = "Permanent";
            }
            else if (isTemporarilyBlocked)
            {
                result.BlockType = "Temporary";
            }
            else
            {
                result.BlockType = "None";
            }
            Console.WriteLine($"Checking block for country code: {countryCode}");
            return result; 

            

        }

    }
}
