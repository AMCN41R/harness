using System.Collections.Generic;

namespace Harness.Settings
{
    /// <summary>
    /// Represents the Harness configuration options.
    /// </summary>
    public class HarnessConfiguration
    {
        /// <summary>
        /// Gets or sets the list of database configurations.
        /// </summary>
        public IList<DatabaseConfig> Databases { get; set; }

        /// <summary>
        /// Gets or sets the Mongo conventions to apply.
        /// </summary>
        public ConventionConfig Conventions { get; set; }
    }
}
