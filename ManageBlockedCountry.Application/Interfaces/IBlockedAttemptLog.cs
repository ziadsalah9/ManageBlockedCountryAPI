using ManageBlockedCountry.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageBlockedCountry.Application.Interfaces
{
    public interface IBlockedAttemptLog
    {
        void LogAttempt(BlockedAttemptLogDto log);
        IEnumerable<BlockedAttemptLogDto> GetAll(int page = 1 ,int pagesize = 10 );
    }
}
