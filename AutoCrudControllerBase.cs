using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AutoCrud
{
    public abstract class AutoCrudControllerBase<Entity, PrimaryKey> : ControllerBase where Entity : class
    {
        protected readonly ILogger _logger;
        protected readonly IAutoCrudRepository<Entity, PrimaryKey> _repository;

        protected AutoCrudControllerBase(IAutoCrudRepository<Entity, PrimaryKey> repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public bool EnableLogging { get; set; } = true;

        protected virtual string GetEntityNameInSingularForLogging()
        {
            return "entity";
        }

        protected virtual string GetEntityNameInPluralForLogging()
        {
            return "entities";
        }

        protected void Log(string message)
        {
            if (EnableLogging) _logger.LogInformation(message);
        }

        protected abstract int GetPageSize();

        protected abstract string GetCreatedEntityUri(Entity createdEntity);

        protected abstract void SetPrimaryKeyValueToEntity(Entity entity, PrimaryKey primaryKey);
    }
}