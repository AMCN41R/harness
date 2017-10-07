using System.Collections.Generic;
using Harness.Settings;
using MongoDB.Driver;

namespace Harness
{
    /// <summary>
    /// <para>
    /// The <see cref="HarnessManager"/> class, alongside the <see cref="SettingsBuilder"/>
    /// class, is used to configure and build one or more mongo databases, putting them
    /// into a known state.
    /// </para>
    /// <para>
    /// Once you have your <see cref="HarnessConfiguration"/> settings object, you can 
    /// pass it to the <see cref="HarnessManager"/> and use build your databases. The 
    /// <see cref="IHarnessManagerBuilder.Build"/> method returns a 
    /// <see cref="Dictionary{String, IMongoClient}"/> containing the MongoClient instances 
    /// (one per unique connection string in the <see cref="HarnessConfiguration"/>) for re-use. 
    /// The dictionary key is the connection string.
    /// </para>
    /// </summary>
    /// <example>
    /// <code lang="C#">
    /// var mongoClients =
    ///     new HarnessManager()
    ///         .UsingSettings(settings) // the settings object built using the SettingsBuilder
    ///         .Build();
    /// </code>
    /// <para>
    /// The following is an example using XUnit. Here, we call build() in the test class constructor 
    /// which means the configuration will run before each test in the class is executed...
    /// </para>
    /// <code lang="C#">
    /// public class SettingsBuilderWithDataFiles
    /// {
    ///     public SettingsBuilderWithDataFiles()
    ///     {
    ///         var settings =
    ///             new SettingsBuilder()
    ///                 .AddDatabase("TestDb1")
    ///                 .WithConnectionString("mongodb://localhost:27017")
    ///                 .DropDatabaseFirst()
    ///                 .AddCollection("col1", true, "path/to/Collection1.json")
    ///                 .AddCollection("col2", true, "path/to/Collection2.json")
    ///                 .Build();
    /// 
    ///         this.MongoConnections =
    ///             new HarnessManager()
    ///                 .UsingSettings(settings)
    ///                 .Build();
    ///     }
    /// 
    ///     private Dictionary&lt;string, IMongoClient&gt; MongoConnections { get; }
    /// 
    ///     [Fact]
    ///     public void Test1()
    ///     {
    ///         // Arrange
    ///         var classUnderTest = new ClassUnderTest();
    /// 
    ///         // Act
    ///         var result = classUnderTest.GetCollectionRecordCount("col1");
    /// 
    ///         // Assert
    ///         Assert.Equal(2, result);
    ///     }
    /// 
    ///     [Fact]
    ///     public void Test2()
    ///     {
    ///         // Arrange
    ///         var classUnderTest = new ClassUnderTest();
    ///         var mongoClient = this.MongoConnections["mongodb://localhost:27017"];
    /// 
    ///         // Act
    ///         var result = classUnderTest.GetCollectionRecordCount(mongoClient, "col1");
    /// 
    ///         // Assert
    ///         Assert.Equal(2, result);
    ///     }
    /// }
    /// </code>
    /// </example>
    public class HarnessManager : IHarnessManager, IHarnessManagerBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HarnessManager"/> class.
        /// </summary>
        public HarnessManager() : this(new SettingsLoader())
        {
        }

        /// <summary>
        /// Internal constructor to allow dependency injection for unit testing.
        /// </summary>
        internal HarnessManager(ISettingsLoader settingsLoader)
        {
            this.SettingsLoader = settingsLoader;
        }

        /// <summary>
        /// Gets or sets the <see cref="HarnessConfiguration"/>.
        /// </summary>
        private HarnessConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets the <see cref="ISettingsLoader"/> instance.
        /// </summary>
        private ISettingsLoader SettingsLoader { get; }

        /// <inheritdoc />
        public IHarnessManagerBuilder UsingSettings(string filepath)
        {
            Guard.AgainstNullEmptyOrWhitespace(filepath, nameof(filepath));

            this.Configuration =
                this.SettingsLoader
                    .GetMongoConfiguration(filepath);

            return this;
        }

        /// <inheritdoc />
        public IHarnessManagerBuilder UsingSettings(HarnessConfiguration configuration)
        {
            Guard.AgainstNullArgument(configuration, nameof(configuration));

            this.Configuration = configuration;
            return this;
        }

        /// <inheritdoc />
        Dictionary<string, IMongoClient> IHarnessManagerBuilder.Build()
            => this.MongoSessionManager().Build();


        // TODO: Replace with injected IMongoSessionManagerFactory
        /// <summary>
        /// Internal factory method to return live implementation of 
        /// IMongoSessionManager that can be overridden and mocked for 
        /// unit testing.
        /// </summary>
        internal virtual IMongoSessionManager MongoSessionManager()
            => new MongoSessionManager(this.Configuration);
    }

    /// <summary>
    /// Harness Manager API. See <see cref="HarnessManager"/>.
    /// </summary>
    public interface IHarnessManager
    {
        /// <summary>
        /// The <see cref="HarnessConfiguration"/> to use.
        /// </summary>
        /// <param name="filepath">The path of the file to load as the <see cref="HarnessConfiguration"/>.</param>
        /// <returns>
        /// The harness manager instance as an <see cref="IHarnessManagerBuilder"/>.
        /// </returns>
        IHarnessManagerBuilder UsingSettings(string filepath);

        /// <summary>
        /// The <see cref="HarnessConfiguration"/> to use.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>
        /// The harness manager instance as an <see cref="IHarnessManagerBuilder"/>.
        /// </returns>
        IHarnessManagerBuilder UsingSettings(HarnessConfiguration configuration);
    }

    /// <summary>
    /// Harness Manager API. See <see cref="HarnessManager"/>.
    /// </summary>
    public interface IHarnessManagerBuilder
    {
        /// <summary>
        /// Builds an instance on <see cref="HarnessManager"/> using the 
        /// options applied in the current instance of the <see cref="HarnessManager"/>.
        /// </summary>
        /// <returns>
        /// The configured <see cref="HarnessManager"/> instance.
        /// </returns>
        Dictionary<string, IMongoClient> Build();
    }
}
