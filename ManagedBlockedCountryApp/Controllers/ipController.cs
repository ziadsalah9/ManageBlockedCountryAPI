using ManageBlockedCountry.Application.Interfaces;
using ManageBlockedCountry.Infrastructure.ExternalApiIntegration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManagedBlockedCountryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ipController : ControllerBase
    {


        private readonly ILocationOfCountry _service;
        private readonly IBlockedAttemptLog _blockedAttemptLog;

        public ipController(ILocationOfCountry service, IBlockedAttemptLog blockedAttemptLog)
        {

            _service = service;
            _blockedAttemptLog = blockedAttemptLog;
        }


        [HttpGet("/lookup")]
        public async Task  <ActionResult> Lookup(string ipAddress) {


            var result = await  _service.LookUp(ipAddress);

            //if (string.IsNullOrEmpty(result.Data.CountryCode))
            //    return NotFound("Couldn't detect country");

            return Ok(result);


        }


        [HttpGet("/lookupWithGeoLocation")]
        public async Task<ActionResult> LookupWithGeoLocation([FromQuery]string ipAddress)
        {


            var result = await _service.LookUPwithGeolocation(ipAddress);

   



            return Ok(result);


        }


        [HttpGet("check-block")]
        public async Task <ActionResult> CheckBolck()
        {

            var IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            Console.WriteLine(IpAddress);
//return Ok(IpAddress);

            if (string.IsNullOrEmpty(IpAddress)) return BadRequest("cant determine the ip address");
            var result =await _service.LookUPwithGeolocation(IpAddress);

            return Ok(new { IP = result.Data.ip , CountryCode = result.Data.country_code, Country = result.Data.country , BlockType = result.Data.BlockType});


        }
    }
}
