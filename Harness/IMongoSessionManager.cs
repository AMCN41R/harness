using System.Collections.Generic;
using MongoDB.Driver;

namespace Harness
{
    internal interface IMongoSessionManager
    {
        Dictionary<string, IMongoClient> Build();
        void SaveOutput();
    }
}
