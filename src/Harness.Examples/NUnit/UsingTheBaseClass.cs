using MongoDB.Bson;
using NUnit.Framework;

namespace Harness.Examples.NUnit
{
    [TestFixture]
    [HarnessConfig(ConfigFilePath = "ExampleSettings.json")]
    public class UsingTheBaseClass : HarnessBase
    {
        [Test]
        public void Test1()
        {
            // Arrange
            var classUnderTest = new ClassUnderTest();

            // Act
            var result = classUnderTest.GetCollectionRecordCount<BsonDocument>("TestCollection1");

            // Assert
            Assert.AreEqual(2, result);

        }

        [Test]
        public void Test2()
        {
            // Arrange
            var classUnderTest = new ClassUnderTest();
            var mongoClient = this.MongoConnections["mongodb://localhost:27017"];

            // Act
            var result = classUnderTest.GetCollectionRecordCount<BsonDocument>(mongoClient, "TestCollection1");

            // Assert
            Assert.AreEqual(2, result);

        }

    }
}
