using System.Collections.Generic;
using Harness.Settings;
using MongoDB.Bson;
using MongoDB.Driver;
using Xunit;

namespace Harness.Examples.XUnit.UsingTheHarnessManager
{
    [Collection("Example.Tests")]
    public class SettingsBuilderWithDataProvider
    {
        public SettingsBuilderWithDataProvider()
        {
            var settings =
                new SettingsBuilder()
                    .AddDatabase("TestDb1")
                    .WithConnectionString("mongodb://localhost:27017")
                    .DropDatabaseFirst()
                    .AddCollection("col1", true, new PersonDataProvider())
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
            var result = classUnderTest.GetCollectionRecordCount<Person>("col1");

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
            var mongoClient = this.MongoConnections["mongodb://localhost:27017"];

            // Act
            var result = classUnderTest.GetCollectionRecordCount<Person>(mongoClient, "col1");

            // Assert
            Assert.Equal(3, result);
        }
    }

    public class PersonDataProvider : IDataProvider
    {
        public IEnumerable<object> GetData()
        {
            return new List<Person>
            {
                new Person {FirstName = "Peter", LastName = "Venkman", Age = 31},
                new Person {FirstName = "Ray", LastName = "Stantz", Age = 32},
                new Person {FirstName = "Egon", LastName = "Spengler", Age = 33}
            };
        }
    }

    public class Person
    {
        public ObjectId Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }
    }
}
