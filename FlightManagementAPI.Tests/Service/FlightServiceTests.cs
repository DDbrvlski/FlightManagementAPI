using AutoFixture;
using FlightManagementAPI.Infrastructure.Exceptions;
using FlightManagementAPI.Interfaces.Services.Airports;
using FlightManagementAPI.Services.Airports;
using FlightManagementAPI.Services.Flights;
using FlightManagementData.Data;
using FlightManagementData.Models.Airports;
using FlightManagementData.Models.Flights;
using FlightManagementDTO.DTOs.Airports;
using FlightManagementDTO.DTOs.Flights;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagementAPI.Tests.Service
{
    public class FlightServiceTests
    {
        private readonly DbContextOptions<FlightManagementContext> _options;
        private readonly FlightManagementContext _context;
        private readonly FlightService _flightService;
        private readonly Fixture _fixture;
        public FlightServiceTests()
        {
            _fixture = new Fixture();
            _options = new DbContextOptionsBuilder<FlightManagementContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new FlightManagementContext(_options);
            _flightService = new FlightService(_context);
        }

        [Fact]
        public async Task GetFlights_ReturnFlights()
        {
            var flightsList = _fixture.CreateMany<Flight>(5).ToList();
            //Ustawienie flagi isactive pierwszego elementu, aby zawsze coś się wyświetliło
            var fli = flightsList.First();
            fli.IsActive = true;

            await _context.Flight.AddRangeAsync(flightsList);
            await _context.SaveChangesAsync();

            var result = await _flightService.GetAllFlightsAsync();

            Assert.NotNull(result);
            Assert.Equal(flightsList.Count(x => x.IsActive), result.Count());
            foreach (var flight in flightsList.Where(x => x.IsActive))
            {
                Assert.Contains(result, x => x.Id == flight.Id);
            }
        }

        [Fact]
        public async Task GetFlightById_FlightExists_ReturnFlight()
        {
            var flight = _fixture.Create<Flight>();
            flight.IsActive = true;
            var flightName = flight.FlightNumber;

            await _context.Flight.AddAsync(flight);
            await _context.SaveChangesAsync();

            var result = await _flightService.GetFlightDetailsAsync(flight.Id);

            Assert.NotNull(result);
            Assert.Equal(flightName, result.FlightNumber);
        }

        [Fact]
        public async Task GetFlightById_FlightNotExists_ThrowsNotFoundException()
        {
            await Assert.ThrowsAsync<NotFoundException>(async () => await _flightService.GetFlightDetailsAsync(0));
        }

        [Fact]
        public async Task PostFlight_FlightAddedSuccessfully()
        {
            var flight = _fixture.Create<FlightPostDTO>();

            await _flightService.CreateFlightAsync(flight);

            var addedFlight = await _context.Flight.Where(x => x.FlightNumber == flight.FlightNumber).FirstOrDefaultAsync();
            Assert.NotNull(addedFlight);
        }

        [Fact]
        public async Task PutFlight_FlightUpdatedSuccessfully()
        {
            var flight = _fixture.Create<Flight>();
            flight.IsActive = true;

            await _context.Flight.AddAsync(flight);
            await _context.SaveChangesAsync();

            var updatedFlight = _fixture.Create<FlightPutDTO>();
            updatedFlight.FlightId = flight.Id;

            await _flightService.UpdateFlightAsync(updatedFlight);

            var updatedFlightFromDb = await _context.Flight.FindAsync(flight.Id);
            Assert.NotNull(updatedFlightFromDb);
            Assert.Equal(updatedFlight.FlightNumber, updatedFlightFromDb.FlightNumber);
            Assert.Equal(updatedFlight.AirplaneTypeId, updatedFlightFromDb.AirplaneTypeID);
            Assert.Equal(updatedFlight.ArrivalAirportId, updatedFlightFromDb.ArrivalAirportID);
            Assert.Equal(updatedFlight.DepartureAirportId, updatedFlightFromDb.DepartureAirpotID);
        }

        [Fact]
        public async Task DeleteFlight_FlightDeletedSuccessfully()
        {
            var flight = _fixture.Create<Flight>();
            flight.IsActive = true;

            await _context.Flight.AddAsync(flight);
            await _context.SaveChangesAsync();

            await _flightService.DeactivateFlightAsync(flight.Id);

            var deactivatedFlight = await _context.Flight.FindAsync(flight.Id);
            Assert.NotNull(deactivatedFlight);
            Assert.False(deactivatedFlight.IsActive);
        }
    }
}
