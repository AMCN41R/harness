namespace Harness.Settings
{
    public class CollectionConfig
    {
        /// <summary>
        /// Gets or sets the name of the MongoDb collection.
        /// </summary>
        public string CollectionName { get; set; }

        /// <summary>
        /// Gets or sets whether to drop the collection if it already exists 
        /// before adding data to the collection.
        /// </summary>
        public bool DropFirst { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IDataProvider"/> instance that will 
        /// provide the data that will be added to the collection.
        /// </summary>
        public IDataProvider DataProvider { get; set; }

        /// <summary>
        /// Gets or sets the path to the test data file. This is only used when
        /// importing data from an external json file, and is turned into 
        /// an instance of <see cref="FromJsonFileDataProvider"/>.
        /// </summary>
        public string DataFileLocation { get; set; }
    }
}
