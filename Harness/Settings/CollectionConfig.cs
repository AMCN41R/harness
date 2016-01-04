
namespace MongoDbUnit.Settings
{
    public class CollectionConfig
    {
        /// <summary>
        /// Gets or sets the name of the MongoDb collection.
        /// </summary>
        public string CollectionName { get; set; }

        /// <summary>
        /// Gets or sets the path to the test data file.
        /// </summary>
        public string DataFileLocation { get; set; }
    }
}
