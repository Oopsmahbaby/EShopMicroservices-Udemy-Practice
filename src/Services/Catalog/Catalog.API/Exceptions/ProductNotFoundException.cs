using BuildingBlocks.Exceptions;

namespace Catalog.API.Exceptions
{
	public class ProductNotFoundException : NotFoundException
	{
		public ProductNotFoundException(Guid productId)
			: base($"Product with ID: {productId} not found.")
		{
		}
		public ProductNotFoundException(string message)
			: base(message)
		{
		}
		public ProductNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
