using Microsoft.Extensions.Logging;

namespace AutoCrud.Test.Dummy
{
    internal class DummyRemoveOptionController : AutoCrudController<DummyEntity, int>
    {
        public DummyRemoveOptionController(IAutoCrudRepository<DummyEntity, int> repository, ILogger logger) : base(
            repository, logger)
        {
            RemoveAction(CrudActionType.GET_PAGE, new UnsupportedActionException("GET PAGE NOT SUPPORTED"));
            RemoveAction(CrudActionType.FIND_ENTITY, new UnsupportedActionException("FIND ENTITY NOT SUPPORTED"));
            RemoveAction(CrudActionType.CREATE, new UnsupportedActionException("CREATE NOT SUPPORTED"));
            RemoveAction(CrudActionType.UPDATE, new UnsupportedActionException("UPDATE NOT SUPPORTED"));
            RemoveAction(CrudActionType.DELETE, new UnsupportedActionException("DELETE NOT SUPPORTED"));
        }

        protected override int GetPageSize()
        {
            return 0;
        }

        protected override string GetCreatedEntityUri(DummyEntity createdEntity)
        {
            return "";
        }

        protected override void SetPrimaryKeyValueToEntity(DummyEntity entity, int primaryKey)
        {
        }
    }

    internal class DummyRemoveOptionControllerAsync : AutoCrudControllerAsync<DummyEntity, int>
    {
        public DummyRemoveOptionControllerAsync(IAutoCrudRepository<DummyEntity, int> repository, ILogger logger) :
            base(
                repository, logger)
        {
            RemoveAction(CrudActionType.GET_PAGE, new UnsupportedActionException("GET PAGE NOT SUPPORTED"));
            RemoveAction(CrudActionType.FIND_ENTITY, new UnsupportedActionException("FIND ENTITY NOT SUPPORTED"));
            RemoveAction(CrudActionType.CREATE, new UnsupportedActionException("CREATE NOT SUPPORTED"));
            RemoveAction(CrudActionType.UPDATE, new UnsupportedActionException("UPDATE NOT SUPPORTED"));
            RemoveAction(CrudActionType.DELETE, new UnsupportedActionException("DELETE NOT SUPPORTED"));
        }

        protected override int GetPageSize()
        {
            return 0;
        }

        protected override string GetCreatedEntityUri(DummyEntity createdEntity)
        {
            return "";
        }

        protected override void SetPrimaryKeyValueToEntity(DummyEntity entity, int primaryKey)
        {
        }
    }
}