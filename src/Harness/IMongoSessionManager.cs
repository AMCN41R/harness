using System.Collections.Generic;
using MongoDB.Driver;

namespace Harness
{
    internal interface IMongoSessionManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Dictionary<string, IMongoClient> Build();
    }
}
