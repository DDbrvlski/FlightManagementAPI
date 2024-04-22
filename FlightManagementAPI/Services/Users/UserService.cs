using FlightManagementAPI.Infrastructure.Exceptions;
using FlightManagementAPI.Interfaces.Services.Users;
using FlightManagementData.Data;
using FlightManagementData.Models.Accounts;
using FlightManagementDTO.DTOs.Users;
using Microsoft.AspNetCore.Identity;

namespace FlightManagementAPI.Services.Users
{
    public class UserService
        (FlightManagementContext context,
        UserManager<User> userManager,
        IUserContextService userContextService) 
        : IUserService
    {
        public async Task CreateUserAsync(UserPostDTO userData)
        {
            if (userData == null)
            {
                throw new BadRequestException("Wystąpił błąd");
            }

            User user = new()
            {
                Email = userData.Email,
                UserName = userData.Username
            };

            var createUserResult = await userManager.CreateAsync(user, userData.Password);
            if (!createUserResult.Succeeded)
            {
                throw new AccountException("Wystąpił błąd podczas tworzenia konta użytkownika.");
            }
        }
        public async Task ValidateUserFieldsAsync(string username, string email)
        {
            List<string> errorMessages = new();

            var isUsernameValid = await userContextService.GetUserByDataAsync(x => x.UserName == username);
            if (isUsernameValid != null)
            {
                errorMessages.Add("Podana nazwa użytkownika jest już zajęta.");
            }

            var isEmailValid = await userContextService.GetUserByDataAsync(x => x.Email == email);
            if (isEmailValid != null)
            {
                errorMessages.Add("Podany email jest już zajęty.");
            }

            if (errorMessages.Any())
            {
                throw new AccountException(errorMessages);
            }
        }
    }
}
