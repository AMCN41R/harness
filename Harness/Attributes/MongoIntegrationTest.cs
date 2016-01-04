using System;

namespace Harness.Attributes
{
    public class MongoIntegrationTest : Attribute
    {
        /// <summary>
        /// Set to false to tell the MongoDbUnit integration test runner to 
        /// skip the test.
        /// </summary>
        public bool Skip { get; set; }

        public MongoIntegrationTest()
        {
            this.Skip = false;
        }
    }
}
