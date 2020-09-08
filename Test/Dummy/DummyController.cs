using Microsoft.Extensions.Logging;

namespace AutoCrud.Test.Dummy
{
    public class DummyController : AutoCrudController<DummyEntity, int>
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


    public class DummyControllerAsync : AutoCrudControllerAsync<DummyEntity, int>
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