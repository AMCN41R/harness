using Harness.Settings;

namespace Harness
{
    internal interface IMongoSessionManagerFactory
    {
        IMongoSessionManager GetMongoSessionManager(HarnessConfiguration configuration);
    }

    internal class MongoSessionManagerFactory : IMongoSessionManagerFactory
    {
        public IMongoSessionManager GetMongoSessionManager(HarnessConfiguration configuration)
        {
            Guard.AgainstNullArgument(configuration, nameof(configuration));

            return new MongoSessionManager(configuration);
        }
    }
}
