using System.Collections.Generic;
using Xunit;
using NSubstitute;
using Harness.Attributes;

namespace Harness.UnitTests
{
    public class MongoIntegrationBaseTests
    {
        /// <summary>
        /// Using a derived class with the correct attribute that specifies a 
        /// filepath, passes the filepath to the HarnessManager, and calls the
        /// Using(string) method with the expected filepath and calls the 
        /// Build() method.
        /// </summary>
        [Fact]
        public void MongoIntegrationBase_AttributeAndFilePath_MakesCorrectCalls()
        {
            // Arrange
            var fakeHarnessManager = Substitute.For<IHarnessManager>();
            fakeHarnessManager
                .Using(Arg.Any<string>())
                .Returns(fakeHarnessManager);
            fakeHarnessManager
                .Build()
                .ReturnsForAnyArgs(
                    new Dictionary<string, MongoDB.Driver.IMongoClient>());

            // Act
            // ReSharper disable once UnusedVariable
            var classUnderTest =
                new TestableMongoIntegrationBase(fakeHarnessManager);

            // Assert
            fakeHarnessManager.Received().Using("TestPath");
            fakeHarnessManager.Received().Build();

        }

        /// <summary>
        /// Using a derived class with the correct attribute that does not
        /// specify a filepath, passes the default filepath to the 
        /// Using(string) method of the HarnessManager, and then calls the 
        /// Build method.
        /// </summary>
        [Fact]
        public void MongoIntegrationBase_AttributeOnly_MakesCorrectCalls()
        {
            // Arrange
            var fakeHarnessManager = Substitute.For<IHarnessManager>();
            fakeHarnessManager
                .Using(Arg.Any<string>())
                .Returns(fakeHarnessManager);
            fakeHarnessManager
                .Build()
                .ReturnsForAnyArgs(
                    new Dictionary<string, MongoDB.Driver.IMongoClient>());

            // Act
            // ReSharper disable once UnusedVariable
            var classUnderTest =
                new TestableMongoIntegrationBaseNoFilePath(fakeHarnessManager);

            // Assert
            fakeHarnessManager.Received().Using(
                "TestableMongoIntegrationBaseNoFilePath.json");
            fakeHarnessManager.Received().Build();

        }

        /// <summary>
        /// Using a derived class with no attribute, the base class passes the 
        /// default filepath to the Using(string) method of the HarnessManager, 
        /// and then calls the Build method.
        /// </summary>
        [Fact]
        public void 
            MongoIntegrationBase_NoAttribute_ThrowsException()
        {
            // Arrange
            var fakeHarnessManager = Substitute.For<IHarnessManager>();
            fakeHarnessManager
                .Using(Arg.Any<string>())
                .Returns(fakeHarnessManager);
            fakeHarnessManager
                .Build()
                .ReturnsForAnyArgs(
                    new Dictionary<string, MongoDB.Driver.IMongoClient>());

            // Act
            // ReSharper disable once UnusedVariable
            var classUnderTest =
                new TestableMongoIntegrationBaseWithoutAttribute(fakeHarnessManager);

            // Assert
            fakeHarnessManager.Received().Using(
                "TestableMongoIntegrationBaseWithoutAttribute.json");
            fakeHarnessManager.Received().Build();
        }

        [HarnessConfig(ConfigFilePath = "TestPath")]
        private class TestableMongoIntegrationBase : HarnessBase
        {
            public TestableMongoIntegrationBase(IHarnessManager harnessManager)
                : base(harnessManager)
            {
            }
            
        }

        [HarnessConfig]
        private class TestableMongoIntegrationBaseNoFilePath 
            : HarnessBase
        {
            public TestableMongoIntegrationBaseNoFilePath(IHarnessManager harnessManager)
                : base(harnessManager)
            {
            }
            
        }

        private class TestableMongoIntegrationBaseWithoutAttribute
            : HarnessBase
        {
            public TestableMongoIntegrationBaseWithoutAttribute(
                IHarnessManager harnessManager)
                : base(harnessManager)
            {
            }
            
        }

    }
}
