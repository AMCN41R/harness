using System;
using Xunit;
using Harness.Attributes;

namespace Harness.UnitTests
{
    public class ExtensionsTests
    {
        [Fact]
        public void GetAttribute_CallOnClasWithCustomAttribute_RetunrsTheAttribute()
        {
            // Arrange
            var testClass = new ExtensionsTestClass();

            // Act
            var result = 
                testClass.GetType().GetAttribute<HarnessConfigAttribute>();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<HarnessConfigAttribute>(result);

        }

        [Fact]
        public void GetAttribute_CallOnClassWithoutAttribute_ReturnsNull()
        {
            // Arrange
            var testClass = new ExtensionTestClassWithoutAttribute();

            // Act
            var result =
                testClass.GetType().GetAttribute<HarnessConfigAttribute>();

            // Assert
            Assert.Null(result);

        }

        [Fact]
        public void GetAttribute_CallOnClassWithDifferentAtribte_ReturnsNull()
        {
            // Arrange
            var testClass = new ExtensionTestClassWithDifferentAttribute();

            // Act
            var result =
                testClass.GetType().GetAttribute<HarnessConfigAttribute>();

            // Assert
            Assert.Null(result);

        }

        [HarnessConfigAttribute]
        private class ExtensionsTestClass
        {
        }

        private class ExtensionTestClassWithoutAttribute
        {
        }

        [TestAttribute]
        private class ExtensionTestClassWithDifferentAttribute
        {
        }

        private class TestAttribute : Attribute
        {
        }

    }
}
