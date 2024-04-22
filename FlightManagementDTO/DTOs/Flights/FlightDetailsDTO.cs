using FlightManagementDTO.DTOs.AirplaneType;
using FlightManagementDTO.DTOs.Airports;

namespace FlightManagementDTO.DTOs.Flights
{
    public class FlightDetailsDTO
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; }
        public DateTime DepartureTime { get; set; }
        public AirportDetailsDTO DepartureAirport { get; set; }
        public AirportDetailsDTO ArrivalAirport { get; set; }
        public AirplaneTypeDTO AirplaneType { get; set; }
    }
}
