using System;

namespace Harness.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MongoIntegrationTestAttribute : Attribute
    {
        /// <summary>
        /// Set to false to tell the Harness integration test runner to 
        /// skip the test. <see cref="Skip"/> is set to false by default.
        /// </summary>
        public bool Skip { get; set; }

        public MongoIntegrationTestAttribute()
        {
            this.Skip = false;
        }
    }
}
