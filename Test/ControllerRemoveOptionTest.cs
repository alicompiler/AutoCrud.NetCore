using System.Threading.Tasks;
using AutoCrud.Test.Dummy;
using Xunit;

namespace AutoCrud.Test
{
    public class ControllerRemoveOptionTest
    {
        [Fact]
        public void ShouldThrowWhenRemovedActionCalledForSyncController()
        {
            var controller = new DummyRemoveOptionController(null , null);
            // ReSharper disable once JoinDeclarationAndInitializer
            UnsupportedActionException exception;
            
            exception = Assert.Throws<UnsupportedActionException>(() => controller.GetPage());
            Assert.Equal("GET PAGE NOT SUPPORTED" , exception.Message);
            
            exception = Assert.Throws<UnsupportedActionException>(() => controller.Find(1));
            Assert.Equal("FIND ENTITY NOT SUPPORTED" , exception.Message);
            
            exception = Assert.Throws<UnsupportedActionException>(() => controller.Create(null));
            Assert.Equal("CREATE NOT SUPPORTED" , exception.Message);
            
            exception = Assert.Throws<UnsupportedActionException>(() => controller.Update(1 , null));
            Assert.Equal("UPDATE NOT SUPPORTED" , exception.Message);

            exception = Assert.Throws<UnsupportedActionException>(() => controller.Delete(1));
            Assert.Equal("DELETE NOT SUPPORTED" , exception.Message);
        }
        
        [Fact]
        public async Task ShouldThrowWhenRemovedActionCalledForAsyncController()
        {
            var controller = new DummyRemoveOptionControllerAsync(null , null);
            // ReSharper disable once JoinDeclarationAndInitializer
            UnsupportedActionException exception;
            
            exception = await Assert.ThrowsAsync<UnsupportedActionException>(() => controller.GetPage());
            Assert.Equal("GET PAGE NOT SUPPORTED" , exception.Message);
            
            exception = await Assert.ThrowsAsync<UnsupportedActionException>(() => controller.Find(1));
            Assert.Equal("FIND ENTITY NOT SUPPORTED" , exception.Message);
            
            exception = await Assert.ThrowsAsync<UnsupportedActionException>(() => controller.Create(null));
            Assert.Equal("CREATE NOT SUPPORTED" , exception.Message);
            
            exception = await Assert.ThrowsAsync<UnsupportedActionException>(() => controller.Update(1 , null));
            Assert.Equal("UPDATE NOT SUPPORTED" , exception.Message);

            exception = await Assert.ThrowsAsync<UnsupportedActionException>(() => controller.Delete(1));
            Assert.Equal("DELETE NOT SUPPORTED" , exception.Message);
        }
    }
}