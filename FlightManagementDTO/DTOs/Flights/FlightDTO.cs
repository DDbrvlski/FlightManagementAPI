using FlightManagementDTO.DTOs.Airports;

namespace FlightManagementDTO.DTOs.Flights
{
    public class FlightDTO
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; }
        public DateTime DepartureTime { get; set; }
        public AirportDTO DepartureAirport { get; set; }
        public AirportDTO ArrivalAirport { get; set; }
    }
}
