using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson.Serialization.Conventions;

namespace Harness.Settings
{
    /// <summary>
    /// A fluent api that lets you build the Harness configuration object in 
    /// code instead of using an external json file.
    /// </summary>
    /// <example>
    /// <para>Example using json files</para>
    /// <para>This example creates a configuration for one database called 'TestDb1', that has two collections...</para>
    /// <code lang="C#">
    /// var settings =
    ///     new SettingsBuilder()
    ///         .AddDatabase("TestDb1")
    ///         .WithConnectionString("mongodb://localhost:27017")
    ///         .DropDatabaseFirst()
    ///         .AddCollection("col1", true, "path/to/Collection1.json")
    ///         .AddCollection("col2", true, "path/to/Collection2.json")
    ///         .Build();
    /// </code>
    /// <para>Example using IDataProvider</para>
    /// <para>This example creates a configuration for a database called 'TestDb2', that has one collection...</para>
    /// <code lang="C#">
    /// var settings =
    ///     new SettingsBuilder()
    ///         .AddDatabase("TestDb2")
    ///         .WithConnectionString("mongodb://localhost:27017")
    ///         .DropDatabaseFirst()
    ///         .AddCollection&lt;Person&gt;("people", true, new PersonDataProvider())
    ///         .Build();
    /// </code>
    /// </example>
    public class SettingsBuilder :
        ISettingsBuilder,
        ISettingsBuilderDatabase,
        ISettingsBuilderConnectionString,
        ISettingsBuilderDatabaseOptions,
        ISettingsBuilderAddMoreCollections
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsBuilder"/> class.
        /// </summary>
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

    /// <summary>
    /// Settings Builder API. See <see cref="SettingsBuilder"/>.
    /// </summary>
    public interface ISettingsBuilder
    {
        /// <summary>
        /// Adds a convention to the Mongo ConventionRegistry.
        /// </summary>
        /// <param name="convention">The convention to add.</param>
        /// <param name="filter">
        /// The filter to select the types to which the convention will be applied.
        /// </param>
        /// <returns>
        /// The settings builder instance as an <see cref="ISettingsBuilderDatabase"/>.
        /// </returns>
        ISettingsBuilderDatabase AddConvention(IConvention convention, Expression<Func<Type, bool>> filter);

        /// <summary>
        /// Adds conventions to the Mongo ConventionRegistry.
        /// </summary>
        /// <param name="conventions">The conventions to add.</param>
        /// <param name="filter">
        /// The filter to select the types to which the convention will be applied.
        /// </param>
        /// <returns>
        /// The settings builder instance as an <see cref="ISettingsBuilderDatabase"/>.
        /// </returns>
        ISettingsBuilderDatabase AddConvention(IList<IConvention> conventions, Expression<Func<Type, bool>> filter);

        /// <summary>
        /// Adds a conventions to the Mongo ConventionRegistry.
        /// </summary>
        /// <param name="conventions">The convention to add.</param>
        /// <param name="filter">
        /// The filter to select the types to which the convention will be applied.
        /// </param>
        /// <returns>
        /// The settings builder instance as an <see cref="ISettingsBuilderDatabase"/>.
        /// </returns>
        ISettingsBuilderDatabase AddConvention(IConventionPack conventions, Expression<Func<Type, bool>> filter);

        /// <summary>
        /// Adds a new database configuration to the <see cref="HarnessConfiguration"/> object. 
        /// </summary>
        /// <param name="name">The name of the MongoDb database.</param>
        /// <returns>
        /// The settings builder instance as an <see cref="ISettingsBuilderConnectionString"/>.
        /// </returns>
        ISettingsBuilderConnectionString AddDatabase(string name);
    }

    /// <summary>
    /// Settings Builder API. See <see cref="SettingsBuilder"/>.
    /// </summary>
    public interface ISettingsBuilderDatabase
    {
        /// <summary>
        /// Adds a new database configuration to the <see cref="HarnessConfiguration"/> object. 
        /// </summary>
        /// <param name="name">The name of the MongoDb database.</param>
        /// <returns>
        /// The settings builder instance as an <see cref="ISettingsBuilderConnectionString"/>.
        /// </returns>
        ISettingsBuilderConnectionString AddDatabase(string name);
    }

    /// <summary>
    /// Settings Builder API. See <see cref="SettingsBuilder"/>.
    /// </summary>
    public interface ISettingsBuilderConnectionString
    {
        /// <summary>
        /// Adds the connection string to the current database configuration.
        /// </summary>
        /// <param name="connectionString">The MongoDb connection string.</param>
        /// <returns>
        /// The settings builder instance as an <see cref="ISettingsBuilderDatabaseOptions"/>.
        /// </returns>
        ISettingsBuilderDatabaseOptions WithConnectionString(string connectionString);
    }

    /// <summary>
    /// Settings Builder API. See <see cref="SettingsBuilder"/>.
    /// </summary>
    public interface ISettingsBuilderDatabaseOptions
    {
        /// <summary>
        /// Sets a string that will be added to the end of each collection name.
        /// </summary>
        /// <param name="suffix">The suffix.</param>
        /// <returns>
        /// The settings builder instance as an <see cref="ISettingsBuilderDatabaseOptions"/>.
        /// </returns>
        ISettingsBuilderDatabaseOptions WithCollectionNameSuffix(string suffix);

        /// <summary>
        /// Indicates that, if the database already exists, it should be dropped first.
        /// </summary>
        /// <returns>
        /// The settings builder instance as an <see cref="ISettingsBuilderDatabaseOptions"/>.
        /// </returns>
        ISettingsBuilderDatabaseOptions DropDatabaseFirst();

        /// <summary>
        /// Configures a collection to be added to the builder's current database.
        /// </summary>
        /// <param name="name">The name of the collection.</param>
        /// <param name="dropFirst">Whether or not the collection should be dropped first.</param>
        /// <param name="fileLocation">The filepath of the json data file that should be used to populate the collection.</param>
        /// <returns>
        /// The settings builder instance as an <see cref="ISettingsBuilderAddMoreCollections"/>.
        /// </returns>
        ISettingsBuilderAddMoreCollections AddCollection(string name, bool dropFirst, string fileLocation);

        /// <summary>
        /// Configures a collection to be added to the builder's current database.
        /// </summary>
        /// <typeparam name="T">The type of object that the instance of <see cref="IDataProvider"/> will return.</typeparam>
        /// <param name="name">The name of the collection.</param>
        /// <param name="dropFirst">Whether or not the collection should be dropped first.</param>
        /// <param name="dataProvider">The data provider that should be used to populate the collection.</param>
        /// <returns>
        /// The settings builder instance as an <see cref="ISettingsBuilderAddMoreCollections"/>.
        /// </returns>
        ISettingsBuilderAddMoreCollections AddCollection<T>(string name, bool dropFirst, IDataProvider dataProvider);
    }

    /// <summary>
    /// Settings Builder API. See <see cref="SettingsBuilder"/>.
    /// </summary>
    public interface ISettingsBuilderAddMoreCollections
    {
        /// <summary>
        /// Configures a collection to be added to the builder's current database.
        /// </summary>
        /// <param name="name">The name of the collection.</param>
        /// <param name="dropFirst">Whether or not the collection should be dropped first.</param>
        /// <param name="fileLocation">The filepath of the json data file that should be used to populate the collection.</param>
        /// <returns>
        /// The settings builder instance as an <see cref="ISettingsBuilderAddMoreCollections"/>.
        /// </returns>
        ISettingsBuilderAddMoreCollections AddCollection(string name, bool dropFirst, string fileLocation);

        /// <summary>
        /// Configures a collection to be added to the builder's current database.
        /// </summary>
        /// <typeparam name="T">The type of object that the instance of <see cref="IDataProvider"/> will return.</typeparam>
        /// <param name="name">The name of the collection.</param>
        /// <param name="dropFirst">Whether or not the collection should be dropped first.</param>
        /// <param name="dataProvider">The data provider that should be used to populate the collection.</param>
        /// <returns>
        /// The settings builder instance as an <see cref="ISettingsBuilderAddMoreCollections"/>.
        /// </returns>
        ISettingsBuilderAddMoreCollections AddCollection<T>(string name, bool dropFirst, IDataProvider dataProvider);

        /// <summary>
        /// Adds a new database configuration to the <see cref="HarnessConfiguration"/> object. 
        /// </summary>
        /// <param name="name">The name of the MongoDb database.</param>
        /// <returns>
        /// The settings builder instance as an <see cref="ISettingsBuilderConnectionString"/>.
        /// </returns>
        ISettingsBuilderConnectionString AddDatabase(string name);

        /// <summary>
        /// Builds an instance on <see cref="HarnessConfiguration"/> using the 
        /// options applied in the current instance of the <see cref="SettingsBuilder"/>.
        /// </summary>
        /// <returns>
        /// The completed <see cref="HarnessConfiguration"/> object.
        /// </returns>
        HarnessConfiguration Build();
    }
}
