namespace Harness.Settings
{
    public interface ISettingsManager
    {
        MongoConfiguration GetMongoConfiguration(string configFilePath);
    }
}
