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

        internal HarnessManager(ISettingsLoader settingsLoader)
        {
            this.SettingsLoader = settingsLoader;
        }

        private MongoConfiguration Configuration { get; set; }

        private ISettingsLoader SettingsLoader { get; }

        public IHarnessManagerBuilder UsingSettings(string filepath)
        {
            this.Configuration =
                this.SettingsLoader
                    .GetMongoConfiguration(filepath);

            return this;
        }

        public IHarnessManagerBuilder UsingSettings(MongoConfiguration configuration)
        {
            this.Configuration = configuration;
            return this;
        }

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

        IHarnessManagerBuilder UsingSettings(MongoConfiguration configuration);
    }

    public interface IHarnessManagerBuilder
    {
        Dictionary<string, IMongoClient> Build();
    }
}
