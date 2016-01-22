using System;

namespace Harness.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// <example>
    /// <code lang="C#">
    /// [MongoIntegrationTestClass(ConfigFilePath = "..\\HarnessConfig.json")]
    /// public class MyMongoIntegrationTests
    /// {
    ///     [Fact]
    ///     public void Test()
    ///     {
    ///         // Test something...
    ///     }
    /// }
    /// </code>
    /// </example>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public class MongoIntegrationTestClassAttribute : Attribute
    {
        /// <summary>
        /// The path to the Harness configuration file.
        /// </summary>
        public string ConfigFilePath { get; set; }

        public MongoIntegrationTestClassAttribute()
        {
            this.ConfigFilePath = string.Empty;
        }
    }
}
