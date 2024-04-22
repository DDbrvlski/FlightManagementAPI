using FlightManagementDTO.DTOs.Flights;

namespace FlightManagementAPI.Interfaces.Services.Flights
{
    public interface IFlightService
    {
        Task CreateFlightAsync(FlightPostDTO flightData);
        Task DeactivateFlightAsync(int flightId);
        Task<IEnumerable<FlightDTO>> GetAllFlightsAsync();
        Task<FlightDetailsDTO> GetFlightDetailsAsync(int flightId);
        Task UpdateFlightAsync(FlightPutDTO flightData);
    }
}