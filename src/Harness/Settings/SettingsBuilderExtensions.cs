using System;
using System.Collections.Generic;
using System.Linq;

namespace Harness.Settings
{
    internal static class SettingsBuilderExtensions
    {
        public static HarnessConfiguration AddDatabase(this HarnessConfiguration config, string name)
        {
            Guard.AgainstNullArgument(config, nameof(config));
            Guard.AgainstNullEmptyOrWhitespace(name, nameof(name));

            if (config.Databases == null)
            {
                config.Databases = new List<DatabaseConfig>();
            }

            if (config.Databases.SingleOrDefault(x => x.DatabaseName == name) != null)
            {
                throw new SettingsBuilderException(
                    $"Cannot add database with name {name} because it has already been added to this configuration.");
            }

            config.Databases.Add(new DatabaseConfig { DatabaseName = name });

            return config;
        }

        public static DatabaseConfig SetValue(this DatabaseConfig config, Action<DatabaseConfig> setter)
        {
            Guard.AgainstNullArgument(config, nameof(config));
            Guard.AgainstNullArgument(setter, nameof(setter));

            setter(config);

            return config;
        }

        public static DatabaseConfig AddCollection(this DatabaseConfig db, string name, bool dropFirst, string fileLocation)
        {
            Guard.AgainstNullArgument(db, nameof(db));
            Guard.AgainstNullEmptyOrWhitespace(name, nameof(name));
            Guard.AgainstNullEmptyOrWhitespace(fileLocation, nameof(fileLocation));

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
                    DataFileLocation = fileLocation,
                    DataProvider = null
                }
            );

            return db;
        }

        public static DatabaseConfig AddDataProviderCollection<T>(this DatabaseConfig db, string name, bool dropFirst, IDataProvider dataProvider)
        {
            Guard.AgainstNullArgument(db, nameof(db));
            Guard.AgainstNullEmptyOrWhitespace(name, nameof(name));
            Guard.AgainstNullArgument(dataProvider, nameof(dataProvider));

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
                    DataFileLocation = null,
                    DataProvider = dataProvider,
                    DataProviderType = typeof(T)
                }
            );

            return db;
        }

        public static string GetCollectionName(this CollectionConfig config, string suffix)
        {
            Guard.AgainstNullArgument(config, nameof(config));

            return
                string.IsNullOrWhiteSpace(suffix)
                    ? config.CollectionName
                    : $"{config.CollectionName}{suffix}";
        }
    }
}