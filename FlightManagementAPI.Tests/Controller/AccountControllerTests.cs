using AutoFixture;
using FlightManagementAPI.Controllers.Accounts;
using FlightManagementAPI.Interfaces.Services.Auths;
using FlightManagementDTO.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace FlightManagementAPI.Tests.Controller
{
    public class AccountControllerTests
    {
        private readonly Mock<IAuthSerivce> _authSerivce;
        private readonly AccountController _accountController;
        private readonly Fixture _fixture;
        public AccountControllerTests()
        {
            _fixture = new Fixture();
            _authSerivce = new Mock<IAuthSerivce>();
            _accountController = new AccountController(_authSerivce.Object);
        }

        [Fact]
        public void AccountController_Register_InvalidUsernameData_ThrowsBadRequestException()
        {
            var registerDataInvalidUsernameFormat = new RegisterDataDTO { Username = "Zl@n", Email = "example@example.com", Password = "Test!123" };
            var registerDataNullUsername = new RegisterDataDTO { Email = "example@example.com", Password = "Test!123" };

            Assert.True(ValidateModel(registerDataInvalidUsernameFormat)
                .Any(x => x.MemberNames.Contains("Username") &&
                x.ErrorMessage.Contains("must match")));
            Assert.True(ValidateModel(registerDataNullUsername)
                .Any(x => x.MemberNames.Contains("Username") &&
                x.ErrorMessage.Contains("required")));
        }

        [Fact]
        public void AccountController_Register_InvalidEmailData_ThrowsBadRequestException()
        {
            var registerDataInvalidEmailFormat = new RegisterDataDTO { Username = "nazwa123", Email = "example", Password = "Test!123" };
            var registerDataNullEmail = new RegisterDataDTO { Username = "nazwa123", Password = "Test!123" };

            Assert.True(ValidateModel(registerDataInvalidEmailFormat)
                .Any(x => x.MemberNames.Contains("Email") &&
                x.ErrorMessage.Contains("valid")));
            Assert.True(ValidateModel(registerDataNullEmail)
                .Any(x => x.MemberNames.Contains("Email") &&
                x.ErrorMessage.Contains("required")));
        }

        [Fact]
        public void AccountController_Register_InvalidPasswordData_ThrowsBadRequestException()
        {
            var registerDataInvalidPasswordFormat = new RegisterDataDTO { Username = "nazwa123", Email = "example@example.com", Password = "test" };
            var registerDataNullPassword = new RegisterDataDTO { Username = "nazwa123", Email = "example@example.com" };

            Assert.True(ValidateModel(registerDataInvalidPasswordFormat)
                .Any(x => x.MemberNames.Contains("Password") &&
                x.ErrorMessage.Contains("must match")));
            Assert.True(ValidateModel(registerDataNullPassword)
                .Any(x => x.MemberNames.Contains("Password") &&
                x.ErrorMessage.Contains("required")));
        }

        [Fact]
        public void AccountController_Login_InvalidEmailData_ThrowsBadRequestException()
        {
            var loginDataInvalidEmailFromat = new LoginDataDTO { Email = "email", Password = "Test" };
            var loginDataNullEmail = new LoginDataDTO { Password = "Test" };

            Assert.True(ValidateModel(loginDataInvalidEmailFromat)
                .Any(x => x.MemberNames.Contains("Email") &&
                x.ErrorMessage.Contains("valid")));
            Assert.True(ValidateModel(loginDataNullEmail)
                .Any(x => x.MemberNames.Contains("Email") &&
                x.ErrorMessage.Contains("required")));
        }

        [Fact]
        public void AccountController_Login_InvalidPasswordData_ThrowsBadRequestException()
        {
            var loginDataNullPassword = new LoginDataDTO { Email = "email@example.com"};

            Assert.True(ValidateModel(loginDataNullPassword)
                .Any(x => x.MemberNames.Contains("Password") &&
                x.ErrorMessage.Contains("required")));
        }

        [Fact]
        public async Task AccountController_Register_ValidData_ReturnOk()
        {
            var registerData = new RegisterDataDTO { Email = "email@example.com", Username = "Test", Password = "Test!123" };

            _authSerivce.Setup(x => x.Register(registerData)).Returns(Task.CompletedTask);

            var result = await _accountController.Register(registerData);

            Assert.IsType<OkResult>(result);
        }
        [Fact]
        public async Task AccountController_Login_ValidData_ReturnOkToken()
        {
            var token = "token";

            _authSerivce.Setup(x => x.Login(It.IsAny<LoginDataDTO>())).ReturnsAsync(token);

            var result = await _accountController.Login(It.IsAny<LoginDataDTO>());

            Assert.IsType<OkObjectResult>(result.Result);
            var okObjectResult = result.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            Assert.IsType<string>(okObjectResult.Value);
            var returnedToken = okObjectResult.Value as string;
            Assert.Equal(token, returnedToken);
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}
