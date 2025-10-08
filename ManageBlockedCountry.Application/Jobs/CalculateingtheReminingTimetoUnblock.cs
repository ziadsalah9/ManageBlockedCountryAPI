using ManageBlockedCountry.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageBlockedCountry.Application.Jobs
{
    public class CalculateingtheReminingTimetoUnblock
    {

        private readonly ITemporaryBlockedCountry _temporaryBlockedCountry;

        public CalculateingtheReminingTimetoUnblock(ITemporaryBlockedCountry temporaryBlockedCountry)
        {

            
            _temporaryBlockedCountry = temporaryBlockedCountry;
        }


        public void Run() {


            var blockedTenpCountry = _temporaryBlockedCountry.GetAll();

            foreach (var item in blockedTenpCountry)
            {

                var code = item.countrycode;
                var duration = item.Duration;


                if (DateTime.UtcNow > duration)
                {
                    Console.WriteLine($"{code} is unblocked");
                    _temporaryBlockedCountry.RemoveExpired();

                }
                else
                {
                    var remining = duration - DateTime.UtcNow;
                    Console.WriteLine($"{code} will unblock in {remining.Minutes}m {remining.Seconds}s");

                }


            }
        

            
        }



    }
}
