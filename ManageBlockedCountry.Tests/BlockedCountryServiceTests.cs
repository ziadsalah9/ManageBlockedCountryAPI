using ManageBlockedCountry.Application.Dtos;
using ManageBlockedCountry.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageBlockedCountry.Tests
{



    
    public class BlockedCountryServiceTests
    {
        private readonly BlockedCountryService _service;

        public BlockedCountryServiceTests()
        {
            _service = new BlockedCountryService();
        }

        [Fact]
        public async Task AddAndGetBlockedCountry_souldAdded ()
        {
            var dto = new CreateCountryDto { Code = "eg", Name = "Egypt" };
            await _service.Add(dto);


            await _service.Add(dto);
            var got = _service.Get("EG");

            if (got == null)
                throw new Exception("Expected country to be found, but got null.");

            if (got.Code != "EG")
                throw new Exception($"Expected Code = 'EG' but got '{got.Code}'.");

            if (got.Name != "Egypt")
                throw new Exception($"Expected Name = 'Egypt' but got '{got.Name}'.");

        }


        [Fact]
        public async Task RemoveCountry_souldRemoved()
        {
            
            var dto = new CreateCountryDto { Code = "US", Name = "USA" };
            await _service.Add(dto);

            await _service.Remove("US");
            var got = _service.Get("US");

        }

        [Fact]
        public async Task IsBlocked_ShouldReturnTrueIfCountryIsBlocked_true()
        {
            
            var service = new BlockedCountryService();
            var dto = new CreateCountryDto { Code = "RU", Name = "RUSSIA" };
            await service.Add(dto);

            var isBlocked = service.IsBlocked("RU");

            if (!isBlocked)
                throw new Exception("Expected IsBlocked to be TRUE for country code 'RU', but got FALSE.");
        }


        [Fact]
        public void IsBlocked_ShouldReturnFalseIfCountryNotBlocked_false()
        {
            var isBlocked = _service.IsBlocked("FR");

            if (isBlocked)
                throw new Exception("Expected IsBlocked to be FALSE for country code 'FR', but got TRUE.");
        }
    }
}
