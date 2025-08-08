
namespace Catalog.API.Products.GetProductByCategory
{
	public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
	public record GetProductByCategoryResult(IEnumerable<Product> Products);

	internal class GetProductByCategoryQueryHandler(IDocumentSession session) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
	{
		public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(query.Category))
			{
				return new GetProductByCategoryResult(Enumerable.Empty<Product>());
			}
			var products = await session.Query<Product>()
				.Where(p => p.Category.Contains(query.Category))
				.ToListAsync();
			if (!products.Any())
			{
				throw new ProductNotFoundException($"No products found for category: {query.Category}");
			}
			return new GetProductByCategoryResult(products);
		}
	}
}
