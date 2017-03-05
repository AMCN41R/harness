using System.Collections.Generic;
using System.IO.Abstractions;
using Harness.Settings;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;

namespace Harness
{
    internal interface IMongoSessionManager
    {
        Dictionary<string, IMongoClient> Build();
    }

    internal class MongoSessionManager : IMongoSessionManager
    {
        /// <summary>
        /// Creates a new instance of the MongoSessionManager.
        /// </summary>
        /// <param name="settings">THe <see cref="HarnessConfiguration"/> settings to use.</param>
        public MongoSessionManager(HarnessConfiguration settings)
            : this(settings, new MongoClientFactory(), new FileSystem())
        {
        }

        /// <summary>
        /// Internal constructor to allow dependency injection for unit testing.
        /// </summary>
        internal MongoSessionManager(
            HarnessConfiguration settings, IMongoClientFactory clientFactory, IFileSystem fileSystem)
        {
            this.Settings = settings;
            this.ClientFactory = clientFactory;
            this.FileSystem = fileSystem;

            this.MongoClients = new Dictionary<string, IMongoClient>();
        }

        /// <summary>
        /// Gets the Mongo configuration settings.
        /// </summary>
        private HarnessConfiguration Settings { get; }

        /// <summary>
        /// Gets the mongo client factory.
        /// </summary>
        private IMongoClientFactory ClientFactory { get; }

        /// <summary>
        /// Gets the file system implementation.
        /// </summary>
        private IFileSystem FileSystem { get; }

        /// <summary>
        /// Gets the dictionary of mongo clients.
        /// </summary>
        private Dictionary<string, IMongoClient> MongoClients { get; }

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
            var databaseName = config.GetDatabaseName();

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
                this.CreateCollection(database, collection, config.CollectionNameSuffix);
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
            var mongoClient = this.ClientFactory.GetNewClient(connectionString);

            this.MongoClients.Add(connectionString, mongoClient);

            return mongoClient;
        }

        private void CreateCollection(
            IMongoDatabase database, CollectionConfig config, string collectionSuffix)
        {
            // Concat the database sufix and name if a suffix is specified
            var collectionName = config.GetCollectionName(collectionSuffix);

            // Drop the collection is specified
            if (config.DropFirst)
            {
                database.DropCollection(collectionName);
            }

            // Load the test data from the specified file
            var provider =
                config.DataProvider
                ?? new FromJsonFileDataProvider(config.DataFileLocation, this.FileSystem);

            var lines =
                provider
                    .GetData()
                    .Select(x => x.ToBsonDocument(config.DataProviderType))
                    .ToList();

            // Get the collection
            var collection = database.GetCollection<BsonDocument>(collectionName);

            // Insert the data into the collection
            collection.InsertMany(lines);
        }
    }
}
