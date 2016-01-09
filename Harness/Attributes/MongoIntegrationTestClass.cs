using System;

namespace Harness.Attributes
{
    public class MongoIntegrationTestClass : Attribute
    {
        /// <summary>
        /// The path to the Harness configuration file.
        /// </summary>
        public string ConfigFilePath { get; set; }

        public MongoIntegrationTestClass()
        {
            this.ConfigFilePath = string.Empty;
        }
    }
}
