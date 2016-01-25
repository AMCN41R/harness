using Harness.Settings;
using Xunit;
using NSubstitute;

namespace Harness.UnitTests
{
    public class HarnessManagerTests
    {
        [Fact]
        public void Using_PassFilepath_PassesFilepathToCorrectSettingsManagerMethod()
        {
            // Arrange
            var fakeSettingsManager = Substitute.For < ISettingsManager>();
            fakeSettingsManager
                .GetMongoConfiguration(Arg.Any<string>())
                .Returns(new MongoConfiguration());

            // Act
            var classUnderTest = new HarnessManager(fakeSettingsManager);
            classUnderTest.Using("TestPath");

            // Assert
            fakeSettingsManager.Received().GetMongoConfiguration("TestPath");

        }

        
    }
}
