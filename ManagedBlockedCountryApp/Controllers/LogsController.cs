using ManageBlockedCountry.Application.Interfaces;
using ManageBlockedCountry.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManagedBlockedCountryApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {

        private readonly IBlockedAttemptLog _logService;

        public LogsController(IBlockedAttemptLog logService)
        {
            _logService = logService;
        }

        [HttpGet("blocked-attempts")]
        public IActionResult GetBlockedAttempts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var logs = _logService.GetAll(page, pageSize);
            return Ok(logs);
        }
    }
}
