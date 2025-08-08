using Marten.Schema;

namespace Catalog.API.Data
{
	public class CatalogIntialData : IInitialData
	{
		public async Task Populate(IDocumentStore store, CancellationToken cancellation)
		{
			using var session = store.LightweightSession();

			// Check if the database is empty
			if ( await session.Query<Product>().AnyAsync())
			{
				return;
			}

			// Seed initial data
			// This is a simple example, in a real application you might want to use a more robust seeding strategy
			session.Store<Product>(GetPreconfiguredProducts());
			await session.SaveChangesAsync();
		}

		/// <summary>
		/// Data seeding method to provide initial products.
		/// </summary>
		/// <returns></returns>
		private static IEnumerable<Product> GetPreconfiguredProducts() => new List<Product>()
		{
			new Product
				{
					Id = Guid.NewGuid(),
					Name = "Product 1",
					Category = new List<string> { "Category1", "Category2" },
					Description = "Description for Product 1",
					ImageFile = "image1.jpg",
					Price = 10.99m
				},
				new Product
				{
					Id = Guid.NewGuid(),
					Name = "Product 2",
					Category = new List<string> { "Category2", "Category3" },
					Description = "Description for Product 2",
					ImageFile = "image2.jpg",
					Price = 20.99m
				},
				new Product
				{
					Id = Guid.NewGuid(),
					Name = "Product 3",
					Category = new List<string> { "Category1", "Category3" },
					Description = "Description for Product 3",
					ImageFile = "image3.jpg",
					Price = 30.99m
				}
		};
	}
}
