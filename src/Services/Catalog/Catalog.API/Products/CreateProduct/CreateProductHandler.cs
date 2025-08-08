namespace Catalog.API.Products.CreateProduct
{
	public record CreateProductCommand(
		string Name,
		List<string> Category,
		string Description,
		string ImageFile,
		decimal Price) : ICommand<CreateProductResult>;

	public record CreateProductResult(Guid Id,
		string Name,
		List<string> Category,
		string Description,
		string ImageFile,
		decimal Price);

	public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
	{
		public CreateProductCommandValidator()
		{
			RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
			RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required.");
			RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
			RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required.");
			RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
		}
	}

	internal class CreateProductCommandHandler(IDocumentSession session, IValidator<CreateProductCommand> validator) : ICommandHandler<CreateProductCommand, CreateProductResult>
	{
		/// <summary>
		/// Create Prduct entity from command object and return the result.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
		{
			// Validate the command
			var result = await validator.ValidateAsync(command, cancellationToken);

			// If validation fails, throw an exception with the validation errors
			if (!result.IsValid)
			{
				Console.WriteLine("Validation failed:");
				foreach (var error in result.Errors)
				{
					Console.WriteLine($"- {error.ErrorMessage}");
				}
				throw new ValidationException(result.Errors);
			}

			// Create a new Product entity from the command
			var product = new Product
			{
				Name = command.Name,
				Category = command.Category,
				Description = command.Description,
				ImageFile = command.ImageFile,
				Price = command.Price
			};

			// TODO: Save the product to the database or any other storage mechanism here.
			try
			{
				session.Store(product);
				await session.SaveChangesAsync(cancellationToken);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error saving to DB: {ex.Message}");
				throw;
			}

			// Log the creation of the product
			var createProductResult = new CreateProductResult(
				product.Id,
				product.Name,
				product.Category,
				product.Description,
				product.ImageFile,
				product.Price);

			return createProductResult;
		}
	}
}
