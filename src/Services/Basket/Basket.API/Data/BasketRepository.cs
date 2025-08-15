namespace Basket.API.Data
{
	public class BasketRepository(IDocumentSession session) : IBasketRepository
	{
		/// <summary>
		/// Deletes the basket for the specified user.
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
		{
			session.Delete<ShoppingCart>(userName);
			try
			{
				await session.SaveChangesAsync(cancellationToken);
				return true;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Failed to delete basket for user '{userName}'.", ex);
			}
		}

		/// <summary>
		/// Gets the basket for the specified user.
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="KeyNotFoundException"></exception>
		public async Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken = default)
		{
			var basket = await session.LoadAsync<ShoppingCart>(userName, cancellationToken);

			return basket is null ? throw new BasketNotFoundException(userName)
				: basket;
		}

		/// <summary>
		/// Stores the basket for the specified user.
		/// </summary>
		/// <param name="basket"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart basket, CancellationToken cancellationToken = default)
		{
			session.Store(basket);
			try
			{
				await session.SaveChangesAsync(cancellationToken);
				return basket;
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException($"Failed to store basket for user '{basket.UserName}'.", ex);
			}
		}
	}
}
