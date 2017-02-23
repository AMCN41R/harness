using System;
using Harness.Attributes;
using Xunit;

namespace Harness.Examples.XUnit.UsingTheHarnessBaseClass
{
    [HarnessConfig(ConfigFilePath = "ExampleSettings.json")]
    public class DatabaseFixture : HarnessBase, IDisposable
    {
        public DatabaseFixture()
        {
            // Any other setup stuff...
        }

        public void Dispose()
        {
            // Any cleaning up...
        }
    }

    [Collection("Example.Tests")]
    public class UsingTheBaseClassWithClassFixture : IClassFixture<DatabaseFixture>
    {
        private DatabaseFixture Fixture { get; }

        public UsingTheBaseClassWithClassFixture(DatabaseFixture fixture)
        {
            this.Fixture = fixture;
        }

        [Fact]
        public void Test1()
        {
            // Arrange
            var classUnderTest = new ClassUnderTest();

            // Act
            var result = classUnderTest.GetCollectionRecordCount("TestCollection1");

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public void Test2()
        {
            // Arrange
            var classUnderTest = new ClassUnderTest();
            var mongoClient = this.Fixture.MongoConnections["mongodb://localhost:27017"];

            // Act
            var result = classUnderTest.GetCollectionRecordCount(mongoClient, "TestCollection1");

            // Assert
            Assert.Equal(2, result);
        }
    }
}
