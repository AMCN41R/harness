using Harness.FileSystem;
using MongoDbUnit.Settings;

namespace Harness.Settings
{
    internal class SettingsManager
    {
        private readonly IFileSystemHelper FileSystem;

        public SettingsManager(IFileSystemHelper fileSystem)
        {
            this.FileSystem = fileSystem;
        }

        public IntegrationTestSettings GetSettings(string configFilePath)
        {
            return null;
        }

    }
}
