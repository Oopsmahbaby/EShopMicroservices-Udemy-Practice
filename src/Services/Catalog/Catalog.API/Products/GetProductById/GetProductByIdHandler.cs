namespace Catalog.API.Products.GetProductById
{
	public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
	public record GetProductByIdResult(Product Product);

	internal class GetProductByIdQueryHandler(IDocumentSession session) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
	{
		public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
		{
			var product = await session.LoadAsync<Product>(query.Id, cancellationToken);
			if (product == null)
			{
				throw new ProductNotFoundException($"Product with ID: {query.Id} not found.");
			}
			return new GetProductByIdResult(product);
		}
	}
}
