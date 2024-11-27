using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Shopping.Core.IServices;

namespace Shopping.Service.CacheService
{
    public class CacheResponseService : ICacheResponseService
    {
        private readonly IDatabase _database;
        public CacheResponseService(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }
        public async Task CachResponse(string Key, object response, TimeSpan timeToLive)
        {
            if (response is null)
                return;
            var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var responseJson =  JsonSerializer.Serialize(response,options);
             await _database.StringSetAsync(Key, responseJson, timeToLive);
        }

        public async Task<string?> GetCachResponse(string Key)
        {
            var response = await _database.StringGetAsync(Key);
            if (response.IsNullOrEmpty)
                return null;
            return response;

        }
    }

    
    
}
