using System.Collections.Generic;

namespace Harness.Settings
{
    public class MongoConfiguration
    {
        /// <summary>
        /// Gets or sets whether to save a mongo dump of the database state
        /// after all tests have run.
        /// </summary>
        public bool SaveOutput { get; set; }

        /// <summary>
        /// Gets or sets the list of database configurations.
        /// </summary>
        public List<DatabaseConfig> Databases { get; set; }
    }

    public class MongoConfigurationBuilder
    {
        private MongoConfiguration Config { get; }


    }
}
