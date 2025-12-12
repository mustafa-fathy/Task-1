using Application.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class CacheService :ICacheService
    {
        private readonly IDatabase _db;
        public CacheService(IConnectionMultiplexer rediss)
        {
            _db = rediss.GetDatabase();
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);

            if (value.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(value!);
        }



        public async Task SetAsync<T>(string key, T value, TimeSpan ttl)
        {
            var result = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, result, ttl);
        }
        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }
    }
}

