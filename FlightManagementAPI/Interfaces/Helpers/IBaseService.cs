namespace FlightManagementAPI.Interfaces.Helpers
{
    public interface IBaseService<TEntity>

    {
        Task<TEntity?> GetEntityByIdAsync(int id);
        Task<IEnumerable<TEntity>?> GetEntitiesAsync();
        Task AddNewEntityAsync(TEntity entity);
        Task UpdateEntityAsync(int id, TEntity entity);
        Task DeactivateEntityAsync(int id);
    }
}
