using Microsoft.Extensions.Logging;
using SampleAPI.Contracts;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace SampleAPI.Services
{
    public class CacheService : ICacheService
    {
        private readonly ILogger<CacheService> _logger;
        private readonly IRedisFactory _redisFactory;
        
        public CacheService(IRedisFactory redisFactory, ILogger<CacheService> logger)
        {
            _redisFactory = redisFactory;
            _logger = logger;
        }

        public async Task<bool> AddAsync(string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            try
            {
                var redis = _redisFactory.GetCache();
                if (redis == null) return false;

                _logger.LogInformation($"Adding value to redis {key}");
                return await redis.StringSetAsync(key, value, expiry);
            }
            catch (RedisConnectionException ex)
            {
                _redisFactory.ForceReconnect();
                _logger.LogError(ex, "Redis Connection Error");
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Redis Add Error for - {key}");
                throw;
            }
        }

        public async Task<string> GetAsync(string key)
        {
            var redis = _redisFactory.GetCache();
            if (redis == null) return null;

            try
            {
                return await redis.StringGetAsync(key);
            }
            catch (RedisConnectionException ex)
            {
                _redisFactory.ForceReconnect();
                _logger.LogError(ex, "Redis Connection Error");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Redis Get Error for - {key}");
                throw;
            }
        }
    }
}
