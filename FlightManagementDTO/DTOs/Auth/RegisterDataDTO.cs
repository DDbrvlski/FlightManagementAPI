using System.ComponentModel.DataAnnotations;

namespace FlightManagementDTO.DTOs.Auth
{
    public class RegisterDataDTO
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9]{5,30}$")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{5,20}$")]
        public string Password { get; set; }
    }
}
