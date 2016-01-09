using System.IO.Abstractions;
using System.Web.Script.Serialization;

namespace Harness.Settings
{
    internal class SettingsManager : ISettingsManager
    {
        private readonly IFileSystem FileSystem;

        public SettingsManager() : this(new FileSystem())
        {
        }

        public SettingsManager(IFileSystem fileSystem)
        {
            this.FileSystem = fileSystem;
        }

        public MongoConfiguration GetMongoConfiguration(string configFilePath)
        {
            if(string.IsNullOrWhiteSpace(configFilePath))
            {
                return null;
            }

            if(!this.FileSystem.File.Exists(configFilePath))
            {
                return null;
            }

            var json = 
                this.FileSystem.File.ReadAllText(configFilePath);

            return
                new JavaScriptSerializer()
                    .Deserialize<MongoConfiguration>(json);

        }

    }
}
