using System.Collections.Generic;

namespace Harness.Settings
{
    /// <summary>
    /// An interface to provide the <see cref="HarnessManager"/> with a 
    /// mechanism to retrieve data for a MongoDb collection.
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// Gets data from a source as an <see cref="IEnumerable{Object}"/>.
        /// </summary>
        /// <returns><see cref="IEnumerable{Object}"/></returns>
        IEnumerable<object> GetData();
    }
}