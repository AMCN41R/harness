using Harness.Attributes;
using Xunit;

namespace Harness.Examples.XUnit.UsingTheHarnessBaseClass
{
    // To use Harness:
    // - add the HarnessConfig attribute to the class that contains the tests
    // - specify the relative path to the Harness settings file by setting the ConfigFilePath variable
    // - specify AUtoRun = false to stop the databases being created
    // - have the class that contains the tests extend HarnessBase on class construction.

    // Setting AutoRun = false, tells the HarnessBase class not to setup the 
    // databases until the BuildDatabase() method is called.

    // The HarnessBase class exposes the IMongoClient objects that it created
    // while setting up the databases so that, if required, they can be re-used
    // in the tests. This is exposed as a a Dictionary<string, IMongoClient>
    // where the dictionary key in the mongo server connection string.

    [HarnessConfig(ConfigFilePath = "ExampleSettings.json", AutoRun = false)]
    public class ManuallyConfigureDatabase : HarnessBase
    {
        [Fact]
        public void Test1()
        {
            // Arrange

            // As AutoRun is set to false on the class attribute, the BuildDatabase()
            // must be called to tell the HarnessBase class to setup the databases.
            base.BuildDatabase();

            var classUnderTest = new ClassUnderTest();

            // Act
            var result = classUnderTest.GetCollectionRecordCount("TestCollection1");

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void Test2()
        {
            // Arrange

            // As AutoRun is set to false on the class attribute, the BuildDatabase()
            // must be called to tell the HarnessBase class to setup the databases.
            base.BuildDatabase();

            var classUnderTest = new ClassUnderTest();

            // The method we are testing needs an instance of IMongoClient.
            // Rather than create a new one, we can re-use the one that was 
            // created by the HarnessBase class when it was setting up the 
            // databases.
            var mongoClient = base.MongoConnections["mongodb://localhost:20719"];

            // Act
            var result = classUnderTest.GetCollectionRecordCount(mongoClient, "TestCollection1");

            // Assert
            Assert.Equal(2, result);
        }
    }
}
