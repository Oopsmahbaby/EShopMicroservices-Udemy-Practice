namespace Basket.API.Data
{
	public class CachedBasketRepository(IBasketRepository basketRepository, IDistributedCache cache) : IBasketRepository
	{
		/// <summary>
		/// Deletes the basket for the specified user.
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
		{
			await basketRepository.DeleteBasketAsync(userName, cancellationToken);

			// Remove the basket from the cache
			await cache.RemoveAsync(userName, cancellationToken);

			return true;
		}

		/// <summary>
		/// Gets the basket for the specified user.
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken = default)
		{
			// Check if the basket is cached
			var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);

			// If cached basket is found, deserialize and return it
			if (!string.IsNullOrEmpty(cachedBasket))
			{
				// Deserialize the cached basket
				return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
			}

			var basket = await basketRepository.GetBasketAsync(userName, cancellationToken);

			// Cache the basket for future requests
			await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), new DistributedCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60) // Set cache expiration time
			}, cancellationToken);

			return basket;
		}

		/// <summary>
		/// Stores the basket for the specified user.
		/// </summary>
		/// <param name="basket"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart basket, CancellationToken cancellationToken = default)
		{
			await basketRepository.StoreBasketAsync(basket, cancellationToken);

			// Cache the basket after storing it
			await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), new DistributedCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60) // Set cache expiration time
			}, cancellationToken);

			return basket;
		}
	}
}
