namespace Ordering.Application.Orders.Commands.DeleteOrder
{
	public class DeleteOrderHandler(IApplicationDbContext dbContext) : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
	{
		public async Task<DeleteOrderResult> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
		{
			// Implementation logic to delete an order would go here.
			// Delete the order with the given OrderId from the database.
			// Return the result indicating success or failure.

			var orderId = OrderId.Of(request.OrderId);
			var order = await dbContext.Orders.FindAsync([orderId], cancellationToken);
			if (order == null)
			{
				throw new OrderNotFoundException(request.OrderId);
			}

			dbContext.Orders.Remove(order);
			await dbContext.SaveChangesAsync(cancellationToken);
			return new DeleteOrderResult(true);
		}
	}
}
