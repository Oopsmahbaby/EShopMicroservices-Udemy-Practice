namespace Catalog.API.Products.DeleteProduct
{
	public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
	public record DeleteProductResult(bool Success);
	public class DeleteProductCommandHandler(IDocumentSession session, ILogger<DeleteProductCommandHandler> logger) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
	{
		public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
		{
			logger.LogInformation("Handling DeleteProductCommand for product with ID: {ProductId}", request.Id);

			// Validate the product ID
			if (request.Id == Guid.Empty)
			{
				logger.LogWarning("Product ID is empty.");
				throw new ArgumentException("Product ID cannot be empty.", nameof(request.Id));
			}

			// Load the existing product from the database
			var existingProduct = await session.LoadAsync<Product>(request.Id);

			// Check if the product exists
			if (existingProduct == null)
			{
				logger.LogWarning("Product with ID: {ProductId} not found", request.Id);
				throw new ProductNotFoundException($"Product with ID: {request.Id} not found.");
			}

			// Delete the product
			logger.LogInformation("Deleting product with ID: {ProductId}", request.Id);
			session.Delete(existingProduct);
			await session.SaveChangesAsync(cancellationToken);

			// Log the successful deletion
			logger.LogInformation("Product with ID: {ProductId} deleted successfully", request.Id);
			return new DeleteProductResult(true);
		}
	}
}
