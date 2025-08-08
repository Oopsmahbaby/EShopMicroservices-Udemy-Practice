namespace Catalog.API.Products.DeleteProduct
{
	public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
	public record DeleteProductResult(bool Success);

	public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
	{
		public DeleteProductCommandValidator()
		{
			RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID cannot be empty.");
		}
	}

	public class DeleteProductCommandHandler(IDocumentSession session) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
	{
		public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
		{
			// Validate the product ID
			if (request.Id == Guid.Empty)
			{
				throw new ArgumentException("Product ID cannot be empty.", nameof(request.Id));
			}

			// Load the existing product from the database
			var existingProduct = await session.LoadAsync<Product>(request.Id);

			// Check if the product exists
			if (existingProduct == null)
			{
				throw new ProductNotFoundException($"Product with ID: {request.Id} not found.");
			}

			// Delete the product
			session.Delete(existingProduct);
			await session.SaveChangesAsync(cancellationToken);

			return new DeleteProductResult(true);
		}
	}
}
