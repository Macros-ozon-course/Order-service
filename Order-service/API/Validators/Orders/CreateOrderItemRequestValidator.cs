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
				.NotEmpty().WithMessage("Product name is required.")
				.MaximumLength(300).WithMessage("Product name is too long.");

			RuleFor(x => x.ProductImageUrl)
				.MaximumLength(1000).WithMessage("Product image URL is too long.");

			RuleFor(x => x.Sku)
				.MaximumLength(100).WithMessage("SKU is too long.");

			RuleFor(x => x.Quantity)
				.GreaterThan(0).WithMessage("Quantity must be greater than zero.");

			RuleFor(x => x.PricePerItem)
				.GreaterThan(0).WithMessage("Price per item must be greater than zero.");

			RuleFor(x => x.TotalPrice)
				.GreaterThan(0).WithMessage("Total price must be greater than zero.")
				.When(x => x.TotalPrice.HasValue);

			RuleFor(x => x.SellerId)
				.NotEmpty().WithMessage("Seller ID is required.");

			RuleFor(x => x)
				.Must(x => !x.TotalPrice.HasValue || x.TotalPrice == x.Quantity * x.PricePerItem)
				.WithMessage("Total price must be equal to quantity multiplied by price per item.");
		}
	}
}
