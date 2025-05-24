using StackExchange.Redis;
using System.Text.Json;

namespace MovieAPI.Services.ResponseCache
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _database;

        public ResponseCacheService(IConnectionMultiplexer redius)
        {
            _database = redius.GetDatabase();
        }

        public async Task CacheResponseAsync(string Key, object Response, TimeSpan timeToLive)
        {
            if (Response is null) return;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            var json = JsonSerializer.Serialize(Response, options);

            await _database.StringSetAsync(Key, json, timeToLive);
        }

        public async Task<string?> GetCachedResponseAsync(string Key)
        {
            var res = await _database.StringGetAsync(Key);
            if (res.IsNullOrEmpty) return null;
            return res;
        }
    }
}