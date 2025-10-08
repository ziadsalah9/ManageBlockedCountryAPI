using ManageBlockedCountry.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageBlockedCountry.Application.Interfaces
{
    public interface ITemporaryBlockedCountry
    {



        public void Add(string countrycode, int Durtation);
        bool isTemporaryBlocked(string countrycode);
        void RemoveExpired();
        IEnumerable<GetAllTemporaryCountryWithDatenotIntDto> GetAll();


    }
}
