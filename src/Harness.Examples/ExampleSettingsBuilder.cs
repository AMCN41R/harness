using Harness.Settings;

namespace Harness.Examples
{
    public class ExampleSettingsBuilder
    {
        public HarnessConfiguration GetSettings()
        {
            return
                new SettingsBuilder()
                    .AddDatabase("test")
                    .WithConnectionString("conn")
                    .DropDatabaseFirst()
                    .AddCollection("col1", true, "path1")
                    .AddCollection("col2", true, "path2")
                    .AddAnotherDatabase("test2")
                    .WithConnectionString("conn2")
                    .WithDatabaseNameSuffix("test")
                    .DropDatabaseFirst()
                    .AddCollection("col1", true, "path1")
                    .AddCollection("col2", true, "path2")
                    .Build();


        }
    }
}