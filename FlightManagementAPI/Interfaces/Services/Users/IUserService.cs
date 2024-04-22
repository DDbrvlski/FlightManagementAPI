using FlightManagementDTO.DTOs.Users;

namespace FlightManagementAPI.Interfaces.Services.Users
{
    public interface IUserService
    {
        Task CreateUserAsync(UserPostDTO userData);
        Task ValidateUserFieldsAsync(string username, string email);
    }
}