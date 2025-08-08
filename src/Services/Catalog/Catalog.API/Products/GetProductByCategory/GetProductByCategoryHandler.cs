
namespace Catalog.API.Products.GetProductByCategory
{
	public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
	public record GetProductByCategoryResult(IEnumerable<Product> Products);

	internal class GetProductByCategoryQueryHandler(IDocumentSession session, ILogger<GetProductByCategoryQueryHandler> logger) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
	{
		public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
		{
			logger.LogInformation("Handling GetProductByCategoryQuery for category: {Category}", query.Category);
			if (string.IsNullOrWhiteSpace(query.Category))
			{
				logger.LogWarning("Category is null or empty.");
				return new GetProductByCategoryResult(Enumerable.Empty<Product>());
			}
			var products = await session.Query<Product>()
				.Where(p => p.Category.Contains(query.Category))
				.ToListAsync();
			if (!products.Any())
			{
				logger.LogWarning("No products found for category: {Category}", query.Category);
				throw new ProductNotFoundException($"No products found for category: {query.Category}");
			}
			return new GetProductByCategoryResult(products);
		}
	}
}
