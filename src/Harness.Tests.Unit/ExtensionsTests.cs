using System;
using NSubstitute;
using Xunit;

namespace Harness.Tests.Unit
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

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ValidateFile_NullEmptyAndWhiteSpaceFilename_ReturnsFalseValidationResult(string value)
        {
            // Arrange
            var fakeFileSystem = Substitute.For<IFileSystem>();

            // Act
            var result = fakeFileSystem.ValidateFile(value);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ValidateFile_FilepathThatDoesNotExist_ReturnsFalseValidationResult()
        {
            // Arrange
            var fakeFileSystem = Substitute.For<IFileSystem>();
            fakeFileSystem.FileExists(Arg.Any<string>()).Returns(false);

            // Act
            var result = fakeFileSystem.ValidateFile("filepath");

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ValidateFile_FilepathThatExists_ReturnsTrueValidationResult()
        {
            // Arrange
            var fakeFileSystem = Substitute.For<IFileSystem>();
            fakeFileSystem.FileExists(Arg.Any<string>()).Returns(true);

            // Act
            var result = fakeFileSystem.ValidateFile("filepath");

            // Assert
            Assert.True(result.IsValid);
        }

        [HarnessConfig]
        private class ExtensionsTestClass
        {
        }

        private class ExtensionTestClassWithoutAttribute
        {
        }

        [Test]
        private class ExtensionTestClassWithDifferentAttribute
        {
        }

        private class TestAttribute : Attribute
        {
        }

    }
}
