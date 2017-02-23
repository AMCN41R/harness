using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using Xunit;
using NSubstitute;
using Harness.Settings;
using Harness.UnitTests.Comparer;

namespace Harness.UnitTests.SettingsTests
{
    public class SettingsManagerTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("    ")]
        [InlineData(null)]
        public void GetMongoConfiguration_PassNullEmptyOrWhiteSpace_ThrowsSettingsManagerException(string testValue)
        {
            // Arrange
            var fakeFileSystem = Substitute.For<IFileSystem>();
            var classUnderTest = new SettingsManager(fakeFileSystem);

            // Act / Assert
            Assert.Throws<SettingsManagerException>(
                () => classUnderTest.GetMongoConfiguration(testValue));
        }

        [Fact]
        public void GetMongoConfiguration_PassPathThatDoesNotHaveJsonExtension_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var fakeFileSystem = Substitute.For<IFileSystem>();
            fakeFileSystem
                .File
                .Exists(Arg.Any<string>())
                .Returns(true);
            fakeFileSystem
                .Path
                .GetExtension(Arg.Any<string>())
                .Returns("doc");

            var classUnderTest = new SettingsManager(fakeFileSystem);

            // Act / Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () => classUnderTest.GetMongoConfiguration("anyFilePath.doc"));

        }

        [Fact]
        public void GetMongoConfiguration_WhenFileDoesNotExist_ThrowsSettingsManagerException()
        {
            // Arrange
            var fakeFileSystem = Substitute.For<IFileSystem>();
            fakeFileSystem
                .File
                .Exists(Arg.Any<string>())
                .Returns(false);

            var classUnderTest = new SettingsManager(fakeFileSystem);

            // Act / Assert
            Assert.Throws<SettingsManagerException>(
                () => classUnderTest.GetMongoConfiguration("anyFilePath.json"));
        }

        [Fact]
        public void GetMongoConfiguration_GivenValidJson_ReturnsExpectedSettings()
        {
            // Arrange
            var testFilePath = "anyFilePath.json";

            var fakeFileSystem = Substitute.For<IFileSystem>();
            fakeFileSystem
                .File
                .ReadAllText(Arg.Any<string>())
                .Returns(this.TestSettingsString);
            fakeFileSystem
                .File
                .Exists(Arg.Any<string>())
                .Returns(true);
            fakeFileSystem
                .Path
                .GetExtension(testFilePath)
                .Returns(".json");

            var classUnderTest = new SettingsManager(fakeFileSystem);

            // Act
            var expected = this.TestSettings;
            var result =
                classUnderTest.GetMongoConfiguration(testFilePath);

            // Assert
            ObjectComparer.AssertObjectsAreEqual(expected, result);

        }

        [Fact]
        public void GetMongoConfiguration_GivenMalformedJson_ThrowsArgumentExcpetion()
        {
            // Arrange
            var testFilePath = "anyFilePath.json";
            var badJson = "{\"SaveOutput\": false \"SomethigElse\": true";

            var fakeFileSystem = Substitute.For<IFileSystem>();
            fakeFileSystem
                .File
                .ReadAllText(Arg.Any<string>())
                .Returns(badJson);
            fakeFileSystem
                .File
                .Exists(Arg.Any<string>())
                .Returns(true);
            fakeFileSystem
                .Path
                .GetExtension(testFilePath)
                .Returns(".json");

            var classUnderTest = new SettingsManager(fakeFileSystem);

            // Act / Assert
            Assert.Throws<ArgumentException>(
                () => classUnderTest.GetMongoConfiguration(testFilePath));

        }

        private string TestSettingsString
            => "{\"Databases\": [{" +
                "\"DatabaseName\": \"TestDb1\"," +
                "\"ConnectionString\": \"mongodb://localhost:27017\"," +
                "\"DatabaseNameSuffix\": \"\"," +
                "\"CollectionNameSuffix\": \"\"," +
                "\"DropFirst\": false," +
                "\"Collections\": [{" +
                "\"CollectionName\": \"TestCollection1\"," +
                "\"DataFileLocation\": \"TestData\\\\Collection1.json\"," +
                "\"DropFirst\": false},{" +
                "\"CollectionName\": \"TestCollection2\"," +
                "\"DataFileLocation\": \"TestData\\\\Collection2.json\"," +
                "\"DropFirst\": false}]}]}";

        private MongoConfiguration TestSettings
            =>
                new MongoConfiguration
                {
                    Databases =
                        new List<DatabaseConfig>
                        {
                            new DatabaseConfig
                            {
                                DatabaseName = "TestDb1",
                                ConnectionString = "mongodb://localhost:27017",
                                DatabaseNameSuffix = "",
                                CollectionNameSuffix = "",
                                DropFirst = false,
                                Collections =
                                    new List<CollectionConfig>
                                    {
                                        new CollectionConfig
                                        {
                                            CollectionName = "TestCollection1",
                                            DataFileLocation = "TestData\\Collection1.json",
                                            DropFirst = false
                                        },
                                        new CollectionConfig
                                        {
                                            CollectionName = "TestCollection2",
                                            DataFileLocation = "TestData\\Collection2.json",
                                            DropFirst = false
                                        }
                                    }
                            }
                        }

                };
    }
}
