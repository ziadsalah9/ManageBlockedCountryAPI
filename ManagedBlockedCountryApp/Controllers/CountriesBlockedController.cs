using ManageBlockedCountry.Application.Dtos;
using ManageBlockedCountry.Application.Interfaces;
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
        public IActionResult Add([FromBody] CreateCountryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            var existingCountry = _service.Get(dto.Code);
            if (existingCountry is null)
            {

               
                _service.Add(dto);

                    return Ok("Added successfully.");

            }

            return BadRequest("Country already blocked.");

        }

        [HttpDelete("block/{counrtycode}")]
        public IActionResult Delete(string counrtycode)
            {


            if (counrtycode is not null)
            {
                if (ModelState.IsValid)
                {
                    _service.Remove(counrtycode);
                    return Ok($"{counrtycode} is removed successfully ");


                }
                else
                    return
                        StatusCode(404,"return valid code ");
            }
            else { return BadRequest("code cant be null "); }

            }


        [HttpGet("blocked/getAll")]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("isblocked/{code}")]
        public IActionResult IsBlocked(string code)
        {
            return Ok(new { ip=code, blocked = _service.IsBlocked(code) });
        }


    }
}
