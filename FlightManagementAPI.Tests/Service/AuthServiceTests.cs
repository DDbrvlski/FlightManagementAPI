using FlightManagementAPI.Infrastructure.Exceptions;
using FlightManagementAPI.Interfaces.Services.Users;
using FlightManagementAPI.Services.Auths;
using FlightManagementData.Models.Accounts;
using FlightManagementDTO.DTOs.Auth;
using FlightManagementDTO.DTOs.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;

namespace FlightManagementAPI.Tests.Service
{
    public class AuthServiceTests
    {
        private Mock<IUserContextService> _userContextService;
        private Mock<UserManager<User>> _userManager;
        private Mock<IUserService> _userService;
        private Mock<IConfiguration> _configuration;
        private AuthSerivce _authService;
        public AuthServiceTests()
        {
            _userContextService = new Mock<IUserContextService>();
            var userStore = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);
            _userService = new Mock<IUserService>();
            _configuration = new Mock<IConfiguration>();
            _configuration.Setup(x => x["JWTKey:TokenExpiryTimeInHour"]).Returns("24");
            _configuration.Setup(x => x["JWTKey:Secret"]).Returns("127489271639862718y721y7bnfaw78427");

            _authService = new AuthSerivce(
                _userManager.Object,
                _userService.Object,
                _configuration.Object,
                _userContextService.Object
                );
        }

        [Fact]
        public async Task Login_ValidLoginData_ReturnsToken()
        {
            var loginData = new LoginDataDTO { Email = "email@example.com", Password = "Test!123" };
            var user = new User { Email = loginData.Email, UserName = "user", Id = "userId" };

            _userContextService.Setup(x => x.GetUserByDataAsync(x => x.Email == loginData.Email))
                                   .ReturnsAsync(user);
            _userManager.Setup(x => x.CheckPasswordAsync(user, loginData.Password))
                            .ReturnsAsync(true);
            

            var result = await _authService.Login(loginData);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.IsType<string>(result);
        }

        [Fact]
        public async Task Login_InvalidLoginData_ThrowsUnauthorizedException()
        {
            var loginData = new LoginDataDTO { Email = "email@example.com", Password = "Test!123" };
            var user = new User { Email = loginData.Email, UserName = "user", Id = "userId" };

            _userContextService.Setup(x => x.GetUserByDataAsync(x => x.Email == loginData.Email))
                                   .ReturnsAsync(user);
            _userManager.Setup(x => x.CheckPasswordAsync(user, loginData.Password))
                            .ReturnsAsync(false);


            await Assert.ThrowsAsync<UnauthorizedException>(() => _authService.Login(loginData));
        }

        [Fact]
        public async Task Login_InvalidLoginData_ThrowsBadRequestException()
        {
            LoginDataDTO loginData = null;
            await Assert.ThrowsAsync<BadRequestException>(() => _authService.Login(loginData));
        }

        [Fact]
        public async Task Register_ValidRegisterData_CreatesUser()
        {
            var registerData = new RegisterDataDTO { Username = "user", Email = "email@example.com", Password = "Test!123" };

            _userService.Setup(x => x.ValidateUserFieldsAsync(registerData.Username, registerData.Email))
                            .Verifiable();
            _userService.Setup(x => x.CreateUserAsync(It.IsAny<UserPostDTO>()))
                            .Verifiable();

            await _authService.Register(registerData);

            _userService.Verify();
        }

        [Fact]
        public async Task Register_InvalidRegisterData_ThrowsAccountException()
        {
            var registerData = new RegisterDataDTO { Username = "testuser", Email = "test@example.com", Password = "Test!123" };

            _userService.Setup(x => x.ValidateUserFieldsAsync(registerData.Username, registerData.Email))
                            .ThrowsAsync(new AccountException("Blad"));

            await Assert.ThrowsAsync<AccountException>(() => _authService.Register(registerData));
        }
    }
}
