using System.Collections.Generic;
using Harness.Settings;
using Xunit;

namespace Harness.UnitTests.SettingsTests
{
    public class SettingsBuilderTests
    {
        [Fact]
        public void SettingsBuilderTest()
        {
            // Arrange
            var expected = new HarnessConfiguration
            {
                Databases = new List<DatabaseConfig>
                {
                    new DatabaseConfig
                    {
                        CollectionNameSuffix = "CS-1",
                        ConnectionString = "connString-1",
                        DatabaseName = "DB-1",
                        DropFirst = true,
                        Collections = new List<CollectionConfig>
                        {
                            new CollectionConfig
                            {
                                CollectionName = "C-1",
                                DataFileLocation = "file-1",
                                DropFirst = true
                            },
                            new CollectionConfig
                            {
                                CollectionName = "C-2",
                                DataFileLocation = "file-2",
                                DropFirst = true
                            }
                        }
                    },
                    new DatabaseConfig
                    {
                        CollectionNameSuffix = "CS-2",
                        ConnectionString = "connString-2",
                        DatabaseName = "DB-2",
                        DropFirst = true,
                        Collections = new List<CollectionConfig>
                        {
                            new CollectionConfig
                            {
                                CollectionName = "C-3",
                                DataFileLocation = "file-3",
                                DropFirst = true
                            },
                            new CollectionConfig
                            {
                                CollectionName = "C-4",
                                DataFileLocation = "file-4",
                                DropFirst = true
                            }
                        }
                    }
                }
            };

            // Act
            var result =
                new SettingsBuilder()
                    .AddDatabase("DB-1")
                    .WithConnectionString("connString-1")
                    .WithCollectionNameSuffix("CS-1")
                    .DropDatabaseFirst()
                    .AddCollection("C-1", true, "file-1")
                    .AddCollection("C-2", true, "file-2")
                    .AddDatabase("DB-2")
                    .WithConnectionString("connString-2")
                    .WithCollectionNameSuffix("CS-2")
                    .DropDatabaseFirst()
                    .AddCollection("C-3", true, "file-3")
                    .AddCollection("C-4", true, "file-4")
                    .Build();

            // Assert
            Assert.Equal(expected, result, Comparers.HarnessConfigurationComparer());
        }
    }
}
