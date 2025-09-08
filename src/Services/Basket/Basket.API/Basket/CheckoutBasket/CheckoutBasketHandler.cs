
using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.API.Basket.CheckoutBasket
{
	/// <summary>
	/// Checkout Basket Command
	/// </summary>
	/// <param name="BasketCheckoutDto"></param>
	public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto) : ICommand<CheckoutBasketResult>;
	public record CheckoutBasketResult(bool IsSuccess);

	public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
	{
		public CheckoutBasketCommandValidator()
		{
			RuleFor(command => command.BasketCheckoutDto)
				.NotNull()
				.WithMessage("Basket checkout data cannot be null.");
			RuleFor(command => command.BasketCheckoutDto.UserName)
				.NotEmpty()
				.WithMessage("User name cannot be empty.");
		}
	}

	/// <summary>
	/// CheckoutBasketCommandHandler
	/// </summary>
	/// <param name="repository"></param>
	/// <param name="publishEndpoint"></param>
	public class CheckoutBasketCommandHandler(IBasketRepository repository, IPublishEndpoint publishEndpoint) : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
	{
		public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
		{
			// Get existing basket with total price
			// Create basket checkout event -- Set TotalPrice on basketCheckoutEventMessage
			// Send checkout event to rabbitmq using MassTransit
			// Remove the basket

			try
			{
				var basket = await repository.GetBasketAsync(command.BasketCheckoutDto.UserName, cancellationToken);
				if (basket is null)
				{
					return new CheckoutBasketResult(false);
				}

				var eventMessage = command.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();

				// set total price on event message
				eventMessage.TotalPrice = basket.TotalPrice;

				// send checkout event to rabbitmq
				await publishEndpoint.Publish(eventMessage, cancellationToken);

				// remove the basket
				await repository.DeleteBasketAsync(basket.UserName, cancellationToken);

				return new CheckoutBasketResult(true);
			}
			catch (Exception ex)
			{
				// Log the exception (you can use a logging framework here)
				Console.WriteLine($"An error occurred during checkout: {ex.Message}");
				return new CheckoutBasketResult(false);

			}
		}
	}
}
