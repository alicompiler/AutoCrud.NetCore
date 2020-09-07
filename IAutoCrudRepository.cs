using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCrud
{
    public interface IAutoCrudRepository<Entity, in PrimaryKey> where Entity : class
    {
        Task<Entity> CreateAsync(Entity entity);
        Entity Create(Entity entity);

        Task<Entity> UpdateAsync(Entity entity);
        Entity Update(Entity entity);

        Task<Entity> DeleteAsync(PrimaryKey key);
        Entity Delete(PrimaryKey key);

        Task<Entity> FindAsync(PrimaryKey key);
        Entity Find(PrimaryKey key);

        Task<List<Entity>> GetPageAsync(int page, int pageSize);
        List<Entity> GetPage(int page, int pageSize);
    }
}