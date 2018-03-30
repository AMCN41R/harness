using Xunit;

namespace Harness.Tests.Unit
{
    public class ValidationResultTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsValid_SetPropertyValue_GetReturnsTheSameValue(bool value)
        {
            // Arrange
            var classUnderTest = new ValidationResult
            {
                IsValid = value
            };

            // Act
            var result = classUnderTest.IsValid;

            // Assert
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("Message")]
        public void Message_SetPropertyValue_GetReturnsTheSameValue(string value)
        {
            // Arrange
            var classUnderTest = new ValidationResult
            {
                Message = value
            };

            // Act
            var result = classUnderTest.Message;

            // Assert
            Assert.Equal(value, result);
        }
    }
}
