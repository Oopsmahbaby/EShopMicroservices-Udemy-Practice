
namespace Catalog.API.Products.GetProductByCategory
{
	//public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
	public record GetProductByCategoryResponse(IEnumerable<Product> Products);
	public class GetProductByCategoryEndpoint : ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapGet("/product/category/{category}", async (string category, ISender sender) =>
			{
				var result = await sender.Send(new GetProductByCategoryQuery(category));
				var response = result.Adapt<GetProductByCategoryResponse>();
				return Results.Ok(response);
			})
			.WithName("GetProductByCategory")
			.Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
			.ProducesProblem(StatusCodes.Status404NotFound)
			.WithSummary("Get Products By Category")
			.WithDescription("Get Products By Category");
		}
	}
}
