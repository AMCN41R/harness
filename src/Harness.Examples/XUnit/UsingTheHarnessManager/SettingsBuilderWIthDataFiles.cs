using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harness.Settings;
using Xunit;

namespace Harness.Examples.XUnit.UsingTheHarnessManager
{
    public class SettingsBuilderWIthDataFiles
    {
        public SettingsBuilderWIthDataFiles()
        {
            var settings =
                new SettingsBuilder()
                    .AddDatabase("TestDb1")
                    .WithConnectionString("mongodb://localhost:27017")
                    .DropDatabaseFirst()
                    .AddCollection("col1", true, "Collection1.json")
                    .AddCollection("col2", true, "Collection2.json")
                    .Build();

            new HarnessManager()
                .UsingSettings(settings)
                .Build();
        }

        [Fact]
        public void Test1()
        {
            // Arrange
            var classUnderTest = new ClassUnderTest();

            // Act
            var result = classUnderTest.GetCollectionRecordCount("col1");

            // Assert
            Assert.Equal(2, result);
        }
    }
}
