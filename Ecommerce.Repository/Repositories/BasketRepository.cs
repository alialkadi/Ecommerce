using Ecommerce.Core.Entities;
using Ecommerce.Core.Repositories.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Repository.Repositories
{
	public class BasketRepository : IBasketRepository
	{
		private readonly IDatabase _database;

		public BasketRepository(IConnectionMultiplexer redis)
		{
			_database = redis.GetDatabase();
		}
		public async Task<bool> DeleteBasketAsync(string basketId)
		{
			return await _database.KeyDeleteAsync(basketId);
		}

		public async Task<CustomerBasket?> GetCustomerBasketAsync(string basketId)
		{
			var basket = await _database.StringGetAsync(basketId);



			return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket!);
		}

		public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
		{
			var createdOrUpdatedBasket = _database.StringSet(basket.id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(3));

			if(createdOrUpdatedBasket is false) return null;

			return await GetCustomerBasketAsync(basket.id);
		}
	}
}
