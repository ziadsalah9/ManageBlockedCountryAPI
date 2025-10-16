# ManageBlockedCountryAPI(BD-Assignment)

 Overview

The Country Block Service is a .NET 8 Web API that determines whether a user’s IP address is blocked based on their country.
It integrates with a Geolocation API to resolve IP addresses into country information and provides endpoints for managing blocked countries.


 Features

1- CRUD operations for blocked countries

2- Detect client country by IP address

3- Block and unblock countries

4- Check if an IP or country is blocked


5- Clean architecture using Onion Architecture principles

6- Unit tests for core logic (blocking/unblocking)



Technologies Used

.NET 8 / ASP.NET Core Web API

Dependency Injection

HttpClientFactory

In-Memory Data Storage (for simplicity)

xUnit for Unit Testing


 Architecture (Onion Architecture)
ManageBlockedCountry.sln

    ManageBlockedCountryApp (API)           ->  Presentation Layer (Controllers, Endpoints)
    ManageBlockedCountry.Application   ->  Business Logic Layer (Services, Interfaces, DTOs)
    ManageBlockedCountry.Domain        ->  Core Entities & Models
    ManageBlockedCountry.Infrastructure ->  For persistence or external integrations

Layers Explanation

Domain: Contains core entities like Country, BlockedAttempt, etc.

Application: Contains interfaces (IBlockedCountry, etc.) and services (BlockedCountryService, TemporaryBlockedCountryService).

Api: Handles HTTP requests and responses using controllers.

Infrastructure: (Optional) Can be extended later for database or external integrations.



Method	             Endpoint	                                                              Description
POST	             /api/CountriesBlocked/block	                                          Add a new blocked country
DELETE	             /api/CountriesBlocked/block/{Country code}	                              Remove a country from block list
GET	                 /api/CountriesBlocked/blocked/getAll	                                  Retrieve all blocked countries
GET	                 /api/CountriesBlocked/blocked?page=1&pageSize=10&search=us	              Get paginated & searchable list
GET	                /api/ip/check-block	Check if user's IP is blocked
GET	                /lookup?ipAddress={ipAdress}	                         Lookup country and block status for a specific IP
GET                  /lookupWithGeoLocation?ipAddress={ipAddress}            Lookup country and block status for a specific IP
GET                 /api/Logs/blocked-attempts?page=1&pageSize=10           paginated list of all blocked access attempts
POST                /api/TemporaryBlockedCountry/temporal-block             Blocked Country Temporary
GET                 /api/TemporaryBlockedCountry/all                        Get All the Temporary countries  




 Notes

The API uses ipApi ipwho.is for IP geolocation becuse ipApi send me rate limiting 429 but ipwho.is is more simple and dosent have rate limit.

In-memory collections are used instead of a database for simplicity.

Easily extendable to include database or caching later.



 Author

Ziad Salah
ZiadS5933@gmail.com
https://github.com/ziadsalah9