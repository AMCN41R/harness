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

        [Fact]
        public void AutoRun_WhenNewInstanceIsCreated_IsSetToTrue()
        {
            // Arrange
            var classUnderTest = new HarnessConfigAttribute();

            // Act
            Assert.True(classUnderTest.AutoRun);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AutoRun_SetValue_GetReturnsTheSameValue(bool value)
        {
            // Arange
            var classUnderTest =
                new HarnessConfigAttribute
                {
                    AutoRun = value
                };

            // Act
            var result = classUnderTest.AutoRun;

            // Assert
            Assert.Equal(value, result);

        }
    }
}
