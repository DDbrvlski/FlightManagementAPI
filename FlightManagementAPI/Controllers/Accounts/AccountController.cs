using FlightManagementAPI.Infrastructure.Exceptions;
using FlightManagementAPI.Interfaces.Services.Auths;
using FlightManagementDTO.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagementAPI.Controllers.Accounts
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAuthSerivce authSerivce) : Controller
    {
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<string>> Login(LoginDataDTO loginData)
        {
            var token = await authSerivce.Login(loginData);
            return Ok(token);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register(RegisterDataDTO registerData)
        {
            await authSerivce.Register(registerData);

            return Ok();
        }
    }
}
