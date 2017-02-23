using System.Collections.Generic;
using Harness.Settings;
using MongoDB.Driver;

namespace Harness
{
    internal class HarnessManager : IHarnessManager
    {
        internal HarnessManager() : this(new SettingsManager())
        {
        }

        internal HarnessManager(ISettingsManager settingsManager)
        {
            this.SettingsManager = settingsManager;
        }

        private MongoConfiguration Configuration { get; set; }

        private ISettingsManager SettingsManager { get; }

        public IHarnessManager UsingSettings(string filepath)
        {
            this.Configuration =
                this.SettingsManager
                    .GetMongoConfiguration(filepath);

            return this;
        }

        public IHarnessManager UsingSettings(MongoConfiguration configuration)
        {
            this.Configuration = configuration;
            return this;
        }

        public Dictionary<string, IMongoClient> Build()
            => this.MongoSessionManager().Build();


        /// <summary>
        /// Internal factory method to return live implementation of 
        /// IMongoSessionManager that can be overridden and mocked for 
        /// unit testing.
        /// </summary>
        internal virtual IMongoSessionManager MongoSessionManager()
            => new MongoSessionManager(this.Configuration);

    }
}
