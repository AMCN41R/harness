using System.Collections.Generic;
using System.Linq;

namespace Harness.Settings
{
    public class SettingsBuilder :
        ISettingsBuilder,
        ISettingsBuilderConnectionString,
        ISettingsBuilderDatabseOptions,
        ISettingsBuilderAddMoreCollections
    {
        public SettingsBuilder()
        {
            this.Config = new MongoConfiguration();
        }

        private MongoConfiguration Config { get; }

        private string CurrentDatabaseName { get; set; }

        public ISettingsBuilderConnectionString AddDatabase(string name)
        {
            this.AddDatabaseToConfig(name);
            return this;
        }

        MongoConfiguration ISettingsBuilder.Build()
        {
            return this.Config;
        }

        ISettingsBuilderDatabseOptions ISettingsBuilderConnectionString.WithConnectionString(string connectionString)
        {
            Guard.AgainstNullEmptyOrWhitespace(nameof(connectionString));

            var db = this.GetDatabaseConfig(this.CurrentDatabaseName);

            if (db == null)
            {
                throw new SettingsBuilderException(
                    $"An error occurred trying to add the connection string to the database {this.CurrentDatabaseName}.");
            }

            db.ConnectionString = connectionString;

            return this;
        }

        ISettingsBuilderDatabseOptions ISettingsBuilderDatabseOptions.WithDatabaseNameSuffix(string suffix)
        {
            Guard.AgainstNullEmptyOrWhitespace(nameof(suffix));

            var db = this.GetDatabaseConfig(this.CurrentDatabaseName);

            if (db == null)
            {
                throw new SettingsBuilderException(
                    $"An error occurred trying to add configuration for the database {this.CurrentDatabaseName}.");
            }

            db.DatabaseNameSuffix = suffix;

            return this;
        }

        ISettingsBuilderDatabseOptions ISettingsBuilderDatabseOptions.WithCollectionNameSuffix(string suffix)
        {
            Guard.AgainstNullEmptyOrWhitespace(nameof(suffix));

            var db = this.GetDatabaseConfig(this.CurrentDatabaseName);

            if (db == null)
            {
                throw new SettingsBuilderException(
                    $"An error occurred trying to add configuration for the database {this.CurrentDatabaseName}.");
            }

            db.CollectionNameSuffix = suffix;

            return this;
        }

        ISettingsBuilderDatabseOptions ISettingsBuilderDatabseOptions.DropDatabaseFirst()
        {
            var db = this.GetDatabaseConfig(this.CurrentDatabaseName);

            if (db == null)
            {
                throw new SettingsBuilderException(
                    $"An error occurred trying to add configuration for the database {this.CurrentDatabaseName}.");
            }

            db.DropFirst = true;

            return this;
        }

        ISettingsBuilderAddMoreCollections ISettingsBuilderDatabseOptions.AddCollection(string name, bool dropFirst, string fileLocation)
        {
            this.AddCollectionToConfig(name, dropFirst, fileLocation);
            return this;
        }

        ISettingsBuilderAddMoreCollections ISettingsBuilderAddMoreCollections.AddCollection(string name, bool dropFirst, string fileLocation)
        {
            this.AddCollectionToConfig(name, dropFirst, fileLocation);
            return this;
        }

        ISettingsBuilderConnectionString ISettingsBuilderAddMoreCollections.AddAnotherDatabase(string name)
        {
            this.AddDatabaseToConfig(name);
            return this;
        }

        MongoConfiguration ISettingsBuilderAddMoreCollections.Build()
        {
            return this.Config;
        }

        private DatabaseConfig GetDatabaseConfig(string name)
        {
            return this.Config.Databases.SingleOrDefault(x => x.DatabaseName == name);
        }

        private void AddDatabaseToConfig(string name)
        {
            Guard.AgainstNullEmptyOrWhitespace(nameof(name));

            if (this.Config.Databases == null)
            {
                this.Config.Databases = new List<DatabaseConfig>();
            }

            if (this.GetDatabaseConfig(name) != null)
            {
                throw new SettingsBuilderException(
                    $"Cannot add database with name {name} because it has already been added to this configuration.");
            }

            this.Config.Databases.Add(new DatabaseConfig { DatabaseName = name });
            this.CurrentDatabaseName = name;
        }

        private void AddCollectionToConfig(string name, bool dropFirst, string fileLocation)
        {
            Guard.AgainstNullEmptyOrWhitespace(nameof(name));
            Guard.AgainstNullEmptyOrWhitespace(nameof(fileLocation));

            var db = this.GetDatabaseConfig(this.CurrentDatabaseName);

            if (db == null)
            {
                throw new SettingsBuilderException(
                    $"An error occurred trying to add a collection to the database {this.CurrentDatabaseName}.");
            }

            var collection = db.Collections?.SingleOrDefault(x => x.CollectionName == name);

            if (collection != null)
            {
                throw new SettingsBuilderException(
                   $"Cannot add collection with name {name} because it has already been added to this configuration.");
            }

            if (db.Collections == null)
            {
                db.Collections = new List<CollectionConfig>();
            }

            db.Collections.Add(
                new CollectionConfig
                {
                    CollectionName = name,
                    DropFirst = dropFirst,
                    DataFileLocation = fileLocation
                }
            );
        }
    }

    public interface ISettingsBuilder
    {
        ISettingsBuilderConnectionString AddDatabase(string name);

        MongoConfiguration Build();
    }

    public interface ISettingsBuilderConnectionString
    {
        ISettingsBuilderDatabseOptions WithConnectionString(string connectionString);
    }

    public interface ISettingsBuilderDatabseOptions
    {
        ISettingsBuilderDatabseOptions WithDatabaseNameSuffix(string suffix);

        ISettingsBuilderDatabseOptions WithCollectionNameSuffix(string suffix);

        ISettingsBuilderDatabseOptions DropDatabaseFirst();

        ISettingsBuilderAddMoreCollections AddCollection(string name, bool dropFirst, string fileLocation);
    }

    public interface ISettingsBuilderAddMoreCollections
    {
        ISettingsBuilderAddMoreCollections AddCollection(string name, bool dropFirst, string fileLocation);

        ISettingsBuilderConnectionString AddAnotherDatabase(string name);

        MongoConfiguration Build();
    }
}
