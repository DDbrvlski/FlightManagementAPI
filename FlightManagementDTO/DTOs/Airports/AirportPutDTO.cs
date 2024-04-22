using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagementDTO.DTOs.Airports
{
    public class AirportPutDTO
    {
        [Required]
        public int AirportId { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9]{5,40}$")]
        public string AirportName { get; set; }
        [Required]
        public int CityId { get; set; }
        [Required]
        public int CountryId { get; set; }
    }
}
