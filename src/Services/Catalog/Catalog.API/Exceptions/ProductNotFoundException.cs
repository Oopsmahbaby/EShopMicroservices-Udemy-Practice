namespace Catalog.API.Exceptions
{
	public class ProductNotFoundException : Exception
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
