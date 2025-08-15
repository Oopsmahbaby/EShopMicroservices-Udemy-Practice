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

	public class StoreBasketHandler(IBasketRepository basketRepository) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
	{
		public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
		{
			ShoppingCart shoppingCart = command.ShoppingCart;

			var result = await basketRepository.StoreBasketAsync(shoppingCart, cancellationToken);

			return new StoreBasketResult(result.UserName);
		}
	}
}
