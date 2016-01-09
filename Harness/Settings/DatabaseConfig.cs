using System.Collections.Generic;

namespace Harness.Settings
{
    public class DatabaseConfig
    {
        /// <summary>
        /// Gets or sets the database name.
        /// </summary>
        public string DatabaseName { get; set; }
        
        /// <summary>
        /// Gets or sets the databse connections string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the the string to add as a suffix to the database name.
        /// </summary>
        public string DatabaseNameSuffix { get; set; }

        /// <summary>
        /// Gets or sets the the string to add as a suffix to each of the 
        /// collection names.
        /// </summary>
        public string CollectionNameSuffix { get; set; }

        /// <summary>
        /// Gets or sets whether to drop the entire database if it already 
        /// exists and create a new one.
        /// </summary>
        public bool DropFirst { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="CollectionConfig"/> items to 
        /// add to the database.
        /// </summary>
        public List<CollectionConfig> Collections { get; set; }
    }
}
