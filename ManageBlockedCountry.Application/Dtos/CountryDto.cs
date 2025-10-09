using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageBlockedCountry.Application.Dtos
{
    public class CountryDto
    {
        public string Code { get; set; }
       
        //not null 
        public string Name { get; set; }

        public CountryDto()
        {

        }
        public CountryDto(string code , string name)
        {
            Code = code;
            Name = name;
            
        }
       
    }
}
