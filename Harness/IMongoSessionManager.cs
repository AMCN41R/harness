using System;

namespace Harness
{
    internal interface IMongoSessionManager : IDisposable
    {
        void Build();
        void SaveOutput();
    }
}
