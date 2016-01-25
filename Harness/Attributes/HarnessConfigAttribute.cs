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
    public class HarnessConfigAttribute : Attribute
    {
        /// <summary>
        /// The path to the Harness configuration file.
        /// </summary>
        public string ConfigFilePath { get; set; }

        public HarnessConfigAttribute()
        {
            this.ConfigFilePath = string.Empty;
        }
    }
}
