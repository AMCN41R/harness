using System.IO.Abstractions;
using MongoDbUnit.Settings;

namespace Harness.Settings
{
    internal class SettingsManager
    {
        private readonly IFileSystem FileSystem;

        public SettingsManager(IFileSystem fileSystem)
        {
            this.FileSystem = fileSystem;
        }

        public IntegrationTestSettings GetSettings(string configFilePath)
        {
            return null;
        }

    }
}
