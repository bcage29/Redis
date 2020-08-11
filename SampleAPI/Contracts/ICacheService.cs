using System;
using System.Threading.Tasks;

namespace SampleAPI.Contracts
{
    public interface ICacheService
    {
        Task<bool> AddAsync(string key, string value, TimeSpan? expiry = default(TimeSpan?));

        Task<string> GetAsync(string key);
    }
}
