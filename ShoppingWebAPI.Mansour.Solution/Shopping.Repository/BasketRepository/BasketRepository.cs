using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Shopping.Core.IRepository;
using Shopping.Core.Models.Basket;


namespace Shopping.Repository.BasketRepository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var basket = await _database.StringGetAsync(basketId);
            if (basket.IsNullOrEmpty)
                return null;
            else
            return JsonSerializer.Deserialize<CustomerBasket>(basket);
        }
        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
          var basketJson =  JsonSerializer.Serialize(basket);
           var result = await _database.StringSetAsync(basket.Id, basketJson,TimeSpan.FromDays(3));
            if (result)
                return await GetBasketAsync(basket.Id);
            else
                return null;
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }


    }
}
