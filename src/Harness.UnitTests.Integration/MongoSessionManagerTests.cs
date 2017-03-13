using System.Linq;
using System.Threading.Tasks;
using Harness.Settings;
using Harness.UnitTests.Integration.DataProviders;
using MongoDB.Bson;
using MongoDB.Driver;
using Xunit;

namespace Harness.UnitTests.Integration
{
    public class MongoSessionManagerTests
    {
        [Fact]
        public async Task Build_DropEverything_BuildsDatabase()
        {
            // Arrange
            var settings =
                new SettingsBuilder()
                    .AddDatabase("test1")
                    .WithConnectionString("mongodb://localhost:27017")
                    .DropDatabaseFirst()
                    .AddCollection("col1", true, "Collection1.json")
                    .AddCollection<Person>("people", true, new PersonDataProvider())
                    .AddAnotherDatabase("test2")
                    .WithConnectionString("mongodb://localhost:27017")
                    .DropDatabaseFirst()
                    .AddCollection("col2", true, "Collection2.json")
                    .Build();

            var sut = new MongoSessionManager(settings);


            // Act
            var connections = sut.Build();


            // Assert
            Assert.Equal(1, connections.Count);

            var mongo = connections["mongodb://localhost:27017"];

            var test1 = mongo.GetDatabase("test1");
            var col1 = test1.GetCollection<BsonDocument>("col1");
            var results1 = (await col1.FindAsync<BsonDocument>(new BsonDocument())).ToList();
            Assert.Equal(2, results1.Count);
            Assert.Equal("Value1b", results1[0].GetElement("Col1b").Value);
            Assert.Equal("Value2b", results1[0].GetElement("Col2b").Value);
            Assert.Equal("Value3b", results1[1].GetElement("Col1b").Value);
            Assert.Equal("Value4b", results1[1].GetElement("Col2b").Value);

            var peopleCol = test1.GetCollection<BsonDocument>("people");
            var people = (await peopleCol.FindAsync<Person>(new BsonDocument())).ToList();
            Assert.Equal(3, people.Count);
            var data = new PersonDataProvider().GetData().ToList();
            Assert.True(people[0].Equals(data[0]));
            Assert.True(people[1].Equals(data[1]));
            Assert.True(people[2].Equals(data[2]));

            var test2 = mongo.GetDatabase("test2");
            var col2 = test2.GetCollection<BsonDocument>("col2");
            var results2 = (await col2.FindAsync<BsonDocument>(new BsonDocument())).ToList();
            Assert.Equal(2, results2.Count);
        }

        [Fact]
        public async Task Build_DontDrop_BuildsDatabase()
        {
            // Arrange
            var mongo = new MongoClient();
            var db = mongo.GetDatabase("testExisting");
            var collection = db.GetCollection<Person>("people");

            await collection.InsertOneAsync(new Person
            {
                FirstName = "John",
                LastName = "Smith",
                Age = 33
            });

            var settings =
                new SettingsBuilder()
                    .AddDatabase("testExisting")
                    .WithConnectionString("mongodb://localhost:27017")
                    .AddCollection<Person>("people", false, new PersonDataProvider())
                    .Build();

            var sut = new MongoSessionManager(settings);


            // Act
            sut.Build();


            // Assert
            var people = await collection.Find(new BsonDocument()).ToListAsync();

            Assert.Equal(4, people.Count);
        }
    }
}
