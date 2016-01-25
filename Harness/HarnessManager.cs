using System.Collections.Generic;
using Harness.Settings;
using MongoDB.Driver;

namespace Harness
{
    public class HarnessManager : IHarnessManager
    {
        private MongoConfiguration Configuration { get; set; }

        private ISettingsManager SettingsManager { get; }

        public HarnessManager() : this(new SettingsManager())
        {
        }

        internal HarnessManager(ISettingsManager settingsManager)
        {
            this.SettingsManager = settingsManager;
        }

        public IHarnessManager Using(string filepath)
        {
            this.Configuration =
                this.SettingsManager
                    .GetMongoConfiguration(filepath);

            return this;
        }

        public IHarnessManager Using(MongoConfiguration configuration)
        {
            this.Configuration = configuration;
            return this;
        }

        public Dictionary<string, IMongoClient> Build()
            => this.MongoSessionManager().Build();


        /// <summary>
        /// Internal factory method to return live implementation of IMongoSessionManager
        /// that can be overrdden and mocked for unit testing.
        /// </summary>
        internal virtual IMongoSessionManager MongoSessionManager()
            => new MongoSessionManager(this.Configuration);

    }
}
