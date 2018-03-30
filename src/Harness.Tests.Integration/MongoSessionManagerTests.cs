using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Harness.Settings;
using Harness.Tests.Integration.DataProviders;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Xunit;

namespace Harness.Tests.Integration
{
    public class MongoSessionManagerTests : IDisposable
    {
        private IMongoClient Client { get; set; }

        private List<string> DbNames { get; } = new List<string>();

        [Fact]
        public async Task Build_DropEverything_BuildsDatabase()
        {
            // Arrange
            var settings =
                new SettingsBuilder()
                    .AddConvention(new CamelCaseElementNameConvention(), x => true)
                    .AddDatabase("test1")
                    .WithConnectionString("mongodb://localhost:27017")
                    .DropDatabaseFirst()
                    .AddCollection("col1", true, "Collection1.json")
                    .AddCollection<Person>("people", true, new PersonDataProvider())
                    .AddDatabase("test2")
                    .WithConnectionString("mongodb://localhost:27017")
                    .DropDatabaseFirst()
                    .AddCollection("col2", true, "Collection2.json")
                    .Build();

            var sut = new MongoSessionManager(settings);

            this.DbNames.Add("test1");
            this.DbNames.Add("test2");


            // Act
            var connections = sut.Build();


            // Assert
            Assert.Single(connections);

            this.Client = connections["mongodb://localhost:27017"];

            var test1 = this.Client.GetDatabase("test1");
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
            Assert.True(people[0].IsEqual(data[0] as Person));
            Assert.True(people[1].IsEqual(data[1] as Person));
            Assert.True(people[2].IsEqual(data[2] as Person));

            var test2 = this.Client.GetDatabase("test2");
            var col2 = test2.GetCollection<BsonDocument>("col2");
            var results2 = (await col2.FindAsync<BsonDocument>(new BsonDocument())).ToList();
            Assert.Equal(2, results2.Count);
        }

        [Fact]
        public async Task Build_DontDrop_BuildsDatabase()
        {
            IMongoDatabase database = null;
            try
            {
                // Arrange
                var mongo = new MongoClient();
                database = mongo.GetDatabase("testExisting");
                var collection = database.GetCollection<Person>("people");

                await collection.InsertOneAsync(new Person
                {
                    FirstName = "John",
                    LastName = "Smith",
                    Age = 33
                });

                var settings =
                    new SettingsBuilder()
                        .AddConvention(new List<IConvention> { new CamelCaseElementNameConvention()}, x => true)
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
            finally
            {
                database?.DropCollection("people");
            }
        }

        public void Dispose()
        {
            this.DbNames?.ForEach(x => this.Client?.DropDatabase(x));
        }
    }
}
