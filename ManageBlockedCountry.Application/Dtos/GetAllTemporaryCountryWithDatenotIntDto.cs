using ManageBlockedCountry.Application.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageBlockedCountry.Application.Dtos
{
    public class GetAllTemporaryCountryWithDatenotIntDto
    {


        [Required]
        public string countrycode { get; set; } = default;


        [Required]


        public DateTime Duration { get; set; }
    }
}
