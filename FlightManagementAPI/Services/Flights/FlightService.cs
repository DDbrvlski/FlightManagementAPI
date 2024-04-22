using FlightManagementAPI.Helpers.DatabaseOperations;
using FlightManagementAPI.Infrastructure.Exceptions;
using FlightManagementAPI.Interfaces.Services.Flights;
using FlightManagementData.Data;
using FlightManagementData.Models.Flights;
using FlightManagementDTO.DTOs.Address;
using FlightManagementDTO.DTOs.AirplaneType;
using FlightManagementDTO.DTOs.Airports;
using FlightManagementDTO.DTOs.Flights;
using Microsoft.EntityFrameworkCore;

namespace FlightManagementAPI.Services.Flights
{
    public class FlightService
        (FlightManagementContext context)
        : IFlightService
    {
        public async Task<IEnumerable<FlightDTO>> GetAllFlightsAsync()
        {
            var flights = await context.Flight
                .Where(x => x.IsActive)
                .Select(x => new FlightDTO()
                {
                    Id = x.Id,
                    FlightNumber = x.FlightNumber,
                    DepartureTime = x.DepartureDate,
                    DepartureAirport = new AirportDTO()
                    {
                        Id = x.DepartureAirpotID,
                        AirportName = x.DepartureAirpot.AirportName,
                    },
                    ArrivalAirport = new AirportDTO()
                    {
                        Id = x.ArrivalAirportID,
                        AirportName = x.ArrivalAirport.AirportName,
                    }
                }).ToListAsync();
            return flights;
        }
        public async Task<FlightDetailsDTO> GetFlightDetailsAsync(int flightId)
        {
            var flight = await context.Flight
                .Where(x => x.IsActive && x.Id == flightId)
                .Select(x => new FlightDetailsDTO()
                {
                    Id = x.Id,
                    FlightNumber = x.FlightNumber,
                    DepartureTime = x.DepartureDate,
                    DepartureAirport = new AirportDetailsDTO()
                    {
                        Id = x.DepartureAirpotID,
                        AirportName = x.DepartureAirpot.AirportName,
                        City = new CityDTO()
                        {
                            Id = x.DepartureAirpot.CityID,
                            CityName = x.DepartureAirpot.City.Name,
                        },
                        Country = new CountryDTO()
                        {
                            Id = x.DepartureAirpot.CountryID,
                            CountryName = x.DepartureAirpot.Country.Name,
                        }
                    },
                    ArrivalAirport = new AirportDetailsDTO()
                    {
                        Id = x.ArrivalAirportID,
                        AirportName = x.ArrivalAirport.AirportName,
                        City = new CityDTO()
                        {
                            Id = x.ArrivalAirport.CityID,
                            CityName = x.ArrivalAirport.City.Name,
                        },
                        Country = new CountryDTO()
                        {
                            Id = x.ArrivalAirport.CountryID,
                            CountryName = x.ArrivalAirport.Country.Name,
                        }
                    },
                    AirplaneType = new AirplaneTypeDTO()
                    {
                        Id = x.AirplaneTypeID,
                        AirplaneTypeName = x.AirplaneType.Name
                    }
                }).FirstOrDefaultAsync();

            if(flight == null)
            {
                throw new NotFoundException("Nie znaleziono lotu");
            }
            return flight;
        }
        public async Task CreateFlightAsync(FlightPostDTO flightData)
        {
            var flight = new Flight()
            {
                FlightNumber = flightData.FlightNumber,
                DepartureDate = flightData.DepartureTime,
                DepartureAirpotID = flightData.DepartureAirportId,
                ArrivalAirportID = flightData.ArrivalAirportId,
                AirplaneTypeID = flightData.AirplaneTypeId
            };

            await context.Flight.AddAsync(flight);
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task UpdateFlightAsync(FlightPutDTO flightData)
        {
            var flight = await context.Flight
                .Where(x => x.IsActive && x.Id == flightData.FlightId)
                .FirstAsync();

            flight.FlightNumber = flightData.FlightNumber;
            flight.DepartureDate = flightData.DepartureTime;
            flight.DepartureAirpotID = flightData.DepartureAirportId;
            flight.ArrivalAirportID = flightData.ArrivalAirportId;
            flight.AirplaneTypeID = flightData.AirplaneTypeId;
            flight.ModifiedDate = DateTime.UtcNow;

            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
        public async Task DeactivateFlightAsync(int flightId)
        {
            var flight = await context.Flight
                .Where(x => x.IsActive && x.Id == flightId)
                .FirstAsync();

            flight.IsActive = false;
            flight.ModifiedDate = DateTime.UtcNow;
            await DatabaseOperationHandler.TryToSaveChangesAsync(context);
        }
    }
}
