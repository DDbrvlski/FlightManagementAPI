using FlightManagementAPI.Infrastructure.Exceptions;
using FlightManagementAPI.Interfaces.Services.Auths;
using FlightManagementAPI.Interfaces.Services.Users;
using FlightManagementData.Data;
using FlightManagementData.Models.Accounts;
using FlightManagementDTO.DTOs.Auth;
using FlightManagementDTO.DTOs.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlightManagementAPI.Services.Auths
{
    public class AuthSerivce
        (UserManager<User> userManager,
        IUserService userService,
        IConfiguration configuration,
        IUserContextService userContextService) : IAuthSerivce
    {
        private readonly SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTKey:Secret"]));
        private readonly long tokenExpiryTimeInHours = Convert.ToInt64(configuration["JWTKey:TokenExpiryTimeInHour"]);
        public async Task<string> Login(LoginDataDTO loginData)
        {
            if (loginData == null)
            {
                throw new BadRequestException("Wystąpił błąd");
            }

            var user = await userContextService.GetUserByDataAsync(x => x.Email == loginData.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, loginData.Password))
            {
                throw new UnauthorizedException("Wprowadzono błędne dane logowania");
            }

            string audience = loginData.Audience;
            if (audience.IsNullOrEmpty())
            {
                audience = "API";
            }
            string token = GenerateTokenAsync(user, audience);
            return token;
        }

        public async Task Register(RegisterDataDTO registerData)
        {
            await userService.ValidateUserFieldsAsync(registerData.Username, registerData.Email);

            await userService.CreateUserAsync(new UserPostDTO()
            {
                Username = registerData.Username,
                Email = registerData.Email,
                Password = registerData.Password,
            });
        }

        private string GenerateTokenAsync(User user, string audience)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = configuration["JWTKey:ValidIssuer"],
                Audience = configuration["Audiences:" + audience],
                Expires = DateTime.UtcNow.AddHours(tokenExpiryTimeInHours),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(authClaims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
