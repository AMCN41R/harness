using System;
using System.Collections.Generic;
using Harness.Attributes;
using MongoDB.Driver;

namespace Harness
{
    public abstract class HarnessBase
    {
        /// <summary>
        /// Gets or sets the filepath of the mongo configuration file.
        /// </summary>
        private string ConfigFilepath { get; set; }

        /// <summary>
        /// Gets or sets whether the database build should happen automatically.
        /// </summary>
        private bool AutoRun { get; set; }

        /// <summary>
        /// Gets the <see cref="IHarnessManager"/> implementation.
        /// </summary>
        private IHarnessManager HarnessManager { get; }

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

            this.GetAttributeSettings();

            if (this.AutoRun)
            {
                this.BuildDatabase();
            }
        }
        
        private void GetAttributeSettings()
        {
            var configFilePath = string.Empty;
            var autoRun = true;

            // Look for the HarnessConfigAttribute attribute
            var mongoTestClassAttribute =
               this.GetType().GetAttribute<HarnessConfigAttribute>();

            if (mongoTestClassAttribute != null)
            {
                // If the attribute is found, get the path to the config file, 
                // or set to default based on class name if not specified.
                configFilePath = mongoTestClassAttribute.ConfigFilePath;

                // Get the AutoRun value
                autoRun = mongoTestClassAttribute.AutoRun;
            }

            // Set the filepath to the default value if the attribute is not 
            // present, or if the value is an empty string.
            if (string.IsNullOrWhiteSpace(configFilePath))
            {
                configFilePath = $"{this.GetType().Name}.json";
            }

            this.ConfigFilepath = configFilePath;
            this.AutoRun = autoRun;
        }



        /// <summary>
        /// Gets or sets the dictionary of mongo clients. The key is the 
        /// mongo server connection string.
        /// </summary>
        public Dictionary<string, IMongoClient> MongoConnections { get; private set; }

        /// <summary>
        /// Puts the databases into the state defined in the given configuration file.
        /// This method can only be called if the <see cref="HarnessConfigAttribute"/>
        /// AutoRun property is set to false.
        /// </summary>
        public void BuildDatabase()
        {
            if (!AutoRun)
            {
                throw new HarnessBaseException(
                    "The current instance is not configured to allow a manual build."
                );
            }

            this.MongoConnections =
                this.HarnessManager
                    .UsingSettings(this.ConfigFilepath)
                    .Build();
        }


    }
}
