using API.Contracts.Orders;
using FluentValidation;

namespace API.Validators.Orders
{
	public class CreateOrderItemRequestValidator : AbstractValidator<CreateOrderItemRequest>
	{
		public CreateOrderItemRequestValidator()
		{
			RuleFor(x => x.ProductId)
				.NotEmpty().WithMessage("Product ID is required.");

			RuleFor(x => x.ProductName)
				.NotEmpty().WithMessage("Product name is required.");

			RuleFor(x => x.Quantity)
				.NotEmpty().WithMessage("Quantity is required.")
				.GreaterThan(0).WithMessage("Quantity must be greater than zero.");

			RuleFor(x => x.PricePerItem)
				.NotEmpty().WithMessage("Price per item is required.")
				.GreaterThan(0).WithMessage("Price per item must be greater than zero.");

			RuleFor(x => x.TotalPrice)
				.NotEmpty().WithMessage("Total price is required.")
				.GreaterThan(0).WithMessage("Total price must be greater than zero.");

			RuleFor(x => x.SellerId)
				.NotEmpty().WithMessage("Seller ID is required.");

			RuleFor(x => x)
				.Must(x => x.TotalPrice == x.Quantity * x.PricePerItem)
				.WithMessage("Total price must be equal to quantity multiplied by price per item.");
		}
	}
}
