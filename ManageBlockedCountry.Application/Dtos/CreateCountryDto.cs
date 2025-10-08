namespace ManageBlockedCountry.Application.Dtos
{
    public class CreateCountryDto
    {

        public string Code { get; set; } = default;
        
        // could be null 
        public string? Name { get; set; } 
    }
}