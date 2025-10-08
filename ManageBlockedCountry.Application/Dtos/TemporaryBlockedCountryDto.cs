using ManageBlockedCountry.Application.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageBlockedCountry.Application.Dtos
{
    public class TemporaryBlockedCountryDto
    {

        [Required]
        [CountryCode]
        public string countrycode { get; set; } = default;


        [Required]
        [Range(1,1440 , ErrorMessage ="Duration time must be 1 to 1440 minutes")]
        

        public int Duration{ get; set; }
    }
}
