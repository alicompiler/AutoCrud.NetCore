using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AutoCrud
{
    public abstract class AutoCrudRepository<Entity, PrimaryKey> : IAutoCrudRepository<Entity, PrimaryKey>
        where Entity : class
    {
        private readonly DbContext _dbContext;

        protected AutoCrudRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ReSharper disable once UnusedParameter.Global
        protected virtual void PreProcessCreate(Entity entity)
        {
        }

        public async Task<Entity> CreateAsync(Entity entity)
        {
            PreProcessCreate(entity);
            var entry = await GetDbSet().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entry.Entity;
        }

        public Entity Create(Entity entity)
        {
            PreProcessCreate(entity);
            var entry = GetDbSet().Add(entity);
            _dbContext.SaveChanges();
            return entry.Entity;
        }


        // ReSharper disable once UnusedParameter.Global
        protected virtual void PreProcessUpdate(Entity entity)
        {
        }

        public async Task<Entity> UpdateAsync(Entity entity)
        {
            var existingEntity = await FindAsync(GetPrimaryKey(entity));
            var entry = GetDbSet().Update(existingEntity);
            PreProcessUpdate(entity);
            SetUpdatedValues(entry, entity);
            await _dbContext.SaveChangesAsync();
            return entry.Entity;
        }

        public Entity Update(Entity entity)
        {
            var existingEntity = Find(GetPrimaryKey(entity));
            var entry = GetDbSet().Update(existingEntity);
            PreProcessUpdate(entity);
            SetUpdatedValues(entry, entity);
            _dbContext.SaveChanges();
            return entry.Entity;
        }

        protected virtual void SetUpdatedValues(EntityEntry<Entity> entry, Entity toUpdate)
        {
            entry.CurrentValues.SetValues(toUpdate);
        }


        public async Task<Entity> DeleteAsync(PrimaryKey key)
        {
            var video = await FindAsync(key);
            var entry = GetDbSet().Remove(video);
            await _dbContext.SaveChangesAsync();
            return entry.Entity;
        }

        public Entity Delete(PrimaryKey key)
        {
            var video = Find(key);
            var entry = GetDbSet().Remove(video);
            _dbContext.SaveChangesAsync();
            return entry.Entity;
        }


        public async Task<Entity> FindAsync(PrimaryKey key)
        {
            return await GetDbSet().FindAsync(key);
        }

        public Entity Find(PrimaryKey key)
        {
            return GetDbSet().Find(key);
        }


        public async Task<List<Entity>> GetPageAsync(int page, int pageSize)
        {
            var query = GetDbSet().Take(pageSize).Skip(page * pageSize);
            query = PreProcessPageQuery(query);
            return await query.ToListAsync();
        }

        public List<Entity> GetPage(int page, int pageSize)
        {
            var query = GetDbSet().Skip(page * pageSize).Take(pageSize);
            query = PreProcessPageQuery(query);
            return query.ToList();
        }

        protected virtual IQueryable<Entity> PreProcessPageQuery(IQueryable<Entity> query)
        {
            return query;
        }

        protected abstract DbSet<Entity> GetDbSet();
        protected abstract PrimaryKey GetPrimaryKey(Entity entity);
    }
}