using Harness.Attributes;
using Harness.Settings;

namespace Harness
{
    public abstract class MongoIntegrationBase
    {

        private MongoConfiguration Settings { get; set; }

        private ISettingsManager SettingsManager { get; }

        protected MongoIntegrationBase() : this(new SettingsManager())
        {
        }

        internal MongoIntegrationBase(ISettingsManager settingsManager)
        {
            this.SettingsManager = settingsManager;

            // Look for the MongoIntegrationTestClass attribute
            var mongoTestClassAttribute =
               this.GetType().GetAttribute<MongoIntegrationTestClass>();

            if (mongoTestClassAttribute == null)
            {
                throw new RequiredAttributeNotFoundException("Insert some helpful message here");
            }


            // If the attribute is found, get the path to the config file, 
            // or set to default based on class name if not specified.
            var configFilePath = mongoTestClassAttribute.ConfigFilePath;

            if (string.IsNullOrEmpty(configFilePath))
            {
                configFilePath = $"{this.GetType().Name}.json";
            }


            // Load the mongo settings from the config file
            LoadSettings(configFilePath);


            this.ConfigureMongo();

        }

        private void LoadSettings(string configFilePath)
        {
            var settingsManager = this.SettingsManager;
            this.Settings = settingsManager.GetMongoConfiguration(configFilePath);
        }

        private void ConfigureMongo()
        {
            MongoSessionManager().Build();
        }

        /// <summary>
        /// Factory method to return live implementation of IMongoSessionManager
        /// that can be overrdden and mocked for unit testing.
        /// </summary>
        internal virtual IMongoSessionManager MongoSessionManager() 
            => new MongoSessionManager(this.Settings);
    }
}
