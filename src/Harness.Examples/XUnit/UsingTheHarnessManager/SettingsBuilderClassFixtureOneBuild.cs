using System;
using System.Collections.Generic;
using Harness.Examples.XUnit.UsingTheHarnessManager.DataProviders;
using Harness.Settings;
using MongoDB.Driver;
using Xunit;

namespace Harness.Examples.XUnit.UsingTheHarnessManager
{
    [Collection("Example.Tests")]
    public class DatabaseBuildFixture : IDisposable
    {
        public DatabaseBuildFixture()
        {
            var settings =
                new SettingsBuilder()
                    .AddDatabase("TestDb1")
                    .WithConnectionString("mongodb://localhost:27017")
                    .DropDatabaseFirst()
                    .AddCollection<Person>("people", true, new PersonDataProvider())
                    .Build();

            this.MongoConnections =
                new HarnessManager()
                    .UsingSettings(settings)
                    .Build();


            // Any other setup stuff...
        }

        public void Dispose()
        {
            // Any cleaning up...
        }

        public Dictionary<string, IMongoClient> MongoConnections { get; }
    }

    [Collection("Example.Tests")]
    public class SettingsBuilderClassFixtureOneBuild : IClassFixture<DatabaseBuildFixture>
    {
        public SettingsBuilderClassFixtureOneBuild(DatabaseBuildFixture fixture)
        {
            this.Fixture = fixture;
        }

        private DatabaseBuildFixture Fixture { get; }

        [Fact]
        public void Test1()
        {
            // Arrange
            var classUnderTest = new ClassUnderTest();

            // Act
            var result = classUnderTest.GetCollectionRecordCount<Person>("people");

            // Assert
            Assert.Equal(3, result);
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
            var mongoClient = this.Fixture.MongoConnections["mongodb://localhost:27017"];

            // Act
            var result = classUnderTest.GetCollectionRecordCount<Person>(mongoClient, "people");

            // Assert
            Assert.Equal(3, result);
        }
    }
}
