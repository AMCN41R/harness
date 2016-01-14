using System;
using System.IO.Abstractions;
using System.Web.Script.Serialization;

namespace Harness.Settings
{
    internal class SettingsManager : ISettingsManager
    {
        /// <summary>
        ///  The file system implementation to use.
        /// </summary>
        private readonly IFileSystem FileSystem;

        /// <summary>
        /// Main entry point that constructs a new instance of the 
        /// SetttingsManager class with the default <see cref="IFileSystem"/> 
        /// implementation.
        /// </summary>
        public SettingsManager() : this(new FileSystem())
        {
        }

        /// <summary>
        /// Additional constructer to allow injection of alternative 
        /// <see cref="IFileSystem"/> implementation. Designed for unit test
        /// dependancy injection.
        /// </summary>
        /// <param name="fileSystem"></param>
        public SettingsManager(IFileSystem fileSystem)
        {
            this.FileSystem = fileSystem;
        }

        /// <summary>
        /// Loads and deserializes the given JSON file to a new instance of 
        /// <see cref="MongoConfiguration"/>.
        /// </summary>
        /// <param name="configFilePath">The file JSON path to load.</param>
        /// <returns>
        /// A new instance of <see cref="MongoConfiguration"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the iven filepath's extension is not .json.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the contents loaded from the given filepath cannot be 
        /// deserialized.
        /// </exception>
        public MongoConfiguration GetMongoConfiguration(string configFilePath)
        {
            if (!this.FileSystem.ValidateFile(configFilePath))
            {
                return null;
            }
            
            if (this.FileSystem.Path.GetExtension(configFilePath) != "json")
            {
                throw new ArgumentOutOfRangeException(
                    nameof(configFilePath),
                    "Invalid file type. File must be a .json file.");
            }

            var json =
                this.FileSystem.File.ReadAllText(configFilePath);

            return
                new JavaScriptSerializer()
                    .Deserialize<MongoConfiguration>(json);

        }

    }
}
