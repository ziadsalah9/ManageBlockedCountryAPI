using ManageBlockedCountry.Application.Dtos;
using ManageBlockedCountry.Application.Interfaces;
using ManageBlockedCountry.Application.Services;
using ManageBlockedCountry.Domian.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManagedBlockedCountryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesBlockedController : ControllerBase
    {

        private readonly IBlockedCountry _service;

        public CountriesBlockedController(IBlockedCountry service)
        {
            _service = service;
            
        }

        [HttpPost("block")]
        public async Task<IActionResult> Add([FromBody] CreateCountryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var existingCountry = _service.Get(dto.Code);
            if (existingCountry is null)
            {

               
               await _service.Add(dto);

                    return Ok("Added successfully.");

            }

            return BadRequest("Country already blocked.");

        }

        [HttpDelete("block/{counrtycode}")]
        public async Task<ActionResult> Delete(string counrtycode)
            {

            if (string.IsNullOrWhiteSpace(counrtycode))
                return BadRequest("Country code cannot be null or empty.");

            var existing = _service.Get(counrtycode);
            if (existing == null)
                return BadRequest($"Country '{counrtycode}' not found.");

            await _service.Remove(counrtycode);
            return Ok($"{counrtycode} has been removed successfully.");

        }


        [HttpGet("blocked/getAll")]
        public async Task<ActionResult>GetAll()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet("isblocked/{code}")]
        public IActionResult IsBlocked(string code)
        {
            return Ok(new { ip=code, blocked = _service.IsBlocked(code) });
        }


        [HttpGet("blocked")]
        public ActionResult GetBlockedCountries([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
        {
            var result =  _service.GetAllAdv(page, pageSize, search);
            return Ok(result);
        }


    }
}
