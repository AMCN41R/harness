using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Harness.Examples
{
    public class ClassUnderTest
    {
        public int GetCollectionRecordCount(string collectionName)
        {
            var client = new MongoClient("mongodb://192.168.99.100:27017");
            var db = client.GetDatabase("TestDb1");
            var collection = db.GetCollection<BsonDocument>(collectionName);
            return collection.AsQueryable().ToList().Count;
        }

        public int GetCollectionRecordCount(IMongoClient client, string collectionName)
        {
            var db = client.GetDatabase("TestDb1");
            var collection = db.GetCollection<BsonDocument>(collectionName);
            return collection.AsQueryable().ToList().Count;
        }
    }
}
