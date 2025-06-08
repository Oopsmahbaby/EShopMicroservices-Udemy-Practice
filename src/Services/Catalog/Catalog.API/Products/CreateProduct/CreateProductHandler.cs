using Catalog.API.Models;

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

	internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
	{
		/// <summary>
		/// Create Prduct entity from command object and return the result.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
		{
			var result = new Product
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
				session.Store(result);
				await session.SaveChangesAsync(cancellationToken);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error saving to DB: {ex.Message}");
				throw;
			}

			var createProductResult = new CreateProductResult(
				result.Id,
				result.Name,
				result.Category,
				result.Description,
				result.ImageFile,
				result.Price);

			return createProductResult;
		}
	}
}
