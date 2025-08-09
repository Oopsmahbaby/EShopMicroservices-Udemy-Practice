using Marten.Pagination;

namespace Catalog.API.Products.GetProducts
{
	public record GetProductQuery(int? PageNumber = 1, int? PageSize = 5) : IQuery<GetProductsResult>;

	public record GetProductsResult(IEnumerable<Product> Products);

	internal class GetProductsQueryHandler
		(IDocumentSession session) : IQueryHandler<GetProductQuery, GetProductsResult>
	{
		public async Task<GetProductsResult> Handle(GetProductQuery query, CancellationToken cancellationToken)
		{
			var products = await session.Query<Product>()
				.ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);
			return new GetProductsResult(products);
		}
	}
}
