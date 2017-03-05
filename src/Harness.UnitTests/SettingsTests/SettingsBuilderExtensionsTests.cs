using System;
using System.Collections.Generic;
using Harness.Settings;
using Xunit;

namespace Harness.UnitTests.SettingsTests
{
    public class SettingsBuilderExtensionsTests
    {
        [Fact]
        public void AddDatabase_NullConfig_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => (null as HarnessConfiguration).AddDatabase("name"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void AddDatabase_NullEmptyOrWhitespaceName_ThrowsArgumentNullException(string value)
        {
            Assert.Throws<ArgumentNullException>(
                () => new HarnessConfiguration().AddDatabase(value));
        }

        [Fact]
        public void AddDatabase_WithNameThatAlreadyExists_ThrowsSettingsBuilderException()
        {
            // Arrange
            var config = new HarnessConfiguration
            {
                Databases = new List<DatabaseConfig>
                {
                    new DatabaseConfig { DatabaseName = "test" }
                }
            };

            // Act / Assert
            Assert.Throws<SettingsBuilderException>(() => config.AddDatabase("test"));
        }

        [Fact]
        public void AddDatabase_NoDatabases_AddsDatabaseConfig()
        {
            // Arrange
            var config = new HarnessConfiguration
            {
                Databases = new List<DatabaseConfig>()
            };

            var expected = new HarnessConfiguration
            {
                Databases = new List<DatabaseConfig>
                {
                    new DatabaseConfig { DatabaseName = "test" }
                }
            };


            // Act
            var result = config.AddDatabase("test");


            // Assert
            Assert.Equal(expected, result, Comparers.HarnessConfigurationComparer());
        }

        [Fact]
        public void AddDatabase_ExisitngDatabase_AddsAdditionalDatabaseConfig()
        {
            // Arrange
            var config = new HarnessConfiguration
            {
                Databases = new List<DatabaseConfig>
                {
                    new DatabaseConfig { DatabaseName = "test" }
                }
            };

            var expected = new HarnessConfiguration
            {
                Databases = new List<DatabaseConfig>
                {
                    new DatabaseConfig { DatabaseName = "test" },
                    new DatabaseConfig { DatabaseName = "second test" }
                }
            };


            // Act
            var result = config.AddDatabase("second test");


            // Assert
            Assert.Equal(expected, result, Comparers.HarnessConfigurationComparer());
        }

        [Fact]
        public void AddCollection_NullConfig_ThorwsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => (null as DatabaseConfig).AddCollection("name", true, "location"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void AddCollection_NullEmptyOrWhitespaceName_ThorwsArgumentNullException(string value)
        {
            Assert.Throws<ArgumentNullException>(
                () => new DatabaseConfig().AddCollection(value, true, "location"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void AddCollection_NullEmptyOrWhitespaceFileLocation_ThorwsArgumentNullException(string value)
        {
            Assert.Throws<ArgumentNullException>(
                () => new DatabaseConfig().AddCollection("name", true, value));
        }

        [Fact]
        public void AddCollection_WithNameThatAlreadyExists_ThrowsSettingsBuilderException()
        {
            // Arrange
            var config = new DatabaseConfig
            {
                Collections = new List<CollectionConfig>
                {
                    new CollectionConfig {CollectionName = "name"}
                }
            };

            // Act / Assert
            Assert.Throws<SettingsBuilderException>(
                () => config.AddCollection("name", true, "location"));
        }

        [Fact]
        public void AddCollection_NoExistingCollections_AddsCollectionConfig()
        {
            // Arrange
            var config = new DatabaseConfig
            {
                Collections = new List<CollectionConfig>()
            };

            var expected = new DatabaseConfig
            {
                Collections = new List<CollectionConfig>
                {
                    new CollectionConfig
                    {
                        CollectionName = "name",
                        DataFileLocation = "location",
                        DropFirst = true,
                        DataProvider = null
                    }
                }
            };

            // Act
            var result = config.AddCollection("name", true, "location");

            // Assert
            Assert.Equal(expected, result, Comparers.DatabaseConfigComparer());
        }

        [Fact]
        public void AddCollection_ExisitngCollections_AddsAdditionalCollectionConfig()
        {
            // Arrange
            var config = new DatabaseConfig
            {
                Collections = new List<CollectionConfig>
                {
                    new CollectionConfig
                    {
                        CollectionName = "name",
                        DataFileLocation = "location",
                        DropFirst = true,
                        DataProvider = null
                    }
                }
            };

            var expected = new DatabaseConfig
            {
                Collections = new List<CollectionConfig>
                {
                    new CollectionConfig
                    {
                        CollectionName = "name",
                        DataFileLocation = "location",
                        DropFirst = true,
                        DataProvider = null
                    },
                    new CollectionConfig
                    {
                        CollectionName = "name-2",
                        DataFileLocation = "location-2",
                        DropFirst = false,
                        DataProvider = null
                    }
                }
            };

            // Act
            var result = config.AddCollection("name-2", false, "location-2");

            // Assert
            Assert.Equal(expected, result, Comparers.DatabaseConfigComparer());
        }

        [Fact]
        public void AddDataProviderCollection_NullConfig_ThorwsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => (null as DatabaseConfig).AddDataProviderCollection<object>("name", true, new TestDataProvider()));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void AddDataProviderCollection_NullEmptyOrWhitespaceName_ThorwsArgumentNullException(string value)
        {
            Assert.Throws<ArgumentNullException>(
                () => new DatabaseConfig().AddDataProviderCollection<object>(value, true, new TestDataProvider()));
        }

        [Fact]
        public void AddDataProviderCollection_NullProvider_ThorwsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new DatabaseConfig().AddDataProviderCollection<object>("name", true, null));
        }

        [Fact]
        public void AddDataProviderCollection_WithNameThatAlreadyExists_ThrowsSettingsBuilderException()
        {
            // Arrange
            var config = new DatabaseConfig
            {
                Collections = new List<CollectionConfig>
                {
                    new CollectionConfig {CollectionName = "name"}
                }
            };

            // Act / Assert
            Assert.Throws<SettingsBuilderException>(
                () => config.AddDataProviderCollection<object>("name", true, new TestDataProvider()));
        }

        [Fact]
        public void AddDataProviderCollection_NoExistingCollections_AddsCollectionConfig()
        {
            // Arrange
            var config = new DatabaseConfig
            {
                Collections = new List<CollectionConfig>()
            };

            var expected = new DatabaseConfig
            {
                Collections = new List<CollectionConfig>
                {
                    new CollectionConfig
                    {
                        CollectionName = "name",
                        DataFileLocation = null,
                        DropFirst = true,
                        DataProvider = new TestDataProvider()
                    }
                }
            };

            // Act
            var result = config.AddDataProviderCollection<object>("name", true, new TestDataProvider());

            // Assert
            Assert.Equal(expected, result, Comparers.DatabaseConfigComparer());
            Assert.NotNull(result.Collections[0].DataProvider);
            Assert.IsType<TestDataProvider>(result.Collections[0].DataProvider);
        }

        [Fact]
        public void AddDataProviderCollection_ExisitngCollections_AddsAdditionalCollectionConfig()
        {
            // Arrange
            var config = new DatabaseConfig
            {
                Collections = new List<CollectionConfig>
                {
                    new CollectionConfig
                    {
                        CollectionName = "name",
                        DataFileLocation = null,
                        DropFirst = true,
                        DataProvider = new TestDataProvider()
                    }
                }
            };

            var expected = new DatabaseConfig
            {
                Collections = new List<CollectionConfig>
                {
                    new CollectionConfig
                    {
                        CollectionName = "name",
                        DataFileLocation = null,
                        DropFirst = true,
                        DataProvider = new TestDataProvider()
                    },
                    new CollectionConfig
                    {
                        CollectionName = "name-2",
                        DataFileLocation = null,
                        DropFirst = false,
                        DataProvider = new TestDataProvider()
                    }
                }
            };

            // Act
            var result = config.AddDataProviderCollection<object>("name-2", false, new TestDataProvider());

            // Assert
            Assert.Equal(expected, result, Comparers.DatabaseConfigComparer());
        }

        [Fact]
        public void SetValue_NullConfig_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => (null as DatabaseConfig).SetValue(x => x.DropFirst = true));
        }

        [Fact]
        public void SetValue_NullAction_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new DatabaseConfig().SetValue(null));
        }

        [Fact]
        public void SetValue_ValidAction_BehavesAsExpected()
        {
            // Arrange
            var config = new DatabaseConfig
            {
                CollectionNameSuffix = "c_suffix",
                ConnectionString = "conn_string",
                DatabaseName = "db_name",
                DatabaseNameSuffix = "db_suffix",
                DropFirst = false
            };

            var expected = new DatabaseConfig
            {
                CollectionNameSuffix = "c_suffix",
                ConnectionString = "conn_string",
                DatabaseName = "db_name",
                DatabaseNameSuffix = "db_suffix",
                DropFirst = true
            };

            // Act
            var result = config.SetValue(x => x.DropFirst = true);

            // Assert
            Assert.Equal(expected, result, Comparers.DatabaseConfigComparer());
        }

        [Fact]
        public void GetDatabaseName_NullConfig_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => (null as DatabaseConfig).GetDatabaseName());
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GetDatabaseName_ConfigWithNullEmptyOrWhitespaceSuffix_ReturnsDatabaseName(string value)
        {
            // Arrange
            var config = new DatabaseConfig
            {
                DatabaseName = "test",
                DatabaseNameSuffix = value
            };

            var expected = "test";

            // Act
            var result = config.GetDatabaseName();

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("test")]
        [InlineData(" test")]
        [InlineData("-test")]
        public void GetDatabaseName_ConfigWithDatabaseNameSuffix_ReturnsExpectedDatabaseName(string value)
        {
            // Arrange
            var config = new DatabaseConfig
            {
                DatabaseName = "db",
                DatabaseNameSuffix = value
            };

            var expected = $"db{value}";

            // Act
            var result = config.GetDatabaseName();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetCollectionName_NullConfig_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => (null as CollectionConfig).GetCollectionName("suffix"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GetCollectionName_NullEmptyOrWhitespaceSuffix_ReturnsCollectionName(string value)
        {
            // Arrange
            var config = new CollectionConfig
            {
                CollectionName = "collection"
            };

            // Act
            var result = config.GetCollectionName(value);

            // Assert
            Assert.Equal("collection", result);
        }

        [Theory]
        [InlineData("test")]
        [InlineData(" test")]
        [InlineData("-test")]
        public void GetCollectionName_ValidSuffix_ReturnsExpectedCollectionName(string value)
        {
            // Arrange
            var config = new CollectionConfig
            {
                CollectionName = "collection"
            };

            var expected = $"collection{value}";

            // Act
            var result = config.GetCollectionName(value);

            // Assert
            Assert.Equal(expected, result);
        }


        #region Helpers

        private class TestDataProvider : IDataProvider
        {
            public IEnumerable<object> GetData()
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
