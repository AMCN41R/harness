using System.Collections.Generic;
using System.IO.Abstractions;
using Harness.Settings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Harness
{
    public class MongoSessionManager : IMongoSessionManager
    {

        private MongoConfiguration Settings { get; }
        private IFileSystem FileSystem { get; }

        private Dictionary<string, IMongoClient> MongoClients { get; }

        public MongoSessionManager(MongoConfiguration settings)
            : this(settings, new FileSystem())
        {
        }

        internal MongoSessionManager(
            MongoConfiguration settings, IFileSystem fileSystem)
        {
            this.Settings = settings;
            this.FileSystem = fileSystem;

            this.MongoClients = new Dictionary<string, IMongoClient>();
        }

        public void Build()
        {
            foreach (var database in this.Settings.Databases)
            {
                this.CreateDatabase(database);
            }

            if (this.Settings.SaveOutput)
            {
                SaveOutput();
            }
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
            if (this.MongoClients.ContainsKey(connectionString))
            {
                return this.MongoClients[connectionString];
            }

            var mongoClient = new MongoClient(connectionString);
            this.MongoClients.Add(connectionString, mongoClient);

            return mongoClient;

        }

        private void CreateCollection(
            IMongoDatabase database, CollectionConfig config)
        {
            // Load the test data from the specified file
            var lines = 
                this.FileSystem
                    .File
                    .ReadAllLines(config.DataFileLocation);

            // Drop the collection is specified
            if (config.DropFirst)
            {
                database.DropCollection(config.CollectionName);
            }

            // Get the collection
            var collection =
                database.GetCollection<BsonDocument>(config.CollectionName);

            // Insert the data into the collection
            foreach (var line in lines)
            {
                collection.InsertOne(BsonDocument.Parse(line));
            }

        }

        private void SaveOutput()
        {
            // Save output.
        }
    }
}
