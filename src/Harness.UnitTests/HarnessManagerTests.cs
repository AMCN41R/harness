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
            var fakeSettingsManager = Substitute.For<ISettingsLoader>();
            fakeSettingsManager
                .GetMongoConfiguration(Arg.Any<string>())
                .Returns(new MongoConfiguration());

            // Act
            IHarnessManager classUnderTest = new HarnessManager(fakeSettingsManager);
            classUnderTest.UsingSettings("TestPath");

            // Assert
            fakeSettingsManager.Received().GetMongoConfiguration("TestPath");

        }


    }
}
