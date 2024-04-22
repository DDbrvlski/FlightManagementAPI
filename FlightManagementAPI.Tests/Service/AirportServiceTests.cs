using AutoFixture;
using FlightManagementAPI.Infrastructure.Exceptions;
using FlightManagementAPI.Services.Airports;
using FlightManagementData.Data;
using FlightManagementData.Models.Airports;
using FlightManagementDTO.DTOs.Airports;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagementAPI.Tests.Service
{
    public class AirportServiceTests
    {
        private readonly DbContextOptions<FlightManagementContext> _options;
        private readonly FlightManagementContext _context;
        private readonly AirportService _airportService;
        private readonly Fixture _fixture;
        public AirportServiceTests()
        {
            _fixture = new Fixture();
            _options = new DbContextOptionsBuilder<FlightManagementContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new FlightManagementContext(_options);
            _airportService = new AirportService(_context);
        }

        [Fact]
        public async Task GetAirports_ReturnAirports()
        {
            var airportsList = _fixture.CreateMany<Airport>(5).ToList();
            //Ustawienie flagi isactive pierwszego elementu, aby zawsze coś się wyświetliło
            var air = airportsList.First();
            air.IsActive = true;

            await _context.Airport.AddRangeAsync(airportsList);
            await _context.SaveChangesAsync();

            var result = await _airportService.GetAllAirportsAsync();

            Assert.NotNull(result);
            Assert.Equal(airportsList.Count(x => x.IsActive), result.Count());
            foreach(var airport in airportsList.Where(x => x.IsActive))
            {
                Assert.Contains(result, x => x.Id == airport.Id);
            }
        }

        [Fact]
        public async Task GetAirportById_AirportExists_ReturnAirport()
        {
            var airport = _fixture.Create<Airport>();
            airport.IsActive = true;
            var airportName = airport.AirportName;

            await _context.Airport.AddAsync(airport);
            await _context.SaveChangesAsync();

            var result = await _airportService.GetAirportDetailsAsync(airport.Id);

            Assert.NotNull(result);
            Assert.Equal(airportName, result.AirportName);
        }

        [Fact]
        public async Task GetAirportById_AirportNotExists_ThrowsNotFoundException()
        {
            await Assert.ThrowsAsync<NotFoundException>(async () => await _airportService.GetAirportDetailsAsync(0));
        }

        [Fact]
        public async Task PostAirport_AirportAddedSuccessfully()
        {
            var airport = _fixture.Create<AirportPostDTO>();

            await _airportService.CreateAirportAsync(airport);

            var addedAirport = await _context.Airport.Where(x => x.AirportName == airport.AirportName).FirstOrDefaultAsync();
            Assert.NotNull(addedAirport);
        }

        [Fact]
        public async Task PutAirport_AirportUpdatedSuccessfully()
        {
            var airport = _fixture.Create<Airport>();
            airport.IsActive = true;

            await _context.Airport.AddAsync(airport);
            await _context.SaveChangesAsync();

            var updatedAirport = _fixture.Create<AirportPutDTO>();
            updatedAirport.AirportId = airport.Id;

            await _airportService.UpdateAirportAsync(updatedAirport);

            var updatedAirportFromDb = await _context.Airport.FindAsync(airport.Id);
            Assert.NotNull(updatedAirportFromDb);
            Assert.Equal(updatedAirport.AirportName, updatedAirportFromDb.AirportName);
            Assert.Equal(updatedAirport.CityId, updatedAirportFromDb.CityID);
            Assert.Equal(updatedAirport.CountryId, updatedAirportFromDb.CountryID);
        }

        [Fact]
        public async Task DeleteAirport_AirportDeletedSuccessfully()
        {
            var airport = _fixture.Create<Airport>();
            airport.IsActive = true;

            await _context.Airport.AddAsync(airport);
            await _context.SaveChangesAsync();

            await _airportService.DeactivateAirportAsync(airport.Id);

            var deactivatedAirport = await _context.Airport.FindAsync(airport.Id);
            Assert.NotNull(deactivatedAirport);
            Assert.False(deactivatedAirport.IsActive);
        }
    }
}
