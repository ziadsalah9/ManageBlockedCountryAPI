using ManageBlockedCountry.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageBlockedCountry.Application.Interfaces
{
    public interface IBlockedCountry
    {

        void Add(CreateCountryDto createCountryDto);
        void Remove(string code);

        IEnumerable<CountryDto> GetAll();
        CountryDto Get(string code);

        bool IsBlocked(string code);

        IEnumerable<CountryDto> GetAllAdv(int page, int pageSize, string? search = null);
    }
}
