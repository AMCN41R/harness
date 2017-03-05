using System;
using Harness.Settings;
using Xunit;
using NSubstitute;

namespace Harness.UnitTests
{
    public class HarnessManagerTests
    {
        [Fact]
        public void UsingSettings_PassFilepath_PassesFilepathToCorrectSettingsManagerMethod()
        {
            // Arrange
            var fakeSettingsManager = Substitute.For<ISettingsLoader>();
            fakeSettingsManager
                .GetMongoConfiguration(Arg.Any<string>())
                .Returns(new HarnessConfiguration());

            IHarnessManager classUnderTest = new HarnessManager(fakeSettingsManager);

            // Act
            classUnderTest.UsingSettings("TestPath");

            // Assert
            fakeSettingsManager.Received().GetMongoConfiguration("TestPath");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void UsingSettings_NullEmptyOrWhitespaceFilepath_ThrowsArgumentNullException(string value)
        {
            // Arrange
            var fakeSettingsManager = Substitute.For<ISettingsLoader>();
            var classUnderTest = new HarnessManager(fakeSettingsManager);

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => classUnderTest.UsingSettings(value));
        }

        [Fact]
        public void UsingSettings_NullConfiguration_ThrowsArgumentNullException()
        {
            // Arrange
            var fakeSettingsManager = Substitute.For<ISettingsLoader>();
            var classUnderTest = new HarnessManager(fakeSettingsManager);

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => classUnderTest.UsingSettings(null as HarnessConfiguration));
        }
    }
}
