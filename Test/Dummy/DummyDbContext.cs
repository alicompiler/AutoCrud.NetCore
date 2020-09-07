using Microsoft.EntityFrameworkCore;

namespace AutoCrud.Test.Dummy
{
    public class DummyDbContext : DbContext
    {
        public DummyDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<DummyEntity> Dummies { get; set; }
    }
}