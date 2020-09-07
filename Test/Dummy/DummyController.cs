using Microsoft.Extensions.Logging;

namespace AutoCrud.Test.Dummy
{
    public class DummyController : SimpleAutoCrudController<DummyEntity, int>
    {
        public DummyController(AutoCrudRepository<DummyEntity, int> repository, ILogger logger) :
            base(repository, logger)
        {
        }

        protected override int GetPageSize()
        {
            return 2;
        }

        protected override string GetCreatedModelUrl(DummyEntity createdModel)
        {
            return "created url";
        }

        protected override void SetPrimaryKeyValueToModel(DummyEntity model, int primaryKey)
        {
            model.Id = primaryKey;
        }
    }
}