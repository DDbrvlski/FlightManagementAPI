using System.ComponentModel.DataAnnotations;

namespace FlightManagementDTO.DTOs.Auth
{
    public class LoginDataDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string? Audience { get; set; }
    }
}
