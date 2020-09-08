# AutoCrudRepository

there are five actions that we support : 

 - Create
 - Update
 - Delete
 - Find
 - GetPage
 
for every action we provide two methods, one is sync and the other is async.

take a look on the `IAutoCrudRepository` interface

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
    
    
 
 
 ## Usage
 
 to create new **auto-crud** repository you need to extends the abstract class `AutoCrudRepository`.
 
 first you need to tell the class what the type of the entity and the type of the primary key.
 
 
    //example
    public class BookRepository extends AutoCrudRepository<Book , int> {...}
    
    
 then you need to implement the abstract methods of `AutoCrudRepository` class, which are : 
 
    DbSet<Entity> GetDbSet();
    
    PrimaryKey GetPrimaryKey(Entity entity);
    
    
 the implementation should very simple, the method `GetDbSet` should return the `DbSet` of the `DbContext` object
 
    DbSet<Book> GetDbSet() {
        return _myDbContext.Books;
    }

 and the method `GetPrimaryKey` should return the primary key value of the passed entity
 
    int GetPrimaryKey(Book book) {
        return book.Id;
    }


take a look on the full example of book repository

    public class BookRepository : AutoCrudRepository<Book, int>
    {
        private readonly LibraryDbContext _dbContext;

        public BookRepository(LibraryDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        protected override DbSet<Book> GetDbSet()
        {
            return _dbContext.Books;
        }

        protected override int GetPrimaryKey(Book book)
        {
            return book.Id;
        }
    }

very easy and simple


## Customization 

you can customize the behavior of the repository by overriding some of `virtual` methods that we provide

#### Creation

 for creation you can alter the entity before persisting it, by overriding the
 method `PreProcessCreate`
 
    void PreProcessCreate(Entity entity)
 
 let's say you want store `CreatedTime` value in the entity, you simply can override this method 
 to alter the entity the way you want.
 
#### Updating
 for updating you have two options, one is to alter the entity before persisting, just like in creation.
 also you can set the fields that you want to be updated by overriding `SetUpdatedValues` method
  
    void SetUpdatedValues(EntityEntry<Entity> entry, Entity toUpdate)
    
  note that `toUpdate` object will have the new values and `entry` is the object that need to be updated.
  
    //example
    void SetUpdatedValues(EntityEntry<Book> entry, Book toUpdate) {
        entry.Entity.Title = toUpdate.Title;
        entry.Entity.Author = toUpdate.Author;
    }
  in the example above, only title and author fields will be updated.
  
  
#### PageQuery
 you can also alter the behavior of page querying, you may want to alter the sorting behavior or you may want 
 to filter some rows. to do that you can override the method `PreProcessPageQuery`
 
    IQueryable<Entity> PreProcessPageQuery(IQueryable<Entity> query)
    
 let's take example, we need to skip the entities that has value `false` on `Deleted` property
 and also we want to sort the result descending by the id.
 
    IQueryable<Book> PreProcessPageQuery(IQueryable<Book> query) {
        return query.Where(b => b.Deleted == false).OrderByDescending(b => b.Id);
    }

