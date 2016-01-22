using Xunit;
using Harness.Attributes;

namespace Harness.UnitTests.AttributesTests
{
    public class MongoIntegrationTestTests
    {
        [Fact]
        public void Skip_WhenNewInstanceIsCreated_IsSetToFalse()
        {
            // Arrange
            var classUnderTest = new MongoIntegrationTestAttribute();

            // Assert
            Assert.False(classUnderTest.Skip);

        }

        [Fact]
        public void Skip_SetToTrue_GetReturnsTrue()
        {
            // Arrange
            var classUnderTest = new MongoIntegrationTestAttribute {Skip = true};

            // Act
            var result = classUnderTest.Skip;

            // Assert
            Assert.True(result);

        }
    }
}
