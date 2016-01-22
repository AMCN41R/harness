using System;
using Harness.Attributes;
using Harness.Settings;

namespace Harness
{
    public abstract class MongoIntegrationBase : IDisposable
    {

        private MongoConfiguration Settings { get; set; }

        private ISettingsManager SettingsManager { get; }

        /// <summary>
        /// Constructs a new instance of the MongoIntegrationBase class.
        /// This base class expects the inheriting class to have the 
        /// <see cref="MongoIntegrationTestClassAttribute"/> attribute. During 
        /// construction, it will attempt to load a Mongo configuration 
        /// settings file and put any specified databases in the required 
        /// state. If the attribute is not present, or a configuration filepath 
        /// is not specified on the atribute, a default value of 
        /// [ClassName].json will be used.
        /// </summary>
        protected MongoIntegrationBase() : this(new SettingsManager())
        {
        }

        /// <summary>
        /// Internal constructor to allow <see cref="ISettingsManager"/> 
        /// injection for testing.
        /// </summary>
        /// <param name="settingsManager"></param>
        internal MongoIntegrationBase(ISettingsManager settingsManager)
        {
            this.SettingsManager = settingsManager;

            var configFilePath = string.Empty;

            // Look for the MongoIntegrationTestClass attribute
            var mongoTestClassAttribute =
               this.GetType().GetAttribute<MongoIntegrationTestClassAttribute>();

            if (mongoTestClassAttribute != null)
            {
                // If the attribute is found, get the path to the config file, 
                // or set to default based on class name if not specified.
                configFilePath = mongoTestClassAttribute.ConfigFilePath;
            }

            // Set the filepath to the default value if the attribute is not 
            // present, or if the value is an empty string.
            if (string.IsNullOrEmpty(configFilePath))
            {
                configFilePath = $"{this.GetType().Name}.json";
            }


            // Load the mongo settings from the config file
            LoadSettings(configFilePath);


            this.ConfigureMongo();

        }

        /// <summary>
        /// Attempts to load mog coniguration settings from the gicen filepath.
        /// </summary>
        /// <param name="configFilePath">
        /// The path to the Mongo configuration settings file.
        /// </param>
        private void LoadSettings(string configFilePath)
        {
            this.Settings =
                this.SettingsManager
                    .GetMongoConfiguration(configFilePath);
        }

        private void ConfigureMongo()
        {
            this.MongoSessionManager().Build();
        }

        /// <summary>
        /// Internal factory method to return live implementation of IMongoSessionManager
        /// that can be overrdden and mocked for unit testing.
        /// </summary>
        internal virtual IMongoSessionManager MongoSessionManager()
            => new MongoSessionManager(this.Settings);

        public void Dispose()
        {
            if (this.Settings.SaveOutput)
            {
                this.MongoSessionManager().SaveOutput();
                this.MongoSessionManager().Dispose();
            }
        }
    }
}
