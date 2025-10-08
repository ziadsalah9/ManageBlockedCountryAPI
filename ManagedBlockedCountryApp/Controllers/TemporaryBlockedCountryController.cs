using ManageBlockedCountry.Application.Dtos;
using ManageBlockedCountry.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManagedBlockedCountryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemporaryBlockedCountryController : ControllerBase
    {


        private readonly ITemporaryBlockedCountry _tempBlockService;
        public TemporaryBlockedCountryController(ITemporaryBlockedCountry tempBlockService)
        {
            _tempBlockService = tempBlockService;
        }
        [HttpPost("temporal-block")]
        public IActionResult AddTemporaryBlock([FromBody] TemporaryBlockedCountryDto dto)
        {


            if (ModelState.IsValid) {

                // check if already temporarily blocked
                    if (!_tempBlockService.isTemporaryBlocked(dto.countrycode))
                    {

                    _tempBlockService.Add(dto.countrycode, dto.Duration);
                    return Ok(new
                    {
                        message = $"Country {dto.countrycode} temporarily blocked for {dto.Duration} minutes.",
                        expiresAt = DateTime.UtcNow.AddMinutes(dto.Duration)
                    });

                }
                return BadRequest($"Country {dto.countrycode} is already temporarily blocked.");

            }
            return BadRequest(ModelState);



        }
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var list = _tempBlockService.GetAll();
            return Ok(list);
        }

  



    }
}
