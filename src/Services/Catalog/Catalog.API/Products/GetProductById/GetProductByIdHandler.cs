namespace Catalog.API.Products.GetProductById
{
	public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
	public record GetProductByIdResult(Product Product);

	internal class GetProductByIdQueryHandler(IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
	{
		public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
		{
			logger.LogInformation("Handling GetProductByIdQuery for product with ID: {ProductId}", query.Id);
			var product = await session.LoadAsync<Product>(query.Id, cancellationToken);
			if (product == null)
			{
				logger.LogWarning("Product with ID: {ProductId} not found", query.Id);
				throw new ProductNotFoundException($"Product with ID: {query.Id} not found.");
			}
			return new GetProductByIdResult(product);
		}
	}
}
