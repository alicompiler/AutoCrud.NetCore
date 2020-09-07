using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;

namespace AutoCrud.Test
{
    public static class TestUtils
    {
        public static T MakeInMemoryDbContext<T>(string dbName) where T : DbContext
        {
            var options = new DbContextOptionsBuilder<T>()
                .UseInMemoryDatabase(dbName)
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            return (T) Activator.CreateInstance(typeof(T), options);
        }

        public static void VerifyLogMessage<T>(Mock<ILogger<T>> logger, string message,
            LogLevel logLevel = LogLevel.Information)
        {
            logger.Verify(l => l.Log(
                    logLevel,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) =>
                        string.Equals(message, o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
                Times.Once
            );
        }

        public static void VerifyLogMessage(Mock<ILogger> logger, string message,
            LogLevel logLevel = LogLevel.Information)
        {
            logger.Verify(l => l.Log(
                    logLevel,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) =>
                        string.Equals(message, o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
                Times.Once
            );
        }
    }
}