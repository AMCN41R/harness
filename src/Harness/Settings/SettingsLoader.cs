using System;
using System.IO.Abstractions;
using Newtonsoft.Json;

namespace Harness.Settings
{
    internal interface ISettingsLoader
    {
        /// <summary>
        /// Loads a JSON file using the given path and deserializes it to a new 
        /// instance of <see cref="HarnessConfiguration"/>.
        /// </summary>
        /// <param name="configFilePath">The path of the JSON file to load.</param>
        /// <returns>A new instance of <see cref="HarnessConfiguration"/>.</returns>
        /// <exception cref="SettingsLoaderException">Thrown if the given filepath is invalid.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the given filepath's extension is not .json.</exception>
        /// <exception cref="ArgumentException">Thrown when the contents loaded from the given filepath cannot be deserialized.</exception>
        HarnessConfiguration GetMongoConfiguration(string configFilePath);
    }

    internal class SettingsLoader : ISettingsLoader
    {
        /// <summary>
        /// Main entry point that constructs a new instance of the 
        /// SetttingsLoader class with the default <see cref="IFileSystem"/> 
        /// implementation.
        /// </summary>
        public SettingsLoader() : this(new FileSystem())
        {
        }

        /// <summary>
        /// Internal constructor to allow dependancy injection for unit testing.
        /// </summary>
        internal SettingsLoader(IFileSystem fileSystem)
        {
            this.FileSystem = fileSystem;
        }

        /// <summary>
        ///  Gets the <see cref="IFileSystem"/> implementation.
        /// </summary>
        private IFileSystem FileSystem { get; }

        /// <inheritdoc />
        public HarnessConfiguration GetMongoConfiguration(string configFilePath)
        {
            var fileValidationResult =
                this.FileSystem.ValidateFile(configFilePath);

            if (!fileValidationResult?.IsValid ?? false)
            {
                throw new SettingsLoaderException(
                    fileValidationResult?.Message ?? "Unable to validate filepath");
            }

            if (this.FileSystem.Path.GetExtension(configFilePath) != ".json")
            {
                throw new ArgumentOutOfRangeException(
                    nameof(configFilePath),
                    "Invalid file type. File must be a .json file.");
            }

            var json = this.FileSystem.File.ReadAllText(configFilePath);

            return JsonConvert.DeserializeObject<HarnessConfiguration>(json);
        }
    }
}
