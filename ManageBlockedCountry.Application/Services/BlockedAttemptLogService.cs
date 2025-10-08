using ManageBlockedCountry.Application.Dtos;
using ManageBlockedCountry.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageBlockedCountry.Application.Services
{
    public class BlockedAttemptLogService : IBlockedAttemptLog
    {
        private readonly ConcurrentBag<BlockedAttemptLogDto> _logs = new();
        private readonly IHttpContextAccessor _httpContext;

        public BlockedAttemptLogService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public IEnumerable<BlockedAttemptLogDto> GetAll(int page = 1, int pagesize = 10)
        {
            return _logs.OrderByDescending(l => l.Timestamp).Skip((page - 1) * pagesize).Take(pagesize).ToList();
                            
        }

        public void LogAttempt(BlockedAttemptLogDto log)
        {
            if (log == null)
            {
                throw new Exception(nameof(log));

            }

            if (string.IsNullOrEmpty(log.UserAgent))
                log.UserAgent = _httpContext.HttpContext?.Request.Headers["User-Agent"].ToString() ?? "Unknown";
            log.Timestamp = DateTime.UtcNow;


            _logs.Add(log);   
        }
    }
}
