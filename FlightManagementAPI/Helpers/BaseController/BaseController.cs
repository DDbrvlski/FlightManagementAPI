using FlightManagementAPI.Interfaces.Helpers;
using FlightManagementData.Models.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagementAPI.Helpers.BaseController
{
    public class BaseController<TEntity>(IBaseService<TEntity> baseService) : ControllerBase where TEntity : BaseEntity
    {
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> DeleteEntity(int id)
        {
            await baseService.DeactivateEntityAsync(id);
            return NoContent();
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<ActionResult<IEnumerable<TEntity>>> GetEntities()
        {
            var entities = await baseService.GetEntitiesAsync();
            return Ok(entities);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public virtual async Task<ActionResult<TEntity>> GetEntity(int id)
        {
            var entity = await baseService.GetEntityByIdAsync(id);
            return Ok(entity);
        }

        [HttpPost]
        public virtual async Task<IActionResult> PostEntity(TEntity entity)
        {
            await baseService.AddNewEntityAsync(entity);
            return Created();
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> PutEntity(int id, [FromBody] TEntity entity)
        {
            await baseService.UpdateEntityAsync(id, entity);
            return NoContent();
        }
    }
}
