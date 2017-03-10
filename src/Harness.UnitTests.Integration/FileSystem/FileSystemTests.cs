using System;
using Xunit;

namespace Harness.UnitTests.Integration.FileSystem
{
    public class FileSystemTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GetFileExtension_NullEmptyOrWhiteSpacePath_ThrowsArgumentNullException(string value)
        {
            Assert.Throws<ArgumentNullException>(
                () => new Harness.FileSystem().GetFileExtension(value));
        }

        [Fact]
        public void GetFileExtension_FilenameWithoutExtension_ReturnsEmptyString()
        {
            Assert.Equal(
                string.Empty,
                new Harness.FileSystem().GetFileExtension("FileSystem\\FileWithoutExtension")
            );
        }

        [Fact]
        public void GetFileExtension_ValidFileName_ReturnsExpectedExtension()
        {
            Assert.Equal(
                ".txt",
                new Harness.FileSystem().GetFileExtension("FileSystem\\FileWithExtension.txt")
            );
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void ReadAllText_NullEmptyOrWhiteSpacePath_ThrowsArgumentNullException(string value)
        {
            Assert.Throws<ArgumentNullException>(
                () => new Harness.FileSystem().ReadAllText(value));
        }

        [Fact]
        public void ReadAllText_FileWithNoText_ReturnsEmptyString()
        {
            Assert.Equal(
                string.Empty,
                new Harness.FileSystem().ReadAllText("FileSystem\\FileWithNoText.txt")
            );
        }

        [Fact]
        public void ReadAllText_FileWithText_ReturnsExpectedText()
        {
            Assert.Equal(
                "Here is some text.",
                new Harness.FileSystem().ReadAllText("FileSystem\\FileWithText.txt")
            );
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void FileExists_NullEmptyOrWhiteSpacePath_ReturnsFalse(string value)
        {
            Assert.False(new Harness.FileSystem().FileExists(value));
        }

        [Fact]
        public void FileExists_FileThatDoesNotExist_ReturnsFalse()
        {
            Assert.False(new Harness.FileSystem().FileExists("FileThatDoesNotExist.nope"));
        }

        [Fact]
        public void FileExists_FileThatExists_ReturnsTrue()
        {
            Assert.False(new Harness.FileSystem().FileExists("FileSystem\\FileThatDoesExists.txt"));
        }
    }
}
