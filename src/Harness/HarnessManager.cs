using System.Collections.Generic;
using Harness.Settings;
using MongoDB.Driver;

namespace Harness
{
    public class HarnessManager : IHarnessManager, IHarnessManagerBuilder
    {
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


        /// <summary>
        /// Internal factory method to return live implementation of 
        /// IMongoSessionManager that can be overridden and mocked for 
        /// unit testing.
        /// </summary>
        internal virtual IMongoSessionManager MongoSessionManager()
            => new MongoSessionManager(this.Configuration);
    }

    public interface IHarnessManager
    {
        IHarnessManagerBuilder UsingSettings(string filepath);

        IHarnessManagerBuilder UsingSettings(HarnessConfiguration configuration);
    }

    public interface IHarnessManagerBuilder
    {
        Dictionary<string, IMongoClient> Build();
    }
}
