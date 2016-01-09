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
        public void GetMongoConfiguration_PassNullEmptyOrWhiteSpace_ReturnsNull(string testValue)
        {
            // Arrange
            var fakeFileSystem = Substitute.For<IFileSystem>();
            var classUnderTest = new SettingsManager(fakeFileSystem);

            // Act
            var result = classUnderTest.GetMongoConfiguration(testValue);

            // Assert
            Assert.Null(result);

        }

        [Fact]
        public void GetMongoConfiguration_WhenFileDoesNotExist_ReturnsNull()
        {
            // Arrange
            var fakeFileSystem = Substitute.For<IFileSystem>();
            fakeFileSystem
                .File
                .Exists(Arg.Any<string>())
                .Returns(false);

            var classUnderTest = new SettingsManager(fakeFileSystem);

            // Act
            var result = classUnderTest.GetMongoConfiguration("anyFilePath");

            // Assert
            Assert.Null(result);

        }

        [Fact]
        public void GetMongoConfiguration_GivenValidJson_ReturnsExpectedSettings()
        {
            // Arrange
            var fakeFileSystem = Substitute.For<IFileSystem>();
            fakeFileSystem
                .File
                .ReadAllText(Arg.Any<string>())
                .Returns(this.TestSettingsString);
            fakeFileSystem
                .File
                .Exists(Arg.Any<string>())
                .Returns(true);

            var classUnderTest = new SettingsManager(fakeFileSystem);

            // Act
            var expected = this.TestSettings;
            var result = classUnderTest.GetMongoConfiguration("anyFilePath");

            // Assert
            ObjectComparer.AssertObjectsAreEqual(expected, result);

        }

        private string TestSettingsString
            => "{\"SaveOutput\": false," +
                "\"Databases\": [{" +
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
                    SaveOutput = false,
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
