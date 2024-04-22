using FlightManagementDTO.DTOs.Address;

namespace FlightManagementDTO.DTOs.Airports
{
    public class AirportDetailsDTO
    {
        public int Id { get; set; }
        public string AirportName { get; set; }
        public CityDTO City { get; set; }
        public CountryDTO Country { get; set; }
    }
}
