using Harness.Attributes;
using Xunit;

namespace Harness.UnitTests.AttributesTests
{
    public class MongoIntegrationTestClassTests
    {
        [Fact]
        public void ConfigFilePath_WhenNewInstanceIsCreated_IsSetToEmptyString()
        {
            // Arrange
            var classUnderTest = new MongoIntegrationTestClass();

            // Act
            Assert.Equal(string.Empty, classUnderTest.ConfigFilePath);

        }

        [Fact]
        public void ConfigFilePath_SetValue_GetReturnsTheSameValue()
        {
            // Arange
            var classUnderTest =
                new MongoIntegrationTestClass
                {
                    ConfigFilePath = "Test\\file.json"
                };

            // Act
            classUnderTest.ConfigFilePath = "Different\\file.json";
            var result = classUnderTest.ConfigFilePath;

            // Assert
            Assert.Equal("Different\\file.json", result);

        }
    }
}
