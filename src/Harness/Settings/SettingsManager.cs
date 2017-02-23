using System;
using System.IO.Abstractions;
using System.Web.Script.Serialization;

namespace Harness.Settings
{
    internal class SettingsManager : ISettingsManager
    {
        /// <summary>
        /// Main entry point that constructs a new instance of the 
        /// SetttingsManager class with the default <see cref="IFileSystem"/> 
        /// implementation.
        /// </summary>
        public SettingsManager() : this(new FileSystem())
        {
        }

        /// <summary>
        /// Internal constructor to allow dependancy injection for unit testing.
        /// </summary>
        internal SettingsManager(IFileSystem fileSystem)
        {
            this.FileSystem = fileSystem;
        }

        /// <summary>
        ///  Gets the <see cref="IFileSystem"/> implementation.
        /// </summary>
        private IFileSystem FileSystem { get; }


        /// <inheritdoc />
        public MongoConfiguration GetMongoConfiguration(string configFilePath)
        {
            var fileValidationResult =
                this.FileSystem.ValidateFile(configFilePath);

            if (!fileValidationResult?.IsValid ?? false)
            {
                throw new SettingsManagerException(
                    fileValidationResult?.Message ?? "Unable to validate filepath");
            }

            if (this.FileSystem.Path.GetExtension(configFilePath) != ".json")
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
