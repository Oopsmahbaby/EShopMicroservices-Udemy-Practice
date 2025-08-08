namespace Catalog.API.Products.UpdateProduct
{
	public record UpdateProductRequest(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand;

	public record UpdateProductResponse(bool Success);
	public class UpdateProductEndpoint : ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapPut("/product/update", async (UpdateProductRequest request, ISender sender) =>
			{
				//var command = new UpdateProductCommand(new Product
				//{
				//	Id = Guid.Parse(request.ProductId),
				//	Name = request.Name,
				//	Description = request.Description,
				//	Price = request.Price
				//});
				var command = request.Adapt<UpdateProductCommand>();

				var result = await sender.Send(command);
				var response = result.Adapt<UpdateProductResponse>();
				return Results.Ok(response);
			})
			.WithName("UpdateProduct")
			.Produces<UpdateProductResponse>(StatusCodes.Status200OK)
			.ProducesProblem(StatusCodes.Status400BadRequest)
			.WithSummary("Update Product")
			.WithDescription("Update Product");
		}
	}
}
