using AutoFixture;
using FlightManagementAPI.Helpers.BaseController;
using FlightManagementAPI.Helpers.BaseService;
using FlightManagementAPI.Interfaces.Helpers;
using FlightManagementData.Models.Address;
using FlightManagementData.Models.Flights;
using FlightManagementData.Models.Helpers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FlightManagementAPI.Tests.Controller
{
    public class BaseControllerTests
    {
        private readonly Fixture _fixture;

        public BaseControllerTests()
        {
            _fixture = new Fixture();
        }


        [Fact]
        public async Task BaseController_GetEntities_ReturnOKAsync()
        {
            await GetEntitiesAsync<City>();
            await GetEntitiesAsync<Country>();
            await GetEntitiesAsync<AirplaneType>();
        }

        private async Task GetEntitiesAsync<TEntity>() where TEntity : BaseEntity
        {
            var entityList = _fixture.CreateMany<TEntity>(5).ToList();

            var serviceMock = CreateServiceMock<TEntity>();
            var controller = new BaseController<TEntity>(serviceMock.Object);

            serviceMock.Setup(x => x.GetEntitiesAsync()).ReturnsAsync(entityList);

            var result = await controller.GetEntities();

            var objResult = result.Result as ObjectResult;
            Assert.Equal(200, objResult.StatusCode);
        }

        [Fact]
        public async Task BaseController_GetSingleEntity_ReturnOKAsync()
        {
            await GetSingleEntityAsync<City>();
            await GetSingleEntityAsync<Country>();
            await GetSingleEntityAsync<AirplaneType>();
        }

        private async Task GetSingleEntityAsync<TEntity>() where TEntity : BaseEntity
        {
            var city = _fixture.Create<TEntity>();

            var serviceMock = CreateServiceMock<TEntity>();
            var controller = new BaseController<TEntity>(serviceMock.Object);

            serviceMock.Setup(x => x.GetEntityByIdAsync(It.IsAny<int>())).ReturnsAsync(city);
            controller = new BaseController<TEntity>(serviceMock.Object);

            var result = await controller.GetEntity(It.IsAny<int>());
            var objResult = result.Result as ObjectResult;
            Assert.Equal(200, objResult.StatusCode);
        }

        [Fact]
        public async Task BaseController_PostEntity_ReturnCreatedAsync()
        {
            await PostEntityAsync<City>();
            await PostEntityAsync<Country>();
            await PostEntityAsync<AirplaneType>();
        }

        private async Task PostEntityAsync<TEntity>() where TEntity : BaseEntity
        {
            var serviceMock = CreateServiceMock<TEntity>();
            var controller = new BaseController<TEntity>(serviceMock.Object);

            serviceMock.Setup(x => x.AddNewEntityAsync(It.IsAny<TEntity>()))
                .Returns(Task.CompletedTask);

            controller = new BaseController<TEntity>(serviceMock.Object);

            var result = await controller.PostEntity(It.IsAny<TEntity>());

            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task BaseController_PutEntity_ReturnNoContentAsync()
        {
            await PutEntityAsync<City>();
            await PutEntityAsync<Country>();
            await PutEntityAsync<AirplaneType>();
        }

        private async Task PutEntityAsync<TEntity>() where TEntity : BaseEntity
        {
            var serviceMock = CreateServiceMock<TEntity>();
            var controller = new BaseController<TEntity>(serviceMock.Object);

            serviceMock.Setup(x => x.UpdateEntityAsync(It.IsAny<int>(), It.IsAny<TEntity>()))
                .Returns(Task.CompletedTask);

            controller = new BaseController<TEntity>(serviceMock.Object);

            var result = await controller.PutEntity(It.IsAny<int>(), It.IsAny<TEntity>());

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task BaseController_DeleteEntity_ReturnNoContentAsync()
        {
            await DeleteEntityAsync<City>();
            await DeleteEntityAsync<Country>();
            await DeleteEntityAsync<AirplaneType>();
        }

        private async Task DeleteEntityAsync<TEntity>() where TEntity : BaseEntity
        {
            var serviceMock = CreateServiceMock<TEntity>();
            var controller = new BaseController<TEntity>(serviceMock.Object);

            serviceMock.Setup(x => x.DeactivateEntityAsync(It.IsAny<int>()))
               .Returns(Task.CompletedTask);

            controller = new BaseController<TEntity>(serviceMock.Object);

            var result = await controller.DeleteEntity(It.IsAny<int>());

            Assert.IsType<NoContentResult>(result);
        }

        private Mock<IBaseService<TEntity>> CreateServiceMock<TEntity>() where TEntity : BaseEntity
        {
            return new Mock<IBaseService<TEntity>>();
        }
    }
}
