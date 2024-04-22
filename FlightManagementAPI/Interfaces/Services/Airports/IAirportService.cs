using FlightManagementDTO.DTOs.Airports;

namespace FlightManagementAPI.Interfaces.Services.Airports
{
    public interface IAirportService
    {
        Task CreateAirportAsync(AirportPostDTO airportData);
        Task DeactivateAirportAsync(int airportId);
        Task<AirportDetailsDTO> GetAirportDetailsAsync(int airportId);
        Task<IEnumerable<AirportDTO>> GetAllAirportsAsync();
        Task UpdateAirportAsync(AirportPutDTO airportData);
    }
}