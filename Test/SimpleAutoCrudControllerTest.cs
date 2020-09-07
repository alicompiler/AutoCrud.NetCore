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
    public class SimpleAutoCrudControllerTest
    {
        public SimpleAutoCrudControllerTest()
        {
            _logger = new Mock<ILogger>();
            _dbContext =
                TestUtils.MakeInMemoryDbContext<DummyDbContext>(nameof(SimpleAutoCrudControllerTest));
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

        private DummyController MakeControllerObject()
        {
            return new DummyController(_repository, _logger.Object);
        }

        [Fact]
        public async Task ShouldCreateModel()
        {
            var model = new DummyEntity {Name = "Created Model"};
            var controller = MakeControllerObject();
            var result = await controller.Create(model);
            Assert.IsAssignableFrom<CreatedResult>(result);
            var returnedModel = (DummyEntity) ((CreatedResult) result).Value;
            Assert.Equal(model.Name, returnedModel.Name);
            Assert.Equal("created url", ((CreatedResult) result).Location);
            TestUtils.VerifyLogMessage(_logger, "Creating new model");
        }


        [Fact]
        public async Task ShouldDeleteModel()
        {
            var controller = MakeControllerObject();
            var result = await controller.Delete(1);
            Assert.IsAssignableFrom<NoContentResult>(result);
            TestUtils.VerifyLogMessage(_logger, "Deleting model with key : 1");
        }

        [Fact]
        public async Task ShouldFindModel()
        {
            var controller = MakeControllerObject();
            var result = await controller.FindModel(1);
            Assert.IsAssignableFrom<OkObjectResult>(result);
            var returnedModel = (DummyEntity) ((OkObjectResult) result).Value;
            Assert.Equal("First", returnedModel.Name);
            TestUtils.VerifyLogMessage(_logger, "Fetching model with key : 1");
        }

        [Fact]
        public async Task ShouldGetModels()
        {
            var controller = MakeControllerObject();
            var result = await controller.GetModels();
            Assert.IsAssignableFrom<OkObjectResult>(result);
            var returnedModels = (List<DummyEntity>) ((OkObjectResult) result).Value;
            Assert.Equal(2, returnedModels.Count);
            TestUtils.VerifyLogMessage(_logger, "Fetching models on page : 0");
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
                        string.Equals("Deleting model with key : 1", o.ToString(),
                            StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
                Times.Never
            );
        }

        [Fact]
        public async Task ShouldUpdateModel()
        {
            //setting Id = 2, but in url we use 1, this could happen by user to edit the value of id, but it should reset the value to 1
            var model = new DummyEntity {Name = "EDITED MODEL", Id = 2};
            var controller = MakeControllerObject();
            var result = await controller.Update(1, model);
            Assert.IsAssignableFrom<OkObjectResult>(result);
            var returnedModel = (DummyEntity) ((OkObjectResult) result).Value;
            Assert.Equal("EDITED MODEL", returnedModel.Name);
            Assert.Equal(1, returnedModel.Id);
            TestUtils.VerifyLogMessage(_logger, "Updating model with key : 1");
        }
    }
}