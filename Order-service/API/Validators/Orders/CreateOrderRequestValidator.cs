using API.Contracts.Orders;
using FluentValidation;

namespace API.Validators.Orders
{
	public sealed class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
	{
		public CreateOrderRequestValidator()
		{
			RuleFor(x => x.Currency)
				.NotEmpty().WithMessage("Currency is required.");

			RuleFor(x => x.TotalAmount)
				.NotEmpty().WithMessage("Total amount is required.")
				.GreaterThan(0).WithMessage("Total amount must be positive.");

			RuleFor(x => x.Items)
				.NotNull().WithMessage("Order items are required.")
				.NotEmpty().WithMessage("Order items cannot be empty.");

			RuleFor(x => x)
				.Must(x => x.Items.Sum(i => i.TotalPrice) == x.TotalAmount)
				.WithMessage("Total amount must be equal to sum of order items.");

			RuleForEach(x => x.Items)
				.SetValidator(new CreateOrderItemRequestValidator());
		}
	}
}
