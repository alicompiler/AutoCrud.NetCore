using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCrud.Test.Dummy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AutoCrud.Test
{
    public class AutoCrudControllerAsyncTest
    {
        public AutoCrudControllerAsyncTest()
        {
            _logger = new Mock<ILogger>();
            _dbContext = TestUtils.MakeInMemoryDbContext<DummyDbContext>(nameof(AutoCrudControllerTest));
            _repository = new DummyRepository(_dbContext);
            SetupData();
        }

        private readonly Mock<ILogger> _logger;
        private readonly DummyRepository _repository;
        private readonly DummyDbContext _dbContext;

        private void SetupData()
        {
            _dbContext.Dummies.RemoveRange(_dbContext.Dummies);
            _dbContext.Dummies.Add(new DummyEntity {Id = 1, Name = "First"});
            _dbContext.Dummies.Add(new DummyEntity {Id = 2, Name = "Second"});
            _dbContext.Dummies.Add(new DummyEntity {Id = 3, Name = "Third"});
            _dbContext.Dummies.Add(new DummyEntity {Id = 4, Name = "Fourth"});
            _dbContext.Dummies.Add(new DummyEntity {Id = 5, Name = "Fifth"});
            _dbContext.Dummies.Add(new DummyEntity {Id = 6, Name = "Sixth"});
            _dbContext.SaveChanges();
        }

        private DummyControllerAsync MakeControllerObject()
        {
            return new DummyControllerAsync(_repository, _logger.Object);
        }

        [Fact]
        public async Task ShouldCreateEntity()
        {
            var entity = new DummyEntity {Name = "Created Entity"};
            var controller = MakeControllerObject();
            var result = await controller.Create(entity);
            Assert.IsAssignableFrom<CreatedResult>(result);
            var returnedEntity = (DummyEntity) ((CreatedResult) result).Value;
            Assert.Equal(entity.Name, returnedEntity.Name);
            Assert.Equal("created url", ((CreatedResult) result).Location);
            TestUtils.VerifyLogMessage(_logger, "Creating new entity");
        }


        [Fact]
        public async Task ShouldDeleteEntity()
        {
            var controller = MakeControllerObject();
            var result = await controller.Delete(1);
            Assert.IsAssignableFrom<NoContentResult>(result);
            TestUtils.VerifyLogMessage(_logger, "Deleting entity with key : 1");
        }

        [Fact]
        public async Task ShouldFindEntity()
        {
            var controller = MakeControllerObject();
            var result = await controller.Find(1);
            Assert.IsAssignableFrom<OkObjectResult>(result);
            var returnedEntity = (DummyEntity) ((OkObjectResult) result).Value;
            Assert.Equal("First", returnedEntity.Name);
            TestUtils.VerifyLogMessage(_logger, "Fetching entity with key : 1");
        }

        [Fact]
        public async Task ShouldGetEntities()
        {
            var controller = MakeControllerObject();
            var result = await controller.GetPage();
            Assert.IsAssignableFrom<OkObjectResult>(result);
            var returnedEntities = (List<DummyEntity>) ((OkObjectResult) result).Value;
            Assert.Equal(2, returnedEntities.Count);
            TestUtils.VerifyLogMessage(_logger, "Fetching entities on page : 0");
        }


        [Fact]
        public async Task ShouldNotLogWhenLoggingNotEnabled()
        {
            var controller = MakeControllerObject();
            controller.EnableLogging = false;
            var result = await controller.Delete(1);
            Assert.IsAssignableFrom<NoContentResult>(result);
            _logger.Verify(l => l.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) =>
                        string.Equals("Deleting entity with key : 1", o.ToString(),
                            StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
                Times.Never
            );
        }

        [Fact]
        public async Task ShouldUpdateEntity()
        {
            //setting Id = 2, but in url we use 1, this could happen by user to edit the value of id, but it should reset the value to 1
            var entity = new DummyEntity {Name = "EDITED ENTITY", Id = 2};
            var controller = MakeControllerObject();
            var result = await controller.Update(1, entity);
            Assert.IsAssignableFrom<OkObjectResult>(result);
            var returnedEntity = (DummyEntity) ((OkObjectResult) result).Value;
            Assert.Equal("EDITED ENTITY_UPDATED", returnedEntity.Name);
            Assert.Equal(1, returnedEntity.Id);
            TestUtils.VerifyLogMessage(_logger, "Updating entity with key : 1");
        }
    }
}