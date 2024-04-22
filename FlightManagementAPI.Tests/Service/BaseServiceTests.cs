using AutoFixture;
using FlightManagementAPI.Helpers.BaseService;
using FlightManagementAPI.Infrastructure.Exceptions;
using FlightManagementData.Data;
using FlightManagementData.Models.Address;
using FlightManagementData.Models.Flights;
using FlightManagementData.Models.Helpers;
using Microsoft.EntityFrameworkCore;

namespace FlightManagementAPI.Tests.Service
{
    public class BaseServiceTests
    {
        private readonly DbContextOptions<FlightManagementContext> _options;
        private readonly FlightManagementContext _context;
        private readonly Fixture _fixture;

        public BaseServiceTests()
        {
            _fixture = new Fixture();
            _options = new DbContextOptionsBuilder<FlightManagementContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new FlightManagementContext(_options);
        }

        [Fact]
        public async Task GetEntities_ReturnEntities()
        {
            await GetEntities<City>();
            await GetEntities<Country>();
            await GetEntities<AirplaneType>();
        }

        private async Task GetEntities<TEntity>() where TEntity : BaseEntity
        {
            var baseService = CreateBaseService<TEntity>();
            var entityList = _fixture.CreateMany<TEntity>(5).ToList();
            var enti = entityList.First();
            //Ustawienie flagi isactive pierwszego elementu, aby zawsze coś się wyświetliło
            enti.IsActive = true;            

            await _context.Set<TEntity>().AddRangeAsync(entityList);
            await _context.SaveChangesAsync();

            var result = await baseService.GetEntitiesAsync();

            Assert.NotNull(result);
            Assert.Equal(entityList.Count(x => x.IsActive), result.Count());
            foreach (var entity in entityList.Where(x => x.IsActive))
            {
                Assert.Contains(result, x => x.Id == entity.Id);
            }
        }

        [Fact]
        public async Task GetEntityByIdAsync_EntityExists_ReturnsEntity()
        {
            await GetExistingEntityByIdAsync<City>();
            await GetExistingEntityByIdAsync<Country>();
            await GetExistingEntityByIdAsync<AirplaneType>();
        }

        private async Task GetExistingEntityByIdAsync<TEntity>() where TEntity : BaseEntity
        {
            var baseService = CreateBaseService<TEntity>();
            var entity = _fixture.Create<TEntity>();
            entity.IsActive = true;
            var entityId = entity.Id;

            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();

            var result = await baseService.GetEntityByIdAsync(entityId);

            Assert.NotNull(result);
            Assert.Equal(entityId, result.Id);
        }

        [Fact]
        public async Task GetEntityByIdAsync_EntityNotExists_ThrowsNotFoundException()
        {
            await GetNotExistingEntityByIdAsync<City>();
            await GetNotExistingEntityByIdAsync<Country>();
            await GetNotExistingEntityByIdAsync<AirplaneType>();
        }

        private async Task GetNotExistingEntityByIdAsync<TEntity>() where TEntity : BaseEntity
        {
            var baseService = CreateBaseService<TEntity>();
            var entityId = 1;

            await Assert
                .ThrowsAsync<NotFoundException>(async () => await baseService.GetEntityByIdAsync(entityId));
        }

        [Fact]
        public async Task PostEntity_EntityAddedSuccessfully()
        {
            await PostEntity<City>();
            await PostEntity<Country>();
            await PostEntity<AirplaneType>();
        }

        private async Task PostEntity<TEntity>() where TEntity : BaseEntity
        {
            var baseService = CreateBaseService<TEntity>();
            var entity = _fixture.Create<TEntity>();

            await baseService.AddNewEntityAsync(entity);

            var addedEntity = await _context.Set<TEntity>().FindAsync(entity.Id);
            Assert.NotNull(addedEntity);
        }

        [Fact]
        public async Task PutEntity_EntityUpdatedSuccessfully()
        {
            await PutEntity<City>();
            await PutEntity<Country>();
            await PutEntity<AirplaneType>();
        }

        private async Task PutEntity<TEntity>() where TEntity : BaseEntity
        {
            var baseService = new BaseService<TEntity>(_context);
            var entity = _fixture.Create<TEntity>();
            entity.IsActive = true;
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();

            var updatedEntity = _fixture.Create<TEntity>();
            updatedEntity.Id = entity.Id;

            await baseService.UpdateEntityAsync(entity.Id, updatedEntity);

            var updatedEntityFromDb = await _context.Set<TEntity>().FindAsync(entity.Id);
            Assert.NotNull(updatedEntityFromDb);
        }

        [Fact]
        public async Task DeleteEntity_EntityDeletedSuccessfully()
        {
            await DeleteEntity<City>();
            await DeleteEntity<Country>();
            await DeleteEntity<AirplaneType>();
        }

        private async Task DeleteEntity<TEntity>() where TEntity : BaseEntity
        {
            var baseService = new BaseService<TEntity>(_context);
            var entity = _fixture.Create<TEntity>();
            entity.IsActive = true;

            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();

            await baseService.DeactivateEntityAsync(entity.Id);

            var deactivatedEntity = await _context.Set<TEntity>().FindAsync(entity.Id);
            Assert.NotNull(deactivatedEntity);
            Assert.False(entity.IsActive);
        }

        private BaseService<TEntity> CreateBaseService<TEntity>() where TEntity : BaseEntity
        {
            return new BaseService<TEntity>(_context);
        }
    }
}
