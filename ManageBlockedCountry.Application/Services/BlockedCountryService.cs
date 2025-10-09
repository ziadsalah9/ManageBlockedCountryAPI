using ManageBlockedCountry.Application.Dtos;
using ManageBlockedCountry.Application.Interfaces;
using ManageBlockedCountry.Domian.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageBlockedCountry.Application.Services
{
    public class BlockedCountryService : IBlockedCountry
    {

        private readonly ConcurrentDictionary<string, Country> _blocked
            = new ConcurrentDictionary<string, Country>();

        public void Add(CreateCountryDto createCountryDto)
        {
            
   var code = createCountryDto.Code.Trim().ToUpperInvariant();
            _blocked.TryAdd(createCountryDto.Code.Trim().ToUpperInvariant(),
                new Country() { Code= createCountryDto.Code.Trim().ToUpperInvariant()
                ,Name=createCountryDto.Name ?? code}

                );
           
        }

        public CountryDto Get(string code)
        {



            if (code == null) return null
                    ;
            if(_blocked.TryGetValue(code.Trim().ToUpperInvariant(),out var country))
                return new CountryDto(country.Code, country.Name);
            return null;

        }

        public IEnumerable<CountryDto> GetAll()
        {

            return _blocked.Values.Select(p => new CountryDto (p.Code,p.Name)).ToList();
        }

        public IEnumerable<CountryDto> GetAllAdv(int page, int pageSize, string? search = null)
        {


            var query = _blocked.Values.AsQueryable();

            if(!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                query = query.Where(p => p.Code.ToLower().Contains(search) || p.Name.ToLower().Contains(search));
            }
            return query.Skip((page-1)*pageSize).Take(pageSize).Select(p=>new CountryDto { 
          
                Code = p.Code,
                Name = p.Name,

            }); 

           
        }

        public bool IsBlocked(string code)
        {

            return _blocked.ContainsKey(code.Trim().ToUpperInvariant());

        }

        public void Remove(string code)
        {

           _blocked.TryRemove(code.Trim().ToUpperInvariant(), out _);


        }


        //public IEnumerable<CountryDto> Search(string? search = null)
        //{
        //    var query = _blocked.Values.AsEnumerable();
        //    if (!string.IsNullOrWhiteSpace(search))
        //        query = query.Where(c => c.Code.Contains(search.ToUpperInvariant()));

        //    return query.Select(c => new CountryDto(c.Code, c.Name));
        //}
    }
}
