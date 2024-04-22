using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagementDTO.DTOs.Flights
{
    public class FlightPutDTO
    {
        [Required]
        public int FlightId { get; set; }
        [Required]
        public string FlightNumber { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        [CustomValidation(typeof(FlightPostDTO), "ValidateDepartureTime")]
        public DateTime DepartureTime { get; set; }
        [Required]
        public int DepartureAirportId { get; set; }
        [Required]
        public int ArrivalAirportId { get; set; }
        [Required]
        public int AirplaneTypeId { get; set; }

        public static ValidationResult ValidateDepartureTime(DateTime departureTime, ValidationContext context)
        {
            if (departureTime.Year <= 2000)
            {
                return new ValidationResult("Data musi być po 2000 roku.");
            }

            return ValidationResult.Success;
        }
    }
}
