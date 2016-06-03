using System.Collections.Generic;
using System.IO.Abstractions;
using Harness.Settings;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Harness
{
    internal class MongoSessionManager : IMongoSessionManager
    {
        /// <summary>
        /// Gets the Mongo configuration settings.
        /// </summary>
        private MongoConfiguration Settings { get; }

        /// <summary>
        /// Gets the file system implementation.
        /// </summary>
        private IFileSystem FileSystem { get; }

        /// <summary>
        /// TODO:
        /// </summary>
        private Dictionary<string, IMongoClient> MongoClients { get; }

        /// <summary>
        /// Creates a new instance of the MongoSessionManager.
        /// </summary>
        /// <param name="settings">THe <see cref="MongoConfiguration"/> settings to use.</param>
        public MongoSessionManager(MongoConfiguration settings)
            : this(settings, new FileSystem())
        {
        }

        /// <summary>
        /// Internal constructor to allow dependency injection for unit testing.
        /// </summary>
        internal MongoSessionManager(
            MongoConfiguration settings, IFileSystem fileSystem)
        {
            this.Settings = settings;
            this.FileSystem = fileSystem;

            this.MongoClients = new Dictionary<string, IMongoClient>();
        }

        /// <summary>
        /// Configures the state of one or more Mongo databases using the 
        /// provided settings.
        /// </summary>
        public Dictionary<string, IMongoClient> Build()
        {
            foreach (var database in this.Settings.Databases)
            {
                this.CreateDatabase(database);
            }

            return this.MongoClients;
        }

        private void CreateDatabase(DatabaseConfig config)
        {
            // Get the mongo client
            var client = GetMongoClient(config.ConnectionString);

            // Concat the database sufix and name if a suffix is specified
            var databaseName =
                string.IsNullOrWhiteSpace(config.DatabaseNameSuffix)
                    ? config.DatabaseNameSuffix + config.DatabaseName
                    : config.DatabaseName;

            // Drop the database is specified
            if (config.DropFirst)
            {
                client.DropDatabase(databaseName);
            }

            // Get the database
            var database = client.GetDatabase(databaseName);

            // Add the collections to the database
            foreach (var collection in config.Collections)
            {
                this.CreateCollection(database, collection);
            }
        }

        private IMongoClient GetMongoClient(string connectionString)
        {
            // Check if we have already cached a connection to the 
            // required mongo database, and return it if we have.
            if (this.MongoClients.ContainsKey(connectionString))
            {
                return this.MongoClients[connectionString];
            }

            // Otherwise, create a new connection and cache it for later.
            var mongoClient = new MongoClient(connectionString);

            this.MongoClients.Add(connectionString, mongoClient);

            return mongoClient;
        }

        private void CreateCollection(
            IMongoDatabase database, CollectionConfig config)
        {
            // Drop the collection is specified
            if (config.DropFirst)
            {
                database.DropCollection(config.CollectionName);
            }

            // Load the test data from the specified file
            var lines = GetTestDataFromFile(config.DataFileLocation);

            if (lines == null)
            {
                return;
            }

            // Get the collection
            var collection =
                database.GetCollection<BsonDocument>(config.CollectionName);

            // Insert the data into the collection
            collection.InsertMany(lines);
        }

        private IEnumerable<BsonDocument> GetTestDataFromFile(string path)
        {
            // Return null if the given filepath is not valid
            if (!this.FileSystem.ValidateFile(path)?.IsValid ?? false)
            {
                return null;
            }

            var itemArray =
                JsonConvert.DeserializeObject(this.FileSystem.File.ReadAllText(path)) as JArray;

            return itemArray?.Select(x => BsonDocument.Parse(x.ToString()));
        }

        /// <summary>
        /// TODO:
        /// </summary>
        public void SaveOutput()
        {
            if (this.Settings.SaveOutput)
            {
                // Save Output
            }
        }

    }

    public class TestData
    {
        public List<string> Objs { get; set; }
    }
}
