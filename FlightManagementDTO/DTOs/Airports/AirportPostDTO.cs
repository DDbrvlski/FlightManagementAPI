using System.ComponentModel.DataAnnotations;

namespace FlightManagementDTO.DTOs.Airports
{
    public class AirportPostDTO
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9]{5,40}$")]
        public string AirportName { get; set; }
        [Required]
        public int CityId { get; set; }
        [Required]
        public int CountryId { get; set; }
    }
}
