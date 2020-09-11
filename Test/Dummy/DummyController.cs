using Microsoft.Extensions.Logging;

namespace AutoCrud.Test.Dummy
{
    internal class DummyController : AutoCrudController<DummyEntity, int>
    {
        public DummyController(AutoCrudRepository<DummyEntity, int> repository, ILogger logger) :
            base(repository, logger)
        {
        }

        protected override int GetPageSize()
        {
            return 2;
        }

        protected override string GetCreatedEntityUri(DummyEntity createdEntity)
        {
            return "created url";
        }

        protected override void SetPrimaryKeyValueToEntity(DummyEntity entity, int primaryKey)
        {
            entity.Id = primaryKey;
        }
    }


    internal class DummyControllerAsync : AutoCrudControllerAsync<DummyEntity, int>
    {
        public DummyControllerAsync(AutoCrudRepository<DummyEntity, int> repository, ILogger logger) :
            base(repository, logger)
        {
        }

        protected override int GetPageSize()
        {
            return 2;
        }

        protected override string GetCreatedEntityUri(DummyEntity createdEntity)
        {
            return "created url";
        }

        protected override void SetPrimaryKeyValueToEntity(DummyEntity entity, int primaryKey)
        {
            entity.Id = primaryKey;
        }
    }
}