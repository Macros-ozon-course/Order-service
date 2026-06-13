using API.Contracts.Orders;
using FluentValidation;

namespace API.Validators.Orders
{
	public sealed class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
	{
		public CreateOrderRequestValidator()
		{
			RuleFor(x => x.UserId)
				.Must(x => !x.HasValue || x.Value != Guid.Empty)
				.WithMessage("User ID cannot be empty.");

			RuleFor(x => x.Currency)
				.NotEmpty().WithMessage("Currency is required.")
				.Length(3).WithMessage("Currency must contain 3 characters.");

			RuleFor(x => x.TotalAmount)
				.GreaterThan(0).WithMessage("Total amount must be positive.")
				.When(x => x.TotalAmount.HasValue);

			RuleFor(x => x.RecipientName)
				.MaximumLength(200).WithMessage("Recipient name is too long.");

			RuleFor(x => x.RecipientPhone)
				.MaximumLength(50).WithMessage("Recipient phone is too long.");

			RuleFor(x => x.DeliveryAddress)
				.NotEmpty().WithMessage("Delivery address is required.")
				.MaximumLength(500).WithMessage("Delivery address is too long.");

			RuleFor(x => x.Items)
				.NotNull().WithMessage("Order items are required.")
				.NotEmpty().WithMessage("Order items cannot be empty.");

			RuleFor(x => x)
				.Must(x => !x.TotalAmount.HasValue || x.Items.Sum(i => i.Quantity * i.PricePerItem) == x.TotalAmount)
				.When(x => x.Items is not null)
				.WithMessage("Total amount must be equal to sum of order items.");

			RuleForEach(x => x.Items)
				.SetValidator(new CreateOrderItemRequestValidator());
		}
	}
}
