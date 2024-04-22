using FlightManagementDTO.DTOs.Auth;

namespace FlightManagementAPI.Interfaces.Services.Auths
{
    public interface IAuthSerivce
    {
        Task<string> Login(LoginDataDTO loginData);
        Task Register(RegisterDataDTO registerData);
    }
}