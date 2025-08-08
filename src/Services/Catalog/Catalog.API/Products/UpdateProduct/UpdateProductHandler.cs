
namespace Catalog.API.Products.UpdateProduct
{
	public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductResult>;
	public record UpdateProductResult(bool Success);

	public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
	{
		public UpdateProductCommandValidator()
		{
			RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID cannot be empty.");
			RuleFor(x => x.Name).NotEmpty().WithMessage("Product name cannot be empty.")
				.Length(2, 150).WithMessage("Name must be between 2 and 150 characters");
			RuleFor(x => x.Category).NotEmpty().WithMessage("Product category cannot be empty.");
			RuleFor(x => x.Description).NotEmpty().WithMessage("Product description cannot be empty.");
			RuleFor(x => x.ImageFile).NotEmpty().WithMessage("Product image file cannot be empty.");
			RuleFor(x => x.Price).GreaterThan(0).WithMessage("Product price must be greater than zero.");
		}
	}
	internal class UpdateProductCommandHandler(IDocumentSession session) : ICommandHandler<UpdateProductCommand, UpdateProductResult>
	{
		public async Task<UpdateProductResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
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

			// Update the product details
			existingProduct.Name = request.Name;
			existingProduct.Category = request.Category;
			existingProduct.Description = request.Description;
			existingProduct.ImageFile = request.ImageFile;
			existingProduct.Price = request.Price;

			// Save the updated product back to the database
			session.Update(existingProduct);

			await session.SaveChangesAsync(cancellationToken);

			return new UpdateProductResult(true);
		}
	}
}
