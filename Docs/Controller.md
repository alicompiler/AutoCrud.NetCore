# AutoCrudController

we provide five endpoints when using `AutoCrudController` : 

 - Find Entity
    
        GET /:key
        return 200 on successful request
   
 - Page Of Entities
    
        GET /
        return 200 on successful request
        
 - Create 
 
        POST /
        return 201 on successful request with the created entity
        
 - Update
 
        PUT /:key
        return 200 on successful request with the updated entity
        
        
 - Delete
 
        DELETE /:key
        return 204 on successful request
        
        
        
 
## Usage

to create new **auto-crud** controller you need to extend the abstract class `AutoCrudController` or `AutoCrudControllerAsync`.

first you need to tell the controller what is the type of the entity and the type of the primary key. 
 
    //example
    [ApiController]
    [Route("/books")]
    public class BookController extends AutoCrudController<Book , int> {...}
    
then you need to implement the abstract methods : 

        int GetPageSize();

        string GetCreatedEntityUri(Entity createdEntity);

        void SetPrimaryKeyValueToEntity(Entity entity, PrimaryKey primaryKey);

the implementation should be simple, 

 - `GetPageSize` : return the number of entities for each page
 - `GetCreatedEntityUri` : return the uri for the created entity.
 - `SetPrimaryKeyValueToEntity` : just for security purposes when updating, the incoming data may have 
 id set to 2, while the requested url `PUT /entity/1`. we just use this method to set the key back, so that it can't be updated.
 
        //example
        void SetPrimaryKeyValueToEntity(Book book, int id) {
            book.Id = id;
        }
        



# Customization

you can add behavior after: Create,Update and Delete by overriding `PostProcessCreate` , `PostProcessUpdate` and `PostProcessDelete` methods respectively.

suppose you want to send email when book is created, then you can override `PostProcessCreate` method.

in **sync** controller these method return `void` and in the **async** controller they return Task, so you want `await` them.



## Logging

you provide logging functionality for every action. you can cancel the logging behavior by setting `EnableLogging` to `false` in the constructor.

we use two virtual methods(you can override) to get the name of the entity when logging :

    string GetEntityNameInSingularForLogging() // return "entity"
    
    string GetEntityNameInPluralForLogging()  // return "entities"
        
when you create an entity, a message is logged : `"Creating new entity"` , but when you override these methods



        override string GetEntityNameInSingularForLogging()
        {
            return "book";
        }

        override string GetEntityNameInPluralForLogging()
        {
            return "books";
        }
        
the logged message when you create an entity will be : "Creating new book".

all log will be at `Information` level