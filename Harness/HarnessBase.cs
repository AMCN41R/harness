using System;
using System.Collections.Generic;
using Harness.Attributes;
using MongoDB.Driver;

namespace Harness
{
    public abstract class HarnessBase : IDisposable
    {

        private string ConfigFilepath { get; }

        private IHarnessManager HarnessManager { get; }

        private Dictionary<string, IMongoClient> Connections { get; set; }

        protected Dictionary<string, IMongoClient> MongoConnections => this.Connections;

        /// <summary>
        /// Constructs a new instance of the MongoIntegrationBase class.
        /// This base class expects the inheriting class to have the 
        /// <see cref="HarnessConfigAttribute"/> attribute. During 
        /// construction, it will attempt to load a Mongo configuration 
        /// settings file and put any specified databases in the required 
        /// state. If the attribute is not present, or a configuration filepath 
        /// is not specified on the atribute, a default value of 
        /// [ClassName].json will be used.
        /// </summary>
        protected HarnessBase() : this(new HarnessManager())
        {
        }

        /// <summary>
        /// Internal constructor to allow <see cref="IHarnessManager"/> 
        /// injection for testing.
        /// </summary>
        internal HarnessBase(IHarnessManager harnessManager)
        {
            if (harnessManager == null)
            {
                throw new ArgumentNullException(nameof(harnessManager));
            }

            this.HarnessManager = harnessManager;

            this.ConfigFilepath = this.GetConfigFilepath();

            this.ConfigureMongo();
        }

        private string GetConfigFilepath()
        {
            var configFilePath = string.Empty;

            // Look for the HarnessConfigAttribute attribute
            var mongoTestClassAttribute =
               this.GetType().GetAttribute<HarnessConfigAttribute>();

            if (mongoTestClassAttribute != null)
            {
                // If the attribute is found, get the path to the config file, 
                // or set to default based on class name if not specified.
                configFilePath = mongoTestClassAttribute.ConfigFilePath;
            }

            // Set the filepath to the default value if the attribute is not 
            // present, or if the value is an empty string.
            if (string.IsNullOrWhiteSpace(configFilePath))
            {
                configFilePath = $"{this.GetType().Name}.json";
            }

            return configFilePath;
        }

        private void ConfigureMongo()
        {
            this.Connections =
                this.HarnessManager
                    .UsingSettings(this.ConfigFilepath)
                    .Build();
        }

        public void Dispose()
        {
            // TODO: Dispose the mongo connections
        }
    }
}
