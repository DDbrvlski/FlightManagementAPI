using AutoFixture;
using FlightManagementAPI.Controllers.Flights;
using FlightManagementAPI.Interfaces.Services.Flights;
using FlightManagementDTO.DTOs.Flights;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace FlightManagementAPI.Tests.Controller
{
    public class FlightControllerTests
    {
        private readonly Mock<IFlightService> _flightService;
        private readonly FlightController _flightController;
        private readonly Fixture _fixture;
        public FlightControllerTests()
        {
            _fixture = new Fixture();
            _flightService = new Mock<IFlightService>();
            _flightController = new FlightController(_flightService.Object);
        }

        [Fact]
        public async Task FlightController_GetFlights_ReturnOkAsync()
        {
            var flightsList = _fixture.CreateMany<FlightDTO>(5).ToList();
            _flightService.Setup(x => x.GetAllFlightsAsync()).ReturnsAsync(flightsList);

            var result = await _flightController.GetFlightsAsync();

            var objResult = result.Result as ObjectResult;
            Assert.Equal(200, objResult.StatusCode);
        }

        [Fact]
        public async Task FlightController_GetSingleFlight_ReturnOkAsync()
        {
            var flight = _fixture.Create<FlightDetailsDTO>();
            _flightService.Setup(x => x.GetFlightDetailsAsync(It.IsAny<int>())).ReturnsAsync(flight);

            var result = await _flightController.GetFlightDetailsAsync(It.IsAny<int>());

            var objResult = result.Result as ObjectResult;
            Assert.Equal(200, objResult.StatusCode);
        }

        [Fact]
        public async Task FlightController_PostFlight_ValidData_ReturnCreatedAsync()
        {
            _flightService.Setup(x => x.CreateFlightAsync(It.IsAny<FlightPostDTO>()))
                .Returns(Task.CompletedTask);

            var result = await _flightController.CreateFlightAsync(It.IsAny<FlightPostDTO>());

            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task FlightController_PostFlight_InvalidFlightNumber_ThrowsBadRequestException()
        {
            var flightNullNumber = _fixture.Create<FlightPostDTO>();
            flightNullNumber.FlightNumber = null;
            var flightEmptyNumber = _fixture.Create<FlightPostDTO>();
            flightEmptyNumber.FlightNumber = "";

            Assert.True(ValidateModel(flightNullNumber)
                .Any(x => x.MemberNames.Contains("FlightNumber") &&
                x.ErrorMessage.Contains("required")));
            Assert.True(ValidateModel(flightEmptyNumber)
                .Any(x => x.MemberNames.Contains("FlightNumber") &&
                x.ErrorMessage.Contains("required")));
        }

        [Fact]
        public async Task FlightController_PostFlight_InvalidDepartureTime_ThrowsBadRequestException()
        {
            var flightInvalidTime = _fixture.Create<FlightPostDTO>();
            flightInvalidTime.DepartureTime = DateTime.MinValue;

            Assert.True(ValidateModel(flightInvalidTime)
                .Any(x => x.ErrorMessage.Contains("2000")));
        }

        [Fact]
        public async Task FlightController_PutFlight_ValidData_ReturnNoContentAsync()
        {
            _flightService.Setup(x => x.UpdateFlightAsync(It.IsAny<FlightPutDTO>()))
                .Returns(Task.CompletedTask);

            var result = await _flightController.EditFlightAsync(It.IsAny<FlightPutDTO>());

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task FlightController_PutFlight_InvalidFlightNumber_ThrowsBadRequestException()
        {
            var flightNullNumber = _fixture.Create<FlightPutDTO>();
            flightNullNumber.FlightNumber = null;
            var flightEmptyNumber = _fixture.Create<FlightPutDTO>();
            flightEmptyNumber.FlightNumber = "";

            Assert.True(ValidateModel(flightNullNumber)
                .Any(x => x.MemberNames.Contains("FlightNumber") &&
                x.ErrorMessage.Contains("required")));
            Assert.True(ValidateModel(flightEmptyNumber)
                .Any(x => x.MemberNames.Contains("FlightNumber") &&
                x.ErrorMessage.Contains("required")));
        }

        [Fact]
        public async Task FlightController_PutFlight_InvalidDepartureTime_ThrowsBadRequestException()
        {
            var flightInvalidTime = _fixture.Create<FlightPutDTO>();
            flightInvalidTime.DepartureTime = DateTime.MinValue;

            Assert.True(ValidateModel(flightInvalidTime)
                .Any(x => x.ErrorMessage.Contains("2000")));
        }

        [Fact]
        public async Task FlightController_DeleteFlight_ReturnNoContentAsync()
        {
            _flightService.Setup(x => x.DeactivateFlightAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            var result = await _flightController.DeactivateFlightAsync(It.IsAny<int>());

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
