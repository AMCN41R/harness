using System.Linq;

namespace Harness.Settings
{
    /// <summary>
    /// A fluent api that lets you build the Harness configuration object in 
    /// code instead of using an external json file.
    /// </summary>
    public class SettingsBuilder :
        ISettingsBuilder,
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
            this.Config.AddDatabase(name);
            this.CurrentDatabaseName = name;
            return this;
        }

        /// <inheritdoc />
        HarnessConfiguration ISettingsBuilder.Build()
        {
            return this.Config;
        }

        /// <inheritdoc />
        ISettingsBuilderDatabaseOptions ISettingsBuilderConnectionString.WithConnectionString(string connectionString)
        {
            Guard.AgainstNullEmptyOrWhitespace(connectionString, nameof(connectionString));

            this.GetDatabaseConfig().SetValue(x => x.ConnectionString = connectionString);

            return this;
        }

        /// <inheritdoc />
        ISettingsBuilderDatabaseOptions ISettingsBuilderDatabaseOptions.WithDatabaseNameSuffix(string suffix)
        {
            Guard.AgainstNullEmptyOrWhitespace(suffix, nameof(suffix));

            this.GetDatabaseConfig().SetValue(x => x.DatabaseNameSuffix = suffix);

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
            this.GetDatabaseConfig().AddCollection(name, dropFirst, fileLocation);
            return this;
        }

        /// <inheritdoc />
        ISettingsBuilderAddMoreCollections ISettingsBuilderAddMoreCollections.AddCollection(string name, bool dropFirst, string fileLocation)
        {
            this.GetDatabaseConfig().AddCollection(name, dropFirst, fileLocation);
            return this;
        }

        /// <inheritdoc />
        ISettingsBuilderConnectionString ISettingsBuilderAddMoreCollections.AddAnotherDatabase(string name)
        {
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
            this.GetDatabaseConfig().AddDataProviderCollection<T>(name, dropFirst, dataProvider);
            return this;
        }

        /// <inheritdoc />
        ISettingsBuilderAddMoreCollections ISettingsBuilderAddMoreCollections.AddCollection<T>(string name, bool dropFirst, IDataProvider dataProvider)
        {
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
        /// Adds a new database configuration to the <see cref="HarnessConfiguration"/> object. 
        /// </summary>
        /// <param name="name">The name of the MongoDb database.</param>
        /// <returns>
        /// An <see cref="ISettingsBuilderConnectionString"/> instance.
        /// </returns>
        ISettingsBuilderConnectionString AddDatabase(string name);

        /// <summary>
        /// Builds the finshed configuration.
        /// </summary>
        /// <returns>A <see cref="HarnessConfiguration"/> instance.</returns>
        HarnessConfiguration Build();
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
        ISettingsBuilderDatabaseOptions WithDatabaseNameSuffix(string suffix);

        ISettingsBuilderDatabaseOptions WithCollectionNameSuffix(string suffix);

        ISettingsBuilderDatabaseOptions DropDatabaseFirst();

        ISettingsBuilderAddMoreCollections AddCollection(string name, bool dropFirst, string fileLocation);

        ISettingsBuilderAddMoreCollections AddCollection<T>(string name, bool dropFirst, IDataProvider dataProvider);
    }

    public interface ISettingsBuilderAddMoreCollections
    {
        ISettingsBuilderAddMoreCollections AddCollection(string name, bool dropFirst, string fileLocation);

        ISettingsBuilderAddMoreCollections AddCollection<T>(string name, bool dropFirst, IDataProvider dataProvider);

        ISettingsBuilderConnectionString AddAnotherDatabase(string name);

        HarnessConfiguration Build();
    }
}
