using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace Harness
{
    /// <summary>
    /// A base class that can be used in conjunction with the <see cref="HarnessConfigAttribute"/>
    /// to configure one or more Mongo databases.
    /// </summary>
    /// <remarks>
    /// This class expects the class that extends it to have the <see cref="HarnessConfigAttribute"/> attribute.
    /// During construction, it will attempt to load a configuration file, and 
    /// put any specified databases into the required state.
    /// If the attribute is not present, or there is no valid filepath for the
    /// configuration file, a default value of [ClassName].json will be used.
    /// </remarks>
    /// <example>
    /// <code lang="C#">
    /// [HarnessConfig(ConfigFilePath = "HarnessConfig.json")]
    /// public class MyMongoIntegrationTests : HarnessBase
    /// {
    ///     [Fact]
    ///     public void Test()
    ///     {
    ///         // Test something...
    ///     }
    /// }
    /// </code>
    /// <code lang="C#">
    /// [HarnessConfig(ConfigFilePath = "HarnessConfig.json", AutoRun = false)]
    /// public class MyMongoIntegrationTests : HarnessBase
    /// {
    ///     [Fact]
    ///     public void Test()
    ///     {
    ///         base.Build();
    /// 
    ///         // Test something...
    ///     }
    /// }
    /// </code>
    /// </example>
    public abstract class HarnessBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HarnessBase"/> class.
        /// </summary>
        /// <remarks>
        /// During construction, it will attempt to load a configuration file, and 
        /// put any specified databases into the required state.
        /// If the attribute is not present, or there is no valid filepath for the
        /// configuration file, a default value of [ClassName].json will be used.
        /// </remarks>
        protected HarnessBase() : this(new HarnessManager())
        {
        }

        /// <summary>
        /// Internal constructor to allow <see cref="IHarnessManager"/> 
        /// injection for testing.
        /// </summary>
        internal HarnessBase(IHarnessManager harnessManager)
        {
            Guard.AgainstNullArgument(harnessManager, nameof(harnessManager));

            this.HarnessManager = harnessManager;

            this.GetAttributeSettings();

            if (this.AutoRun)
            {
                this.BuildDatabases();
            }
        }

        /// <summary>
        /// Gets the dictionary of mongo clients, whose key is the mongo server connection string.
        /// </summary>
        public Dictionary<string, IMongoClient> MongoConnections => this.Connections;

        /// <summary>
        /// Backing property for <see cref="MongoConnections"/>.
        /// </summary>
        private Dictionary<string, IMongoClient> Connections { get; set; }

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
        /// Puts the databases into the state defined in the given configuration file.
        /// This method can only be called if the <see cref="HarnessConfigAttribute"/>
        /// AutoRun property is set to false.
        /// </summary>
        /// <example>
        /// <code lang="C#">
        /// [HarnessConfig(ConfigFilePath = "HarnessConfig.json", AutoRun = false)]
        /// public class MyMongoIntegrationTests : HarnessBase
        /// {
        ///     [Fact]
        ///     public void Test()
        ///     {
        ///         base.Build();
        /// 
        ///         // Test something...
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <exception cref="InvalidOperationException">Thrown if the method is called when the <see cref="AutoRun"/> property is set to true.</exception>
        public void Build()
        {
            if (this.AutoRun)
            {
                throw new InvalidOperationException(
                    "The current instance is not configured to allow a manual build."
                );
            }

            this.BuildDatabases();
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

        private void BuildDatabases()
        {
            this.Connections =
                this.HarnessManager
                    .UsingSettings(this.ConfigFilepath)
                    .Build();
        }
    }
}
