using Xunit;
using NSubstitute;
using Harness.Attributes;
using Harness.Settings;

namespace Harness.UnitTests
{
    public class MongoIntegrationBaseTests
    {
        /// <summary>
        /// Using a derived class with the correct attribute that specifies a 
        /// filepath, passes the filepath to the SettingsManager, calls the 
        /// GetMongoConfiguration method and calls the SessionManager Build
        /// method.
        /// </summary>
        [Fact]
        public void MongoIntegrationBase_AttributeAndFilePath_MakesCorrectCalls()
        {
            // Arrange
            var filePathReceivedBySettingsManager = string.Empty;

            var fakeSettingsManager = Substitute.For<ISettingsManager>();
            fakeSettingsManager
                .GetMongoConfiguration(Arg.Any<string>())
                .Returns(new MongoConfiguration());
            fakeSettingsManager
                .GetMongoConfiguration(Arg.Do<string>(
                    x => filePathReceivedBySettingsManager = x));

            // Act
            var classUnderTest =
                new TestableMongoIntegrationBase(fakeSettingsManager);

            // Assert
            Assert.True(classUnderTest.SessionManagerReceivedCallToBuild);
            Assert.Equal("TestPath", filePathReceivedBySettingsManager);

        }

        /// <summary>
        /// Using a derived class with the correct attribute that does not
        /// specify a filepath, passes the default filepath to the 
        /// SettingsManager, calls the GetMongoConfiguration method and calls 
        /// the SessionManager Build method.
        /// </summary>
        [Fact]
        public void MongoIntegrationBase_AttributeOnly_MakesCorrectCalls()
        {
            // Arrange
            var filePathReceivedBySettingsManager = string.Empty;

            var fakeSettingsManager = Substitute.For<ISettingsManager>();
            fakeSettingsManager
                .GetMongoConfiguration(Arg.Any<string>())
                .Returns(new MongoConfiguration());
            fakeSettingsManager
                .GetMongoConfiguration(Arg.Do<string>(
                    x => filePathReceivedBySettingsManager = x));

            // Act
            var classUnderTest =
                new TestableMongoIntegrationBaseNoFilePath(fakeSettingsManager);

            // Assert
            Assert.True(classUnderTest.SessionManagerReceivedCallToBuild);
            Assert.Equal(
                "TestableMongoIntegrationBaseNoFilePath.json", 
                filePathReceivedBySettingsManager
                );

        }

        /// <summary>
        /// Using a derived class with no attribute, the base class throws
        /// a RequiredAttributeNotFoundException.
        /// </summary>
        [Fact]
        public void 
            MongoIntegrationBase_NoAttribute_ThrowsException()
        {
            // Arrange
            var fakeSettingsManager = Substitute.For<ISettingsManager>();

            // Act / Assert
            Assert.Throws<RequiredAttributeNotFoundException>(() =>
                new TestableMongoIntegrationBaseWithoutAttribute(fakeSettingsManager));

        }

        [MongoIntegrationTestClassAttribute(ConfigFilePath = "TestPath")]
        private class TestableMongoIntegrationBase : MongoIntegrationBase
        {
            public bool SessionManagerReceivedCallToBuild { get; private set; }

            public TestableMongoIntegrationBase(ISettingsManager settingsManager)
                : base(settingsManager)
            {
            }

            internal override IMongoSessionManager MongoSessionManager()
            {
                var fakeMongoSessionManager =
                    Substitute.For<IMongoSessionManager>();

                fakeMongoSessionManager
                    .When(x => x.Build())
                    .Do(x => this.SessionManagerReceivedCallToBuild = true);

                return fakeMongoSessionManager;

            }
        }

        [MongoIntegrationTestClassAttribute]
        private class TestableMongoIntegrationBaseNoFilePath 
            : MongoIntegrationBase
        {
            public bool SessionManagerReceivedCallToBuild { get; private set; }

            public TestableMongoIntegrationBaseNoFilePath(ISettingsManager settingsManager)
                : base(settingsManager)
            {
            }

            internal override IMongoSessionManager MongoSessionManager()
            {
                var fakeMongoSessionManager =
                    Substitute.For<IMongoSessionManager>();

                fakeMongoSessionManager
                    .When(x => x.Build())
                    .Do(x => this.SessionManagerReceivedCallToBuild = true);

                return fakeMongoSessionManager;

            }
        }

        private class TestableMongoIntegrationBaseWithoutAttribute
            : MongoIntegrationBase
        {
            public TestableMongoIntegrationBaseWithoutAttribute(
                ISettingsManager settingsManager)
                : base(settingsManager)
            {
            }

            internal override IMongoSessionManager MongoSessionManager()
            {
                var fakeMongoSessionManager =
                    Substitute.For<IMongoSessionManager>();

                return fakeMongoSessionManager;

            }
        }

    }
}
