using Harness.Attributes;
using Xunit;

namespace Harness.Examples.XUnit.UsingTheHarnessBaseClass
{
    // To use Harness:
    // - add the HarnessConfig attribute to the class that contains the tests
    // - specify the relative path to the Harness settings file by setting the ConfigFilePath variable
    // - have the class that contains the tests extend HarnessBase

    // When a test is run, the HarnessBase class will use the settings file to 
    // put the mongo databases specified within it into the desired state.
    // This will happen automatically when the class constructor is called.

    // The HarnessBase class exposes the IMongoClient objects that it created
    // while setting up the databases so that, if required, they can be re-used
    // in the tests. This is exposed as a a Dictionary<string, IMongoClient>
    // where the dictionary key in the mongo server connection string.

    [Collection("Example.Tests")]
    [HarnessConfig(ConfigFilePath = "ExampleSettings.json")]
    public class AutoConfigureDatabase : HarnessBase
    {
        [Fact]
        public void Test1()
        {
            // Arrange
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
            var classUnderTest = new ClassUnderTest();

            // The method we are testing needs an instance of IMongoClient.
            // Rather than create a new one, we can re-use the one that was 
            // created by the HarnessBase class when it was setting up the 
            // databases.
            var mongoClient = base.MongoConnections["mongodb://localhost:27017"];

            // Act
            var result = classUnderTest.GetCollectionRecordCount(mongoClient, "TestCollection1");

            // Assert
            Assert.Equal(2, result);
        }
    }

}
