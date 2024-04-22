using System.ComponentModel.DataAnnotations;

namespace FlightManagementDTO.DTOs.Flights
{
    public class FlightPostDTO
    {
        [Required]
        [MinLength(1)]
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
