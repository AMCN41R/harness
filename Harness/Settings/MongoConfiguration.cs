using System.Collections.Generic;

namespace Harness.Settings
{
    public class MongoConfiguration
    {
        public bool SaveOutput { get; set; }

        public List<DatabaseConfig> Databases { get; set; }
    }
}
