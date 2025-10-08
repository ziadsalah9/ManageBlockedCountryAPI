using ManageBlockedCountry.Application.Dtos;
using ManageBlockedCountry.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        private readonly IBlockedAttemptLog _blockedAttemptLog;
        private readonly IHttpContextAccessor _httpContext;


        public LocationOfCountryService(  IBlockedCountry blockedService,
                                          HttpClient httpClient,
                                          ITemporaryBlockedCountry tempBlockedService ,
                                          IBlockedAttemptLog blockedAttemptLog
                                              , IHttpContextAccessor httpContext)
                                        {
                                            _httpClient = httpClient;
                                            _blockedService = blockedService;
                                            _tempBlockedService = tempBlockedService;
                                            _blockedAttemptLog = blockedAttemptLog;
                                             _httpContext = httpContext;

                                        }
        public async Task<FetchIPLookUPDto> LookUp(string ip)
        {
            // GET https://ipapi.co/{ip}/{format}/

            var result = await _httpClient.GetFromJsonAsync<Dictionary<string, object>>($"https://ipapi.co/{ip}/json/");


            var countryCode = result?["country_code"]?.ToString();
            var userAgent = _httpContext.HttpContext?.Request.Headers["User-Agent"].ToString() ?? "Unknown";


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



            _blockedAttemptLog.LogAttempt(new BlockedAttemptLogDto
            {
                IpAddress = ip,
                CountryCode = countryCode ?? "N/A",
                BlockedStatus = isBlocked,
                UserAgent = userAgent,
                Timestamp = DateTime.UtcNow
            });

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
           
                var userAgent = _httpContext.HttpContext?.Request.Headers["User-Agent"].ToString();

                _blockedAttemptLog.LogAttempt(new BlockedAttemptLogDto
                {
                    IpAddress = result.ip ?? ip,
                    CountryCode = countryCode ?? "Unknown",
                    BlockedStatus = result.IsBlocked ,
                    // BlockType = result.BlockType,
                    Timestamp = DateTime.UtcNow,
                    UserAgent = userAgent ?? "Unknown"
                });
            
            return result; 

            

        }

    }
}
