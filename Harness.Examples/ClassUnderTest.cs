using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Harness.Examples
{
    /// <summary>
    /// A very simple example data access class that will be used to illustrate 
    /// how to use Harness when writing MongoDB integration tests.
    /// </summary>
    public class ClassUnderTest
    {
        /// <summary>
        /// A sample method that takes in the name of a MongoDB collection, 
        /// connects to the database and returns a count of records in the 
        /// given collection.
        /// </summary>
        public int GetCollectionRecordCount(string collectionName)
        {
            var client = new MongoClient("mongodb://192.168.99.100:27017");

            var db = client.GetDatabase("TestDb1");

            var collection = db.GetCollection<BsonDocument>(collectionName);

            return collection.AsQueryable().ToList().Count;
        }

        /// <summary>
        /// A sample method that connections to the given mongo client and 
        /// returns a count of records in the given collection.
        /// </summary>
        public int GetCollectionRecordCount(IMongoClient client, string collectionName)
        {
            var db = client.GetDatabase("TestDb1");

            var collection = db.GetCollection<BsonDocument>(collectionName);

            return collection.AsQueryable().ToList().Count;
        }
    }
}
