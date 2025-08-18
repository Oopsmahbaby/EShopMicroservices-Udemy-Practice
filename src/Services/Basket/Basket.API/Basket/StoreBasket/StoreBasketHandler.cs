using Discount.Grpc;

namespace Basket.API.Basket.StoreBasket
{
	public record StoreBasketCommand(ShoppingCart ShoppingCart) : ICommand<StoreBasketResult>;
	public record StoreBasketResult(string UserName);

	public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
	{
		public StoreBasketCommandValidator()
		{
			RuleFor(command => command.ShoppingCart)
				.NotNull()
				.WithMessage("Shopping cart cannot be null.");
			RuleFor(command => command.ShoppingCart.UserName)
				.NotEmpty()
				.WithMessage("User name cannot be empty.");
			//RuleFor(command => command.ShoppingCart.Items)
			//	.NotEmpty()
			//	.WithMessage("Shopping cart must contain at least one item.");
		}
	}

	public class StoreBasketHandler(IBasketRepository basketRepository, DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
	{
		public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
		{
			var shoppingCart = command.ShoppingCart;

			// Validate the shopping cart
			if (shoppingCart == null)
			{
				throw new ArgumentNullException(nameof(shoppingCart), "Shopping cart cannot be null.");
			}

			// TODO: communicate with Discount service to recalculate prices if needed
			// Call the discount service to deduct discounts from the shopping cart items
			await DeductDiscount(shoppingCart, cancellationToken);

			var result = await basketRepository.StoreBasketAsync(shoppingCart, cancellationToken);

			return new StoreBasketResult(result.UserName);
		}

		/// <summary>
		/// Deducts discounts from the shopping cart items by calling the Discount service.
		/// </summary>
		/// <param name="shoppingCart"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		private async Task DeductDiscount(ShoppingCart shoppingCart, CancellationToken cancellationToken)
		{
			try
			{
				foreach (var item in shoppingCart.Items)
				{
					// Call the discount service to get the discount for each item
					var discountRequest = new GetDiscountRequest { ProductName = item.ProductName };
					var discountResponse = await discountProtoServiceClient.GetDiscountAsync(discountRequest, cancellationToken: cancellationToken);

					if (discountResponse != null && discountResponse.Amount > 0)
					{
						item.Price -= discountResponse.Amount; // Apply discount
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}
