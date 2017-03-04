using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using Xunit;
using NSubstitute;
using Harness.Settings;

namespace Harness.UnitTests.SettingsTests
{
    public class SettingsLoaderTests
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
            var classUnderTest = new SettingsLoader(fakeFileSystem);

            // Act / Assert
            Assert.Throws<SettingsLoaderException>(
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

            var classUnderTest = new SettingsLoader(fakeFileSystem);

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

            var classUnderTest = new SettingsLoader(fakeFileSystem);

            // Act / Assert
            Assert.Throws<SettingsLoaderException>(
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

            var expected = this.TestSettings;

            var classUnderTest = new SettingsLoader(fakeFileSystem);


            // Act
            var result =
                classUnderTest.GetMongoConfiguration(testFilePath);


            // Assert
            Assert.Equal(expected, result, Comparers.HarnessConfigurationComparer());
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

            var classUnderTest = new SettingsLoader(fakeFileSystem);

            // Act / Assert
            Assert.Throws<Newtonsoft.Json.JsonReaderException>(
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

        private HarnessConfiguration TestSettings
            =>
                new HarnessConfiguration
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
