
namespace Catalog.API.Products.UpdateProduct
{
	public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductResult>;
	public record UpdateProductResult(bool Success);
	internal class UpdateProductCommandHandler(IDocumentSession session, ILogger<UpdateProductCommandHandler> logger) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
	{
		public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
		{
			logger.LogInformation("Handling UpdateProductCommand for product with ID: {ProductId}", request.Id);

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

			// Update the product details
			logger.LogInformation("Updating product with ID: {ProductId}", request.Id);
			existingProduct.Name = request.Name;
			existingProduct.Category = request.Category;
			existingProduct.Description = request.Description;
			existingProduct.ImageFile = request.ImageFile;
			existingProduct.Price = request.Price;

			// Save the updated product back to the database
			session.Update(existingProduct);

			await session.SaveChangesAsync(cancellationToken);

			// Log the successful update
			logger.LogInformation("Product with ID: {ProductId} updated successfully", request.Id);
			return new UpdateProductResult(true);
		}
	}
}
