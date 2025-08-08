
namespace Catalog.API.Products.DeleteProduct
{
	//public record DeleteProductRequest(Guid ProductId);
	public record DeleteProductResponse(bool Success);
	public class DeleteProductEndpoint : ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapDelete("/product/delete/{productId}", async (Guid productId, ISender sender) =>
				{
					var result = await sender.Send(new DeleteProductCommand(productId));
					var response = result.Adapt<DeleteProductResponse>();
					return Results.Ok(response);
				})
				.WithName("DeleteProduct")
				.Produces<DeleteProductResponse>(StatusCodes.Status200OK)
				.ProducesProblem(StatusCodes.Status404NotFound)
				.WithSummary("Delete Product")
				.WithDescription("Delete Product");
		}
	}
}
