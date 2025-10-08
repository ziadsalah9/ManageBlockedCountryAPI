using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageBlockedCountry.Application.Validation
{
    public class CountryCodeAttribute :ValidationAttribute
     {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            if (value is not string code || code.Length != 2)
                return new ValidationResult("CountryCode must be exactly 2 letters.");

            if (!code.All(char.IsLetter))
            {
                return new ValidationResult("CountryCode contain only letters ");

            }

            return ValidationResult.Success;
        }

    }
}
