using FlightManagementAPI.Helpers.DatabaseOperations;
using FlightManagementAPI.Infrastructure.Exceptions;
using FlightManagementAPI.Interfaces.Services.Airports;
using FlightManagementData.Data;
using FlightManagementData.Models.Airports;
using FlightManagementDTO.DTOs.Address;
using FlightManagementDTO.DTOs.Airports;
using Microsoft.EntityFrameworkCore;

namespace FlightManagementAPI.Services.Airports
{
    public class AirportService
        (FlightManagementContext context)
        : IAirportService
    {
        public async Task<IEnumerable<AirportDTO>> GetAllAirportsAsync()
        {
            var airports = await context.Airport
                .Where(x => x.IsActive)
                .Select(x => new AirportDTO()
                {
                    Id = x.Id,
                    AirportName = x.AirportName,
                }).ToListAsync();
            return airports;
        }
        public async Task<AirportDetailsDTO> GetAirportDetailsAsync(int airportId)
        {
            var airport = await context.Airport
                .Where(x => x.IsActive && x.Id == airportId)
                .Select(x => new AirportDetailsDTO()
                {
                    Id = x.Id,
                    AirportName = x.AirportName,
                    City = new CityDTO()
                    {
                        Id = x.CityID,
                        CityName = x.City.Name,
                    },
                    Country = new CountryDTO()
                    {
                        Id = x.CountryID,
                        CountryName = x.Country.Name
                    }
                }).FirstOrDefaultAsync();
            if(airport == null)
            {
                throw new NotFoundException("Nie znaleziono");
            }
            return airport;
        }
        public async Task CreateAirportAsync(AirportPostDTO airportData)
        {
            var airport = new Airport()
            {
                AirportName = airportData.AirportName,
                CityID = airportData.CityId,
                CountryID = airportData.CountryId
            };

            await context.Airport.AddAsync(airport);
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task UpdateAirportAsync(AirportPutDTO airportData)
        {
            var airport = await context.Airport
                .Where(x => x.IsActive && x.Id == airportData.AirportId)
                .FirstAsync();

            airport.AirportName = airportData.AirportName;
            airport.CountryID = airportData.CountryId;
            airport.CityID = airportData.CityId;
            airport.ModifiedDate = DateTime.UtcNow;

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task DeactivateAirportAsync(int airportId)
        {
            var airport = await context.Airport
                .Where(x => x.IsActive && x.Id == airportId)
                .FirstAsync();

            airport.IsActive = false;
            airport.ModifiedDate = DateTime.UtcNow;

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
    }
}
