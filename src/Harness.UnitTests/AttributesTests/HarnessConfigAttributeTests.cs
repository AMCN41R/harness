using Harness.Attributes;
using Xunit;

namespace Harness.UnitTests.AttributesTests
{
    public class HarnessConfigAttributeTests
    {
        [Fact]
        public void ConfigFilePath_WhenNewInstanceIsCreated_IsSetToEmptyString()
        {
            // Arrange
            var classUnderTest = new HarnessConfigAttribute();

            // Act
            Assert.Equal(string.Empty, classUnderTest.ConfigFilePath);

        }

        [Fact]
        public void ConfigFilePath_SetValue_GetReturnsTheSameValue()
        {
            // Arange
            var classUnderTest =
                new HarnessConfigAttribute
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
