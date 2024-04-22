using AutoFixture;
using FlightManagementAPI.Infrastructure.Exceptions;
using FlightManagementAPI.Interfaces.Services.Users;
using FlightManagementAPI.Services.Users;
using FlightManagementData.Data;
using FlightManagementData.Models.Accounts;
using FlightManagementDTO.DTOs.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FlightManagementAPI.Tests.Service
{
    public class UserServiceTests
    {
        private readonly DbContextOptions<FlightManagementContext> _options;
        private readonly Mock<UserManager<User>> _userManager;
        private readonly Mock<IUserContextService> _userContextService;
        private readonly FlightManagementContext _context;
        private readonly UserService _userService;
        private readonly Fixture _fixture;
        public UserServiceTests()
        {
            _fixture = new Fixture();
            _options = new DbContextOptionsBuilder<FlightManagementContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new FlightManagementContext(_options);
            var userStore = new Mock<IUserStore<User>>();
            _userManager = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);
            _userContextService = new Mock<IUserContextService>();
            _userService = new UserService(_context, _userManager.Object, _userContextService.Object);
        }

        [Fact]
        public async Task CreateUserAsync_ValidUserData_CreatesUser()
        {
            var userData = new UserPostDTO { Email = "test@example.com", Username = "testuser", Password = "Test!123" };

            _userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                 .ReturnsAsync(IdentityResult.Success);

            await _userService.CreateUserAsync(userData);

            _userManager.Verify(x => x.CreateAsync(It.IsAny<User>(), userData.Password), Times.Once);
        }

        [Fact]
        public async Task CreateUserAsync_NullUserData_ThrowsBadRequestException()
        {
            UserPostDTO userData = null;

            await Assert.ThrowsAsync<BadRequestException>(() => _userService.CreateUserAsync(userData));
        }

        [Fact]
        public async Task ValidateUserFieldsAsync_UniqueUsernameAndEmail_NoExceptionsThrown()
        {
            var username = "user";
            var email = "email@example.com";

            _userContextService.Setup(x => x.GetUserByDataAsync(x => x.IsActive))
                                    .ReturnsAsync((User)null);

            await _userService.ValidateUserFieldsAsync(username, email);
        }

        [Fact]
        public async Task ValidateUserFieldsAsync_DuplicateUsername_ThrowsAccountException()
        {
            var existingUser = new User { UserName = "user123" };
            var username = "user123";

            _userContextService.Setup(x => x.GetUserByDataAsync(x => x.UserName == username))
                                    .ReturnsAsync(existingUser);

            await Assert.ThrowsAsync<AccountException>(() => 
                _userService.ValidateUserFieldsAsync(username, "new@example.com"));
        }

        [Fact]
        public async Task ValidateUserFieldsAsync_DuplicateEmail_ThrowsAccountException()
        {
            var existingUser = new User { Email = "email@example.com" };
            var email = "email@example.com";

            _userContextService.Setup(x => x.GetUserByDataAsync(x => x.Email == email))
                                    .ReturnsAsync(existingUser);

            await Assert.ThrowsAsync<AccountException>(() => 
                _userService.ValidateUserFieldsAsync("user", email));
        }
    }
}
