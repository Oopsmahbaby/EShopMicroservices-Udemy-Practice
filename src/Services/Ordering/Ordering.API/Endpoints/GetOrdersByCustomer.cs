using Ordering.Application.Orders.Queries.GetOrdersByCustomer;

namespace Ordering.API.Endpoints
{

	//public record GetOrderByCustomerRequest(Guid CustomerId);
	public record class GetOrderByCustomerResponse(IEnumerable<OrderDto> Orders);

	public class GetOrdersByCustomer : ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapGet("/orders/customer/{customerId}", async (Guid customerId, ISender sender) =>
			{
				var result = await sender.Send(new GetOrdersByCustomerQuery(customerId));

				var response = result.Adapt<GetOrderByCustomerResponse>();

				return Results.Ok(response);
			})
			.WithName("GetOrdersByCustomer")
			.Produces<GetOrderByCustomerResponse>(StatusCodes.Status200OK)
			.ProducesProblem(StatusCodes.Status400BadRequest)
			.WithSummary("Get orders by customer id")
			.WithDescription("Get orders by customer id");
		}
	}
}
