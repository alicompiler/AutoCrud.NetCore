using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AutoCrud
{
    public abstract class AutoCrudController<Entity, PrimaryKey> : AutoCrudControllerBase<Entity, PrimaryKey>,
        IAutoCrudController<Entity, PrimaryKey> where Entity : class
    {
        protected AutoCrudController(IAutoCrudRepository<Entity, PrimaryKey> repository, ILogger logger) : base(
            repository, logger)
        {
        }


        [HttpGet]
        public virtual IActionResult GetPage([FromQuery] int page = 0)
        {
            BeforeGetPage();
            Log($"Fetching {GetEntityNameInPluralForLogging()} on page : {page}");
            var entities = _repository.GetPage(page, GetPageSize());
            return Ok(entities);
        }

        [HttpGet("{key}")]
        public IActionResult Find([FromRoute] PrimaryKey key)
        {
            BeforeFindEntity();
            Log($"Fetching {GetEntityNameInSingularForLogging()} with key : {key}");
            var entity = _repository.Find(key);
            return Ok(entity);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Entity entity)
        {
            BeforeCreate();
            Log($"Creating new {GetEntityNameInSingularForLogging()}");
            var createdEntity = _repository.Create(entity);
            PostProcessCreate(entity);
            return Created(GetCreatedEntityUri(createdEntity), createdEntity);
        }

        [HttpPut("{key}")]
        public IActionResult Update([FromRoute] PrimaryKey key, [FromBody] Entity entity)
        {
            BeforeUpdate();
            Log($"Updating {GetEntityNameInSingularForLogging()} with key : {key}");
            SetPrimaryKeyValueToEntity(entity, key);
            var editedEntity = _repository.Update(entity);
            PostProcessUpdate(entity);
            return Ok(editedEntity);
        }

        [HttpDelete("{key}")]
        public IActionResult Delete([FromRoute] PrimaryKey key)
        {
            BeforeDelete();
            Log($"Deleting {GetEntityNameInSingularForLogging()} with key : {key}");
            var deletedEntity = _repository.Delete(key);
            PostProcessDelete(deletedEntity);
            return NoContent();
        }

        protected void PostProcessCreate(Entity entity)
        {
        }

        protected void PostProcessUpdate(Entity entity)
        {
        }

        protected void PostProcessDelete(Entity entity)
        {
        }
    }
}