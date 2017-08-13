using System;
using System.Collections.Generic;
using Harness.Settings;
using MongoDB.Bson.Serialization.Conventions;
using Xunit;

namespace Harness.UnitTests.SettingsTests
{
    public class SettingsBuilderTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void AddDatabase_NullEmptyOrWhitespaceName_ThrowsArgumentNullException(string value)
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SettingsBuilder().AddDatabase(value));
        }

        [Fact]
        public void AddConvention_NullConvention_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SettingsBuilder().AddConvention(null as IConvention, x => true));
        }

        [Fact]
        public void AddConvention_ValidConventionButNullFilter_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SettingsBuilder().AddConvention(new CamelCaseElementNameConvention(), null));
        }

        [Fact]
        public void AddConvention_NullListOfConventions_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SettingsBuilder().AddConvention(null as IList<IConvention>, x => true));
        }

        [Fact]
        public void AddConvention_ValidListOfConventionsButNullFilter_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SettingsBuilder().AddConvention(new List<IConvention> { new CamelCaseElementNameConvention() }, null));
        }

        [Fact]
        public void AddConvention_NullConventionPack_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SettingsBuilder().AddConvention(null as IConventionPack, x => true));
        }

        [Fact]
        public void AddConvention_ValidConventionPackButNullFilter_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SettingsBuilder().AddConvention(new ConventionPack(), null));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void WithConnectionString_NullEmptyOrWhitespaceConnectionString_ThrowsArgumentNullException(string value)
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SettingsBuilder()
                    .AddDatabase("db")
                    .WithConnectionString(value)
            );
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void AddCollection_NullEmptyOrWhitespaceName_ThrowsArgumentNullException(string value)
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SettingsBuilder()
                    .AddDatabase("db")
                    .WithConnectionString("connStr")
                    .AddCollection(value, true, "file")
            );
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void AddCollection_NullEmptyOrWhitespaceLocation_ThrowsArgumentNullException(string value)
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SettingsBuilder()
                    .AddDatabase("db")
                    .WithConnectionString("connStr")
                    .AddCollection("collection", true, value)
            );
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void AddCollection_ValidDataProviderButNullEmptyOrWhitespaceName_ThrowsArgumentNullException(string value)
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SettingsBuilder()
                    .AddDatabase("db")
                    .WithConnectionString("connStr")
                    .AddCollection<int>(value, true, new TestDataProvider())
            );
        }

        [Fact]
        public void AddCollection_NullDataProvider_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new SettingsBuilder()
                    .AddDatabase("db")
                    .WithConnectionString("connStr")
                    .AddCollection<int>("collection", true, null)
            );
        }

        [Fact]
        public void SettingsBuilderTest_NoConventions()
        {
            // Arrange
            var expected = new HarnessConfiguration
            {
                Conventions = null,
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

        [Fact]
        public void SettingsBuilderTest_OneConvention()
        {
            // Arrange
            var expected = new HarnessConfiguration
            {
                Conventions = new ConventionConfig
                {
                    Name = "conventions",
                    ConventionPack = new ConventionPack
                    {
                        new CamelCaseElementNameConvention()
                    },
                    Filter = x => x.Name == "MyClass"
                },
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
                    .AddConvention(new CamelCaseElementNameConvention(), x => x.Name == "MyClass")
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

        [Fact]
        public void SettingsBuilderTest_ListOfConventions()
        {
            // Arrange
            var expected = new HarnessConfiguration
            {
                Conventions = new ConventionConfig
                {
                    Name = "conventions",
                    ConventionPack = new ConventionPack
                    {
                        new CamelCaseElementNameConvention()
                    },
                    Filter = x => x.Namespace == "MyNamespace"
                },
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
                    .AddConvention(new List<IConvention>{new CamelCaseElementNameConvention()}, x => x.Namespace == "MyNamespace")
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

        [Fact]
        public void SettingsBuilderTest_ConventionPack()
        {
            // Arrange
            var expected = new HarnessConfiguration
            {
                Conventions = new ConventionConfig
                {
                    Name = "conventions",
                    ConventionPack = new ConventionPack
                    {
                        new CamelCaseElementNameConvention()
                    },
                    Filter = x => true
                },
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
                    .AddConvention(new ConventionPack{new CamelCaseElementNameConvention()}, x => true)
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

        #region Helpers

        public class TestDataProvider : IDataProvider
        {
            public IEnumerable<object> GetData()
            {
                return new List<object>();
            }
        }

        #endregion
    }
}
