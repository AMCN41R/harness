using System;
using Xunit;

namespace Harness.Tests.Unit
{
    public class MongoSessionManagerFactoryTests
    {
        [Fact]
        public void GetMongoSessionManager_NullHarnessConfiguration_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new MongoSessionManagerFactory().GetMongoSessionManager(null));
        }
    }
}
