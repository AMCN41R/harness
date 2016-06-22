using System.Collections.Generic;
using Xunit;
using NSubstitute;
using Harness.Attributes;

namespace Harness.UnitTests
{
    public class HarnessBaseTests
    {
        /// <summary>
        /// Using a derived class with the correct attribute that specifies a 
        /// filepath, passes the filepath to the HarnessManager, and calls the
        /// Using(string) method with the expected filepath and calls the 
        /// Build() method.
        /// </summary>
        [Fact]
        public void HarnessBase_AttributeAndFilePath_MakesCorrectCalls()
        {
            // Arrange
            var fakeHarnessManager = Substitute.For<IHarnessManager>();
            fakeHarnessManager
                .UsingSettings(Arg.Any<string>())
                .Returns(fakeHarnessManager);
            fakeHarnessManager
                .Build()
                .ReturnsForAnyArgs(
                    new Dictionary<string, MongoDB.Driver.IMongoClient>());

            // Act
            // ReSharper disable once UnusedVariable
            var classUnderTest =
                new TestableHarnessBase(fakeHarnessManager);

            // Assert
            fakeHarnessManager.Received().UsingSettings("TestPath");
            fakeHarnessManager.Received().Build();

        }

        /// <summary>
        /// Using a derived class with the correct attribute that does not
        /// specify a filepath, passes the default filepath to the 
        /// Using(string) method of the HarnessManager, and then calls the 
        /// Build method.
        /// </summary>
        [Fact]
        public void HarnessBase_AttributeOnly_MakesCorrectCalls()
        {
            // Arrange
            var fakeHarnessManager = Substitute.For<IHarnessManager>();
            fakeHarnessManager
                .UsingSettings(Arg.Any<string>())
                .Returns(fakeHarnessManager);
            fakeHarnessManager
                .Build()
                .ReturnsForAnyArgs(
                    new Dictionary<string, MongoDB.Driver.IMongoClient>());

            // Act
            // ReSharper disable once UnusedVariable
            var classUnderTest =
                new TestableHarnessBaseNoFilePath(fakeHarnessManager);

            // Assert
            fakeHarnessManager.Received().UsingSettings(
                "TestableHarnessBaseNoFilePath.json");
            fakeHarnessManager.Received().Build();

        }

        /// <summary>
        /// Using a derived class with no attribute, the base class passes the 
        /// default filepath to the Using(string) method of the HarnessManager, 
        /// and then calls the Build method.
        /// </summary>
        [Fact]
        public void
            HarnessBase_NoAttribute_ThrowsException()
        {
            // Arrange
            var fakeHarnessManager = Substitute.For<IHarnessManager>();
            fakeHarnessManager
                .UsingSettings(Arg.Any<string>())
                .Returns(fakeHarnessManager);
            fakeHarnessManager
                .Build()
                .ReturnsForAnyArgs(
                    new Dictionary<string, MongoDB.Driver.IMongoClient>());

            // Act
            // ReSharper disable once UnusedVariable
            var classUnderTest =
                new TestableHarnessBaseWithoutAttribute(fakeHarnessManager);

            // Assert
            fakeHarnessManager.Received().UsingSettings(
                "TestableHarnessBaseWithoutAttribute.json");
            fakeHarnessManager.Received().Build();
        }

        [HarnessConfig(ConfigFilePath = "TestPath")]
        private class TestableHarnessBase : HarnessBase
        {
            public TestableHarnessBase(IHarnessManager harnessManager)
                : base(harnessManager)
            {
            }

        }

        [HarnessConfig]
        private class TestableHarnessBaseNoFilePath
            : HarnessBase
        {
            public TestableHarnessBaseNoFilePath(IHarnessManager harnessManager)
                : base(harnessManager)
            {
            }

        }

        private class TestableHarnessBaseWithoutAttribute
            : HarnessBase
        {
            public TestableHarnessBaseWithoutAttribute(
                IHarnessManager harnessManager)
                : base(harnessManager)
            {
            }

        }

    }
}
