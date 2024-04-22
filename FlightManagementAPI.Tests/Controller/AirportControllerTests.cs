using AutoFixture;
using FlightManagementAPI.Controllers.Airports;
using FlightManagementAPI.Interfaces.Services.Airports;
using FlightManagementDTO.DTOs.Airports;
using FlightManagementDTO.DTOs.Flights;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace FlightManagementAPI.Tests.Controller
{
    public class AirportControllerTests
    {
        private readonly Mock<IAirportService> _airportService;
        private readonly AirportController _airportController;
        private readonly Fixture _fixture;
        public AirportControllerTests()
        {
            _fixture = new Fixture();
            _airportService = new Mock<IAirportService>();
            _airportController = new AirportController(_airportService.Object);
        }

        [Fact]
        public async Task AirportController_GetAirports_ReturnOkAsync()
        {
            var airportsList = _fixture.CreateMany<AirportDTO>(5).ToList();
            _airportService.Setup(x => x.GetAllAirportsAsync()).ReturnsAsync(airportsList);

            var result = await _airportController.GetAirportsAsync();

            var objResult = result.Result as ObjectResult;
            Assert.Equal(200, objResult.StatusCode);
        }

        [Fact]
        public async Task AirportController_GetSingleAirport_ReturnOkAsync()
        {
            var airport = _fixture.Create<AirportDetailsDTO>();
            _airportService.Setup(x => x.GetAirportDetailsAsync(It.IsAny<int>())).ReturnsAsync(airport);

            var result = await _airportController.GetAirportDetailsAsync(It.IsAny<int>());

            var objResult = result.Result as ObjectResult;
            Assert.Equal(200, objResult.StatusCode);
        }

        [Fact]
        public async Task AirportController_PostAirport_ValidData_ReturnCreatedAsync()
        {
            _airportService.Setup(x => x.CreateAirportAsync(It.IsAny<AirportPostDTO>()))
                .Returns(Task.CompletedTask);

            var result = await _airportController.CreateAirportAsync(It.IsAny<AirportPostDTO>());

            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task AirportController_PostAirport_InvalidAirportName_ThrowsBadRequestException()
        {
            var airportInvalidNumber = _fixture.Create<AirportPostDTO>();
            airportInvalidNumber.AirportName = "@aw";

            Assert.True(ValidateModel(airportInvalidNumber)
                .Any(x => x.MemberNames.Contains("AirportName") &&
                x.ErrorMessage.Contains("expression")));
        }

        [Fact]
        public async Task AirportController_PostAirport_InvalidNullAirportName_ThrowsBadRequestException()
        {
            var airportNullNumber = _fixture.Create<AirportPostDTO>();
            airportNullNumber.AirportName = null;

            Assert.True(ValidateModel(airportNullNumber)
                .Any(x => x.MemberNames.Contains("AirportName") &&
                x.ErrorMessage.Contains("required")));
        }

        [Fact]
        public async Task AirportController_PutAirport_ValidData_ReturnNoContentAsync()
        {
            _airportService.Setup(x => x.UpdateAirportAsync(It.IsAny<AirportPutDTO>()))
                .Returns(Task.CompletedTask);

            var result = await _airportController.EditAirportAsync(It.IsAny<AirportPutDTO>());

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task AirportController_PutAirport_InvalidAirportName_ThrowsBadRequestException()
        {
            var airportInvalidNumber = _fixture.Create<AirportPutDTO>();
            airportInvalidNumber.AirportName = "@aw";

            Assert.True(ValidateModel(airportInvalidNumber)
                .Any(x => x.MemberNames.Contains("AirportName") &&
                x.ErrorMessage.Contains("expression")));
        }

        [Fact]
        public async Task AirportController_PutAirport_InvalidNullAirportName_ThrowsBadRequestException()
        {
            var airportNullNumber = _fixture.Create<AirportPutDTO>();
            airportNullNumber.AirportName = null;

            Assert.True(ValidateModel(airportNullNumber)
                .Any(x => x.MemberNames.Contains("AirportName") &&
                x.ErrorMessage.Contains("required")));
        }

        [Fact]
        public async Task AirportController_DeleteAirport_ReturnNoContentAsync()
        {
            _airportService.Setup(x => x.DeactivateAirportAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            var result = await _airportController.DeactivateAirportAsync(It.IsAny<int>());

            Assert.IsType<NoContentResult>(result);
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
