using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AutoCrud.Test.Dummy
{
    internal class DummyRepository : AutoCrudRepository<DummyEntity, int>
    {
        private readonly DummyDbContext _dbContext;

        public DummyRepository(DummyDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        protected override DbSet<DummyEntity> GetDbSet()
        {
            return _dbContext.Dummies;
        }

        protected override int GetPrimaryKey(DummyEntity entity)
        {
            return entity.Id;
        }

        protected override void PreProcessCreate(DummyEntity entity)
        {
            entity.Name = entity.Name + "_CREATED";
        }

        protected override void PreProcessUpdate(DummyEntity entity)
        {
            entity.Name = entity.Name + "_UPDATED";
        }

        protected override IQueryable<DummyEntity> PreProcessPageQuery(IQueryable<DummyEntity> query)
        {
            return query.OrderByDescending(d => d.Id);
        }
    }
}