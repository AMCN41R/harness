using System;

namespace Harness.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// <example>
    /// <code lang="C#">
    /// [HarnessConfig(ConfigFilePath = "..\\HarnessConfig.json")]
    /// public class MyMongoIntegrationTests : HarnessBase
    /// {
    ///     [Fact]
    ///     public void Test()
    ///     {
    ///         // Test something...
    ///     }
    /// }
    /// </code>
    /// <example>
    /// <code lang="C#">
    /// [HarnessConfig(ConfigFilePath = "..\\HarnessConfig.json", AutoRun = false)]
    /// public class MyMongoIntegrationTests : HarnessBase
    /// {
    ///     [Fact]
    ///     public void Test()
    ///     {
    ///         base.BuildDatabase();
    /// 
    ///         // Test something...
    ///     }
    /// }
    /// </code>
    /// </example>
    /// </example>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public class HarnessConfigAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the path to the Harness configuration file.
        /// </summary>
        public string ConfigFilePath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if the MongoDB setup should happen
        /// automatically or not.
        /// If set to true, the database will be put into the required state
        /// during class construction. If set to false, the BuildDatabase() 
        /// method must be called.
        /// The default value is true.
        /// </summary>
        public bool AutoRun { get; set; }

        public HarnessConfigAttribute()
        {
            this.ConfigFilePath = string.Empty;

            this.AutoRun = true;
        }
    }
}
