using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using Xunit;
using NSubstitute;
using Harness.Settings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Harness.UnitTests.Integration
{
    public class MongoSessionManagerTests
    {
        [Fact]
        public void Build_ValidSettingsAndDataFiles_CreatesMongoDatabaseAndCollections()
        {
            // Arrange 
            var fakeFileSystem = Substitute.For<IFileSystem>();
            fakeFileSystem
                .File
                .Exists(Arg.Any<string>())
                .Returns(true);
            fakeFileSystem
                .File
                .ReadAllLines(Arg.Any<string>())
                .Returns(this.TestData);

            var classUnderTest =
                new MongoSessionManager(this.TestSettings, fakeFileSystem);

            // Act
            classUnderTest.Build();

            // Assert
            var client = new MongoClient("mongodb://localhost:27017");
            var countCollection1 = 
                client
                    .GetDatabase("TestDb1")
                    .GetCollection<BsonDocument>("TestCollection1")
                    .AsQueryable()
                    .ToList()
                    .Count;

            var countCollection2 =
                client
                    .GetDatabase("TestDb1")
                    .GetCollection<BsonDocument>("TestCollection2")
                    .AsQueryable()
                    .ToList()
                    .Count;

            Assert.Equal(2, countCollection1);
            Assert.Equal(2, countCollection2);

        }



        private string[] TestData
            => new[]
            {
                "{\"Col1b\": \"Value1b\", \"Col2b\": \"Value2b\"}",
                "{\"Col1b\": \"Value3b\", \"Col2b\": \"Value4b\"}"
            };


        private MongoConfiguration TestSettings
            =>
                new MongoConfiguration
                {
                    SaveOutput = false,
                    Databases =
                        new List<DatabaseConfig>
                        {
                            new DatabaseConfig
                            {
                                DatabaseName = "TestDb1",
                                ConnectionString = "mongodb://localhost:27017",
                                DatabaseNameSuffix = "",
                                CollectionNameSuffix = "",
                                DropFirst = true,
                                Collections =
                                    new List<CollectionConfig>
                                    {
                                        new CollectionConfig
                                        {
                                            CollectionName = "TestCollection1",
                                            DataFileLocation = "TestData\\Collection1.json",
                                            DropFirst = false
                                        },
                                        new CollectionConfig
                                        {
                                            CollectionName = "TestCollection2",
                                            DataFileLocation = "TestData\\Collection2.json",
                                            DropFirst = false
                                        }
                                    }
                            }
                        }

                };

    }
}
