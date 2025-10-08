using ManageBlockedCountry.Application.Interfaces;
using ManageBlockedCountry.Infrastructure.ExternalApiIntegration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManagedBlockedCountryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {


        private readonly ILocationOfCountry _service;

        public LocationController(ILocationOfCountry service)
        {
            
            _service = service;
        }


        [HttpGet("/lookup/{ip}")]
        public async Task  <ActionResult> Lookup(string ip) {


            var result = await  _service.LookUp(ip);

            if (string.IsNullOrEmpty(result.CountryCode))
                return NotFound("Couldn't detect country");

            return Ok(result);


        }


        [HttpGet("/lookupWithGeoLocation/{ip}")]
        public async Task<ActionResult> LookupWithGeoLocation(string ip)
        {


            var result = await _service.LookUPwithGeolocation(ip);

            if (string.IsNullOrEmpty(result.country_code))
                return NotFound("Couldn't detect country");



            return Ok(result);


        }
    }
}
