using System.Threading.Tasks;
using AutoCrud.Test.Dummy;
using Xunit;

namespace AutoCrud.Test
{
    public class SimpleAutoCrudRepositoryTest
    {
        public SimpleAutoCrudRepositoryTest()
        {
            _dbContext = TestUtils.MakeInMemoryDbContext<DummyDbContext>(nameof(SimpleAutoCrudRepositoryTest));
            _repository = new DummyRepository(_dbContext);
            SetupInitialData();
        }

        private readonly DummyDbContext _dbContext;
        private readonly DummyRepository _repository;

        private void SetupInitialData()
        {
            _dbContext.Dummies.RemoveRange(_dbContext.Dummies);
            _dbContext.Dummies.Add(new DummyEntity {Id = 1, Name = "First"});
            _dbContext.Dummies.Add(new DummyEntity {Id = 2, Name = "Second"});
            _dbContext.Dummies.Add(new DummyEntity {Id = 3, Name = "Third"});
            _dbContext.Dummies.Add(new DummyEntity {Id = 4, Name = "Fourth"});
            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task ShouldCreateModelAsync()
        {
            var model = new DummyEntity {Name = "New Model"};
            var newModel = await _repository.CreateAsync(model);
            Assert.Equal(model.Name, newModel.Name);
            Assert.True(model.Id > 4);
        }

        [Fact]
        public void ShouldCreateModel()
        {
            var model = new DummyEntity {Name = "New Model"};
            var newModel = _repository.Create(model);
            Assert.Equal(model.Name, newModel.Name);
            Assert.True(model.Id > 4);
        }

        [Fact]
        public void ShouldUsePreProcessCreate()
        {
            var model = new DummyEntity {Name = "New Model"};
            var newModel = _repository.Create(model);
            Assert.Equal(model.Name, newModel.Name);
            Assert.Equal("New Model_CREATED", model.Name);
        }


        [Fact]
        public async Task ShouldUpdateModelAsync()
        {
            var toEditModel = new DummyEntity {Name = "Edited Model", Id = 1};
            var editedModel = await _repository.UpdateAsync(toEditModel);
            Assert.Equal(toEditModel.Name, editedModel.Name);
            Assert.Equal(toEditModel.Id, editedModel.Id);
        }

        [Fact]
        public void ShouldUpdateModel()
        {
            var toEditModel = new DummyEntity {Name = "Edited Model", Id = 1};
            var editedModel = _repository.Update(toEditModel);
            Assert.Equal(toEditModel.Name, editedModel.Name);
            Assert.Equal(toEditModel.Id, editedModel.Id);
        }

        [Fact]
        public void ShouldUsePreProcessUpdate()
        {
            var toEditModel = new DummyEntity {Name = "Edited Model", Id = 1};
            var editedModel = _repository.Update(toEditModel);
            Assert.Equal("Edited Model_UPDATED", editedModel.Name);
            Assert.Equal(toEditModel.Id, editedModel.Id);
        }


        [Fact]
        public async Task ShouldDeleteModelAsync()
        {
            const int id = 1;
            var model = await _repository.DeleteAsync(id);
            Assert.Equal("First", model.Name);
        }


        [Fact]
        public void ShouldDeleteModel()
        {
            const int id = 1;
            var model = _repository.Delete(id);
            Assert.Equal("First", model.Name);
        }


        [Fact]
        public async Task ShouldFindEntityAsync()
        {
            const int id = 1;
            var returnedModel = await _repository.FindAsync(id);
            Assert.Equal(1, returnedModel.Id);
            Assert.Equal("First", returnedModel.Name);
        }


        [Fact]
        public void ShouldFindEntity()
        {
            const int id = 1;
            var returnedModel = _repository.Find(id);
            Assert.Equal(1, returnedModel.Id);
            Assert.Equal("First", returnedModel.Name);
        }

        [Fact]
        public async Task ShouldGetPageAsync()
        {
            var models = await _repository.GetPageAsync(0, 2);
            Assert.Equal(2, models.Count);
        }

        [Fact]
        public void ShouldGetPage()
        {
            var models = _repository.GetPage(0, 2);
            Assert.Equal(2, models.Count);
        }

        [Fact]
        public async Task ShouldUsePreProcessPageQuery()
        {
            var models = await _repository.GetPageAsync(0, 2);
            Assert.Equal(2, models[0].Id);
            Assert.Equal(1, models[1].Id);
        }
    }
}