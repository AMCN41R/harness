
namespace Harness.Settings
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

        /// <summary>
        /// Gets or sets whether to drop the collection if it already exists 
        /// and create a new one.
        /// </summary>
        public bool DropFirst { get; set; }
    }
}
