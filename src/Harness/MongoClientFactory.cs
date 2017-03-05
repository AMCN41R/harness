using MongoDB.Driver;

namespace Harness
{
    internal interface IMongoClientFactory
    {
        IMongoClient GetNewClient();

        IMongoClient GetNewClient(string connectionString);
    }

    internal class MongoClientFactory : IMongoClientFactory
    {
        public IMongoClient GetNewClient()
        {
            return new MongoClient();
        }

        public IMongoClient GetNewClient(string connectionString)
        {
            return new MongoClient(connectionString);
        }
    }
}