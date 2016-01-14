namespace Harness.Settings
{
    internal interface ISettingsManager
    {
        MongoConfiguration GetMongoConfiguration(string configFilePath);
    }
}
