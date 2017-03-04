using System.Collections.Generic;

namespace Harness.Settings
{
    public class HarnessConfiguration
    {
        /// <summary>
        /// Gets or sets the list of database configurations.
        /// </summary>
        public List<DatabaseConfig> Databases { get; set; }
    }
}
