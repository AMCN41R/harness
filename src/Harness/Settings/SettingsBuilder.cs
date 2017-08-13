using System.Linq;
using MongoDB.Bson.Serialization.Conventions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Harness.Settings
{
    /// <summary>
    /// A fluent api that lets you build the Harness configuration object in 
    /// code instead of using an external json file.
    /// </summary>
    public class SettingsBuilder :
        ISettingsBuilder,
        ISettingsBuilderDatabase,
        ISettingsBuilderConnectionString,
        ISettingsBuilderDatabaseOptions,
        ISettingsBuilderAddMoreCollections
    {
        public SettingsBuilder()
        {
            this.Config = new HarnessConfiguration();
        }

        /// <summary>
        /// Gets the <see cref="HarnessConfiguration"/> object.
        /// </summary>
        private HarnessConfiguration Config { get; }

        /// <summary>
        /// Gets the name of the database that is currently being configured.
        /// </summary>
        private string CurrentDatabaseName { get; set; }

        /// <inheritdoc />
        public ISettingsBuilderConnectionString AddDatabase(string name)
        {
            Guard.AgainstNullEmptyOrWhitespace(name, nameof(name));

            this.Config.AddDatabase(name);
            this.CurrentDatabaseName = name;
            return this;
        }

        /// <inheritdoc />
        public ISettingsBuilderDatabase AddConvention(IConvention convention, Expression<Func<Type, bool>> filter)
        {
            Guard.AgainstNullArgument(convention, nameof(convention));
            Guard.AgainstNullArgument(filter, nameof(filter));

            this.Config.SetConventions(new ConventionPack {convention}, filter);
            return this;
        }

        /// <inheritdoc />
        public ISettingsBuilderDatabase AddConvention(IList<IConvention> conventions, Expression<Func<Type, bool>> filter)
        {
            Guard.AgainstNullArgument(conventions, nameof(conventions));
            Guard.AgainstNullArgument(filter, nameof(filter));

            if (!conventions.Any())
            {
                return this;
            }

            var pack = new ConventionPack();
            pack.AddRange(conventions);

            this.Config.SetConventions(pack, filter);
            return this;
        }

        /// <inheritdoc />
        public ISettingsBuilderDatabase AddConvention(IConventionPack conventions, Expression<Func<Type, bool>> filter)
        {
            Guard.AgainstNullArgument(conventions, nameof(conventions));
            Guard.AgainstNullArgument(filter, nameof(filter));

            this.Config.SetConventions(conventions, filter);
            return this;
        }

        /// <inheritdoc />
        ISettingsBuilderDatabaseOptions ISettingsBuilderConnectionString.WithConnectionString(string connectionString)
        {
            Guard.AgainstNullEmptyOrWhitespace(connectionString, nameof(connectionString));

            this.GetDatabaseConfig().SetValue(x => x.ConnectionString = connectionString);
            return this;
        }

        /// <inheritdoc />
        ISettingsBuilderDatabaseOptions ISettingsBuilderDatabaseOptions.WithCollectionNameSuffix(string suffix)
        {
            Guard.AgainstNullEmptyOrWhitespace(suffix, nameof(suffix));

            this.GetDatabaseConfig().SetValue(x => x.CollectionNameSuffix = suffix);
            return this;
        }

        /// <inheritdoc />
        ISettingsBuilderDatabaseOptions ISettingsBuilderDatabaseOptions.DropDatabaseFirst()
        {
            this.GetDatabaseConfig().SetValue(x => x.DropFirst = true);
            return this;
        }

        /// <inheritdoc />
        ISettingsBuilderAddMoreCollections ISettingsBuilderDatabaseOptions.AddCollection(string name, bool dropFirst, string fileLocation)
        {
            Guard.AgainstNullEmptyOrWhitespace(name, nameof(name));
            Guard.AgainstNullEmptyOrWhitespace(fileLocation, nameof(fileLocation));

            this.GetDatabaseConfig().AddCollection(name, dropFirst, fileLocation);
            return this;
        }

        /// <inheritdoc />
        ISettingsBuilderAddMoreCollections ISettingsBuilderAddMoreCollections.AddCollection(string name, bool dropFirst, string fileLocation)
        {
            Guard.AgainstNullEmptyOrWhitespace(name, nameof(name));
            Guard.AgainstNullEmptyOrWhitespace(fileLocation, nameof(fileLocation));

            this.GetDatabaseConfig().AddCollection(name, dropFirst, fileLocation);
            return this;
        }

        /// <inheritdoc />
        ISettingsBuilderConnectionString ISettingsBuilderAddMoreCollections.AddDatabase(string name)
        {
            Guard.AgainstNullEmptyOrWhitespace(name, nameof(name));

            this.Config.AddDatabase(name);
            this.CurrentDatabaseName = name;
            return this;
        }

        /// <inheritdoc />
        HarnessConfiguration ISettingsBuilderAddMoreCollections.Build()
        {
            return this.Config;
        }

        /// <inheritdoc />
        ISettingsBuilderAddMoreCollections ISettingsBuilderDatabaseOptions.AddCollection<T>(string name, bool dropFirst, IDataProvider dataProvider)
        {
            Guard.AgainstNullEmptyOrWhitespace(name, nameof(name));
            Guard.AgainstNullArgument(dataProvider, nameof(dataProvider));

            this.GetDatabaseConfig().AddDataProviderCollection<T>(name, dropFirst, dataProvider);
            return this;
        }

        /// <inheritdoc />
        ISettingsBuilderAddMoreCollections ISettingsBuilderAddMoreCollections.AddCollection<T>(string name, bool dropFirst, IDataProvider dataProvider)
        {
            Guard.AgainstNullEmptyOrWhitespace(name, nameof(name));
            Guard.AgainstNullArgument(dataProvider, nameof(dataProvider));

            this.GetDatabaseConfig().AddDataProviderCollection<T>(name, dropFirst, dataProvider);
            return this;
        }

        private DatabaseConfig GetDatabaseConfig()
        {
            return this.Config.Databases.SingleOrDefault(x => x.DatabaseName == this.CurrentDatabaseName);
        }
    }

    public interface ISettingsBuilder
    {
        /// <summary>
        /// Adds a convention to the Mongo ConventionRegistry.
        /// </summary>
        /// <param name="convention">The convention to add.</param>
        /// <param name="filter">
        /// The filter to select the types to which the convention will be applied.
        /// </param>
        /// <returns></returns>
        ISettingsBuilderDatabase AddConvention(IConvention convention, Expression<Func<Type, bool>> filter);

        /// <summary>
        /// Adds conventions to the Mongo ConventionRegistry.
        /// </summary>
        /// <param name="conventions">The conventions to add.</param>
        /// <param name="filter">
        /// The filter to select the types to which the convention will be applied.
        /// </param>
        /// <returns></returns>
        ISettingsBuilderDatabase AddConvention(IList<IConvention> conventions, Expression<Func<Type, bool>> filter);

        /// <summary>
        /// Adds a conventions to the Mongo ConventionRegistry.
        /// </summary>
        /// <param name="conventions">The convention to add.</param>
        /// <param name="filter">
        /// The filter to select the types to which the convention will be applied.
        /// </param>
        /// <returns></returns>
        ISettingsBuilderDatabase AddConvention(IConventionPack conventions, Expression<Func<Type, bool>> filter);

        /// <summary>
        /// Adds a new database configuration to the <see cref="HarnessConfiguration"/> object. 
        /// </summary>
        /// <param name="name">The name of the MongoDb database.</param>
        /// <returns>
        /// An <see cref="ISettingsBuilderConnectionString"/> instance.
        /// </returns>
        ISettingsBuilderConnectionString AddDatabase(string name);
    }

    public interface ISettingsBuilderDatabase
    {
        /// <summary>
        /// Adds a new database configuration to the <see cref="HarnessConfiguration"/> object. 
        /// </summary>
        /// <param name="name">The name of the MongoDb database.</param>
        /// <returns>
        /// An <see cref="ISettingsBuilderConnectionString"/> instance.
        /// </returns>
        ISettingsBuilderConnectionString AddDatabase(string name);
    }

    public interface ISettingsBuilderConnectionString
    {
        /// <summary>
        /// Adds the connection string to the current database configuration.
        /// </summary>
        /// <param name="connectionString">The MongoDb connection string.</param>
        /// <returns>
        /// An <see cref="ISettingsBuilderDatabaseOptions"/> instance.
        /// </returns>
        ISettingsBuilderDatabaseOptions WithConnectionString(string connectionString);
    }

    public interface ISettingsBuilderDatabaseOptions
    {
        ISettingsBuilderDatabaseOptions WithCollectionNameSuffix(string suffix);

        ISettingsBuilderDatabaseOptions DropDatabaseFirst();

        ISettingsBuilderAddMoreCollections AddCollection(string name, bool dropFirst, string fileLocation);

        ISettingsBuilderAddMoreCollections AddCollection<T>(string name, bool dropFirst, IDataProvider dataProvider);
    }

    public interface ISettingsBuilderAddMoreCollections
    {
        ISettingsBuilderAddMoreCollections AddCollection(string name, bool dropFirst, string fileLocation);

        ISettingsBuilderAddMoreCollections AddCollection<T>(string name, bool dropFirst, IDataProvider dataProvider);

        ISettingsBuilderConnectionString AddDatabase(string name);

        HarnessConfiguration Build();
    }
}
