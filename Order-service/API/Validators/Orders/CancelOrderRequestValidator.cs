using API.Contracts.Orders;
using FluentValidation;

namespace API.Validators.Orders
{
	public class CancelOrderRequestValidator : AbstractValidator<CancelOrderRequest>
	{
		public CancelOrderRequestValidator()
		{
			RuleFor(x => x.Reason)
				.NotEmpty().WithMessage("Cancel reason is required.");
		}
	}
}
