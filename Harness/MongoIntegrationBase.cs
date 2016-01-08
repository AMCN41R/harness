using Harness.Attributes;
using Harness.FileSystem;
using Harness.Settings;
using MongoDbUnit.Settings;

namespace Harness
{
    public abstract class MongoIntegrationBase
    {
        internal IFileSystemHelper FileSystem { get; set; }

        private IntegrationTestSettings Settings { get; set; }

        protected MongoIntegrationBase()
        {
            this.FileSystem = new FileSystemHelper();


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

        }

        private void LoadSettings(string configFilePath)
        {
            var settingsManager = new SettingsManager(this.FileSystem);
            this.Settings = settingsManager.GetSettings(configFilePath);
        }

    }
}
