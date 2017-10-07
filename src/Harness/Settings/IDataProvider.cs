using System.Collections.Generic;

namespace Harness.Settings
{
    /// <summary>
    /// An interface to provide the <see cref="HarnessManager"/> with a 
    /// mechanism to retrieve data for a MongoDb collection.
    /// </summary>
    /// <example>
    /// <code lang="C#">
    /// public class PersonDataProvider : IDataProvider
    /// {
    ///     public IEnumerable&lt;object&gt; GetData()
    ///     {
    ///         return new List&lt;Person&gt;
    ///         {
    ///             new Person {FirstName = "Peter", LastName = "Venkman", Age = 31},
    ///             new Person {FirstName = "Ray", LastName = "Stantz", Age = 32},
    ///             new Person {FirstName = "Egon", LastName = "Spengler", Age = 33}
    ///         };
    ///     }
    /// }
    /// 
    /// public class Person
    /// {
    ///     public ObjectId Id { get; set; }
    ///     public string FirstName { get; set; }
    ///     public string LastName { get; set; }
    ///     public int Age { get; set; }
    /// }
    /// </code>
    /// </example>
    public interface IDataProvider
    {
        /// <summary>
        /// Gets data from a source as an <see cref="IEnumerable{Object}"/>.
        /// </summary>
        /// <returns><see cref="IEnumerable{Object}"/></returns>
        IEnumerable<object> GetData();
    }
}