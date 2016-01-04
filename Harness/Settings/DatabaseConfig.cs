using System.Collections.Generic;

namespace MongoDbUnit.Settings
{
    public class DatabaseConfig
    {
        public string DatabaseName { get; set; }

        public string DatabaseConnectionString { get; set; }

        public string DatabaseNameSuffixing { get; set; }

        public string CollectionNameSuffixing { get; set; }

        public List<CollectionConfig> Collections { get; set; }
    }
}
