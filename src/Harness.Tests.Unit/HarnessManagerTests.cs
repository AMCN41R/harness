using System;
using Harness.Settings;
using NSubstitute;
using Xunit;

namespace Harness.Tests.Unit
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

            var fakeSessionFactory = Substitute.For<IMongoSessionManagerFactory>();

            IHarnessManager classUnderTest = 
                new HarnessManager(fakeSettingsManager, fakeSessionFactory);

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
            var fakeSessionFactory = Substitute.For<IMongoSessionManagerFactory>();
            var classUnderTest = 
                new HarnessManager(fakeSettingsManager, fakeSessionFactory);

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => classUnderTest.UsingSettings(value));
        }

        [Fact]
        public void UsingSettings_NullConfiguration_ThrowsArgumentNullException()
        {
            // Arrange
            var fakeSettingsManager = Substitute.For<ISettingsLoader>();
            var fakeSessionFactory = Substitute.For<IMongoSessionManagerFactory>();
            var classUnderTest = 
                new HarnessManager(fakeSettingsManager, fakeSessionFactory);

            // Act / Assert
            Assert.Throws<ArgumentNullException>(() => classUnderTest.UsingSettings(null as HarnessConfiguration));
        }
    }
}
