using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Services;

namespace Talabat.Service
{
    public class ResponseCachService: IResponseCachService
    {
        private readonly IDatabase _database;

        public ResponseCachService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task CacheResponseAsync(string key, object response, TimeSpan timeToLife)
        {
            if (response == null) return;

            var options = new JsonSerializerOptions() { PropertyNamingPolicy= JsonNamingPolicy.CamelCase };

            var jsonSerializer = JsonSerializer.Serialize(response, options);

            await _database.StringSetAsync(key, jsonSerializer, timeToLife);
        }

        public async Task<string> GetCacheResponseAsync(string cacheKey)
        {
            var cachedResponse = await _database.StringGetAsync(cacheKey);

            if (cachedResponse.IsNullOrEmpty) return null;

            return cachedResponse;
        }
    }
}
