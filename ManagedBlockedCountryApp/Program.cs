
using Hangfire;
using Hangfire.MemoryStorage;
using ManageBlockedCountry.Application.Interfaces;
using ManageBlockedCountry.Application.Jobs;
using ManageBlockedCountry.Application.Services;
using ManageBlockedCountry.Infrastructure.ExternalApiIntegration;

namespace ManagedBlockedCountryApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddSingleton<IBlockedCountry, BlockedCountryService>();
            builder.Services.AddHttpClient<ILocationOfCountry, LocationOfCountryService>();
            builder.Services.AddSingleton<ITemporaryBlockedCountry, TemporaryBlockedCountryService>();


            //            builder.Services.AddHttpContextAccessor();



            //BackGrongJObs 
            builder.Services.AddHangfire(config =>
            {
                config.UseMemoryStorage(); 
            });
            builder.Services.AddHangfireServer();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHangfireDashboard("/jobs");

            // Schedule recurring job every 5 minutes
            RecurringJob.AddOrUpdate<CalculateingtheReminingTimetoUnblock>(
                        "clean-expired-temp-blocks",
                            service => service.Run(),
                            Cron.Minutely

                  );

           

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
