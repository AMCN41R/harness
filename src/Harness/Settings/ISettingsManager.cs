using System;

namespace Harness.Settings
{
    internal interface ISettingsManager
    {
        /// <summary>
        /// Loads and deserializes a JSON file using the given path to a new 
        /// instance of <see cref="MongoConfiguration"/>.
        /// </summary>
        /// <param name="configFilePath">The file JSON path to load.</param>
        /// <returns>A new instance of <see cref="MongoConfiguration"/>.</returns>
        /// <exception cref="SettingsManagerException">Thrown if the given filepath is invalid.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the given filepath's extension is not .json.</exception>
        /// <exception cref="ArgumentException">Thrown when the contents loaded from the given filepath cannot be deserialized.</exception>
        MongoConfiguration GetMongoConfiguration(string configFilePath);
    }
}
