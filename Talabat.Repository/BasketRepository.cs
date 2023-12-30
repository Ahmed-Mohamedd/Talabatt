using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Repositories;

namespace Talabat.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _redis;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _redis = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasket(string BasketId)
        {
            return await _redis.KeyDeleteAsync(BasketId);
        }

        public async Task<CustomerBasket?> GetBasket(string BasketId)
        {
            var basket = await _redis.StringGetAsync(BasketId);
            if (basket.IsNull) return null;
            var CustomerBasket = JsonSerializer.Deserialize<CustomerBasket>(basket);
            return CustomerBasket;
        }

        public async Task<CustomerBasket?> UpdateBasket(CustomerBasket basket)
        {
            var RedisBasket = JsonSerializer.Serialize(basket);
            var CreateOrUpdate = await _redis.StringSetAsync(basket.Id, RedisBasket, TimeSpan.FromDays(1));
            if (!CreateOrUpdate) return null;
            
            return await GetBasket(basket.Id);

        }
    }
}
