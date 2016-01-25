using System.Collections.Generic;
using Harness.Settings;
using MongoDB.Driver;

namespace Harness
{
    public interface IHarnessManager
    {
        IHarnessManager Using(string filepath);

        IHarnessManager Using(MongoConfiguration configuration);

        Dictionary<string, IMongoClient> Build();
    }
}
