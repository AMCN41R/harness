using System.IO.Abstractions;
using System.Web.Script.Serialization;

namespace Harness.Settings
{
    public class SettingsManager : ISettingsManager
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
            var jss = new JavaScriptSerializer();

            var json = 
                this.FileSystem.File.ReadAllText(configFilePath);

            return 
                jss.Deserialize<MongoConfiguration>(json);

        }

    }
}
