using StackExchange.Redis;
using System;

namespace SampleAPI.Contracts
{
    public interface IRedisFactory : IDisposable
    {
        IDatabase GetCache();

        void ForceReconnect();
    }
}
