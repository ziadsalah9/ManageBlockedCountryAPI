using ManageBlockedCountry.Application.Dtos;
using ManageBlockedCountry.Application.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageBlockedCountry.Application.Services
{
    public class TemporaryBlockedCountryService : ITemporaryBlockedCountry
    {


        private readonly ConcurrentDictionary<string, DateTime> _TempBlocked = new();
        private readonly IBlockedCountry _permanentBlocked;

        public TemporaryBlockedCountryService(IBlockedCountry permanentBlocked)
        {
            this._permanentBlocked = permanentBlocked;
            
        }
        public void Add(string countrycode, int DurtationMIN)
        {
                var code = countrycode.Trim().ToUpperInvariant();

            if (_permanentBlocked.IsBlocked(code)) throw new Exception($" Country {code} is permanently blocked ");
           if ( _TempBlocked.ContainsKey(code)) throw new Exception("country is temporaryblocked already");

           var expiredDateAt = DateTime.UtcNow.AddMinutes(DurtationMIN);
            
            _TempBlocked.TryAdd(code, expiredDateAt);

            
        }

        public IEnumerable<GetAllTemporaryCountryWithDatenotIntDto> GetAll()
        {


            return _TempBlocked.Select(p => new GetAllTemporaryCountryWithDatenotIntDto
            {
                countrycode = p.Key,
                Duration = p.Value
            }).ToList();

        }

        public bool isTemporaryBlocked(string countrycode)
        {
            var code = countrycode.Trim().ToUpperInvariant();

            if(_TempBlocked.TryGetValue(code, out var expiredDateAt))
            {
                if(DateTime.UtcNow<expiredDateAt) return true;

                _TempBlocked.TryRemove(code, out _);

            }
            return false;

        }

        public void RemoveExpired()
        {
            foreach (var kvp in _TempBlocked)
            {
                if (kvp.Value <= DateTime.UtcNow)
                    _TempBlocked.TryRemove(kvp.Key, out _);
            }
        }
    }
}
