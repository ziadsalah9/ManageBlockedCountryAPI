using ManageBlockedCountry.Application;
using ManageBlockedCountry.Application.Dtos;
using ManageBlockedCountry.Application.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public async Task<Response<FetchIPLookUPDto>> LookUp(string ip)
        {
            // GET https://ipapi.co/{ip}/{format}/


            try
            {
                var response = await _httpClient.GetAsync($"https://ipapi.co/{ip}/json/");


                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    return new Response<FetchIPLookUPDto>
                    {
                        StatusCode = 429,
                        Message = "Rate limit exceeded (429)",
                        Data = null
                    };
                }

                response.EnsureSuccessStatusCode();


                var result = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();




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


                var dto = new FetchIPLookUPDto()
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
                return new Response<FetchIPLookUPDto> 
                {
                    Message ="fetched successfully !!",
                    StatusCode=200,
                    Data = dto
                 

                };
            }
            catch (Exception ex) {



                return new Response<FetchIPLookUPDto>
                {
                    Message = ex.Message
                    ,
                    StatusCode = 429,

                };

                //return new FetchIPLookUPDto
                //{
                //    Ip = ip,
                //    Country = "Unknown",
                //    CountryCode = "N/A",
                //    City = "N/A",
                //    Isp = "N/A",
                //    IsBlocked = false,
                //    BlockType = "None"
                //};
            }
         


        }
   

        public async Task <Response <FetchCountryLookupUsingIPGeolocation>> LookUPwithGeolocation(string ip)
        {


            // if user dosent send ip --> ip is nullorempty 

            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = _httpContext.HttpContext?.Connection?.RemoteIpAddress?.ToString();

                return new Response<FetchCountryLookupUsingIPGeolocation>
                {
                    StatusCode = 400,
                    Message = "Invalid IP address format.",
                    Data = null
                };
            }

            // convert ipv4->ipv4
            if (IPAddress.TryParse(ip, out var parsedIp))
            {
                if (ip.StartsWith("::ffff:") || ip.StartsWith("192.") || ip.StartsWith("10.") || ip.StartsWith("172."))
                {
                    return new Response<FetchCountryLookupUsingIPGeolocation>
                    {
                        StatusCode = 400,
                        Message = "Private or local IP addresses are not supported.",
                        Data = null
                    };
                }
                //if (parsedIp.IsIPv4MappedToIPv6)
                //{
                //    ip = parsedIp.MapToIPv4().ToString();

                //    Console.WriteLine($"ip is -> {ip}");
                //}
            }


            // Validate IP format
            if (!IPAddress.TryParse(ip, out _))
            {
                throw new ArgumentException("Invalid IP address format");
            }


            HttpResponseMessage response;
            try
            {

                 response = await _httpClient.GetAsync($"https://ipwho.is/{ip}");


                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    return new Response<FetchCountryLookupUsingIPGeolocation>
                    {
                        StatusCode = 429,
                        Message = "Rate limit exceeded. Please try again later.",
                        Data = null
                    };
                }
                if (!response.IsSuccessStatusCode)
                {
                    return new Response<FetchCountryLookupUsingIPGeolocation>
                    {
                        StatusCode = (int)response.StatusCode,
                        Message = $"Failed to fetch IP details. Status: {response.StatusCode}",
                        Data = null
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Network error occurred while contacting the IP API.", ex);
            }

            //  var result = await _httpClient.GetFromJsonAsync<FetchCountryLookupUsingIPGeolocation>($"https://ipwho.is/{ip}");
            var result = await response.Content.ReadFromJsonAsync<FetchCountryLookupUsingIPGeolocation>();



            if (result == null)
                return new Response<FetchCountryLookupUsingIPGeolocation>
                {
                    StatusCode = 500,
                    Message = "Failed to parse IP details response.",
                    Data = null
                };            //var isblocked = _blockedService?.IsBlocked(result.country_code);

            //if (result2.StatusCode == HttpStatusCode.TooManyRequests)
            //    throw new Exception("Rate limit exceeded. Please try again later.");

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




            return new Response<FetchCountryLookupUsingIPGeolocation>
            {
                StatusCode = 200,
                Message = "Fetched successfully.",
                Data = result
            };


        }

    }
}
