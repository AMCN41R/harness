using System;

namespace Harness.Attributes
{
    public class MongoIntegrationTestClass : Attribute
    {
        /// <summary>
        /// The path to the MongoDbUnit configuration file.
        /// </summary>
        public string ConfigFilePath { get; set; }

    }
}
