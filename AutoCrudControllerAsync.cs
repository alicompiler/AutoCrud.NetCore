using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AutoCrud
{
    public abstract class AutoCrudControllerAsync<Entity, PrimaryKey> : AutoCrudControllerBase<Entity, PrimaryKey>,
        IAutoCrudControllerAsync<Entity, PrimaryKey> where Entity : class
    {
        protected AutoCrudControllerAsync(IAutoCrudRepository<Entity, PrimaryKey> repository, ILogger logger) : base(
            repository, logger)
        {
        }


        [HttpGet]
        public virtual async Task<IActionResult> GetPage([FromQuery] int page = 0)
        {
            Log($"Fetching {GetEntityNameInPluralForLogging()} on page : {page}");
            var entities = await _repository.GetPageAsync(page, GetPageSize());
            return Ok(entities);
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> Find([FromRoute] PrimaryKey key)
        {
            Log($"Fetching {GetEntityNameInSingularForLogging()} with key : {key}");
            var entity = await _repository.FindAsync(key);
            return Ok(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Entity entity)
        {
            Log($"Creating new {GetEntityNameInSingularForLogging()}");
            var createdEntity = await _repository.CreateAsync(entity);
            await PostProcessCreate(entity);
            return Created(GetCreatedEntityUri(createdEntity), createdEntity);
        }

        [HttpPut("{key}")]
        public async Task<IActionResult> Update([FromRoute] PrimaryKey key, [FromBody] Entity entity)
        {
            Log($"Updating {GetEntityNameInSingularForLogging()} with key : {key}");
            SetPrimaryKeyValueToEntity(entity, key);
            var editedEntity = await _repository.UpdateAsync(entity);
            await PostProcessUpdate(entity);
            return Ok(editedEntity);
        }

        [HttpDelete("{key}")]
        public async Task<IActionResult> Delete([FromRoute] PrimaryKey key)
        {
            Log($"Deleting {GetEntityNameInSingularForLogging()} with key : {key}");
            var deletedEntity = await _repository.DeleteAsync(key);
            await PostProcessDelete(deletedEntity);
            return NoContent();
        }

        protected virtual Task PostProcessCreate(Entity entity)
        {
            return Task.CompletedTask;
        }

        protected virtual Task PostProcessUpdate(Entity entity)
        {
            return Task.CompletedTask;
        }

        protected virtual Task PostProcessDelete(Entity entity)
        {
            return Task.CompletedTask;
        }
    }
}