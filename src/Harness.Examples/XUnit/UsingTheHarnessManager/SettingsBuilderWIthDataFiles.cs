using System.Collections.Generic;
using Harness.Settings;
using MongoDB.Bson;
using MongoDB.Driver;
using Xunit;

namespace Harness.Examples.XUnit.UsingTheHarnessManager
{
    // Instead of using a config file and extending the HarnessBase class, you
    // can use the Harness fluent api to build a settings object and setup one
    // or more mongo databases.

    [Collection("Example.Tests")]
    public class SettingsBuilderWithDataFiles
    {
        public SettingsBuilderWithDataFiles()
        {
            var settings =
                new SettingsBuilder()
                    .AddDatabase("TestDb1")
                    .WithConnectionString("mongodb://localhost:27017")
                    .DropDatabaseFirst()
                    .AddCollection("col1", true, "Collection1.json")
                    .AddCollection("col2", true, "Collection2.json")
                    .Build();

            this.MongoConnections =
                new HarnessManager()
                    .UsingSettings(settings)
                    .Build();
        }

        private Dictionary<string, IMongoClient> MongoConnections { get; }

        [Fact]
        public void Test1()
        {
            // Arrange
            var classUnderTest = new ClassUnderTest();

            // Act
            var result = classUnderTest.GetCollectionRecordCount<BsonDocument>("col1");

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void Test2()
        {
            // Arrange
            var classUnderTest = new ClassUnderTest();

            // The method we are testing needs an instance of IMongoClient.
            // Rather than create a new one, we can re-use the one that was 
            // created by the HarnessManager class when it was setting up the 
            // databases.
            var mongoClient = this.MongoConnections["mongodb://localhost:27017"];

            // Act
            var result = classUnderTest.GetCollectionRecordCount<BsonDocument>(mongoClient, "col1");

            // Assert
            Assert.Equal(2, result);
        }
    }
}
