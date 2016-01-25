using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harness.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using Xunit;

namespace Harness.Examples.XUnit
{
    [HarnessConfig(ConfigFilePath = "ExampleSettings.json")]
    public class UsingTheBaseClass : HarnessBase
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            var classUnderTest = new ClassUnderTest();

            // Act
            var result = classUnderTest.GetCollectionRecordCount("TestCollection1");

            // Assert
            Assert.Equal(2, result);

        }

    }

    public class ClassUnderTest
    {
        public int GetCollectionRecordCount(string collectionName)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("TestDb1");
            var collection = db.GetCollection<BsonDocument>(collectionName);
            return collection.AsQueryable().ToList().Count;
        }
    }

}
