using API.Contracts.Orders;
using FluentValidation;
using Models.Entities.enums;

namespace API.Validators.Orders
{
	public class UpdateOrderStatusRequestValidator : AbstractValidator<UpdateOrderStatusRequest>
	{
		private static readonly string AllowedStatuses = string.Join(", ", Enum.GetNames<OrderStatus>());

		public UpdateOrderStatusRequestValidator()
		{
			RuleFor(x => x.Status)
				.NotEmpty().WithMessage("Status is required.")
				.Must(x => Enum.TryParse<OrderStatus>(x, true, out _))
				.WithMessage($"Status must be one of: {AllowedStatuses}.");

			RuleFor(x => x.ChangedByUserId)
				.Must(x => !x.HasValue || x.Value != Guid.Empty)
				.WithMessage("Changed by user ID cannot be empty.");
		}
	}
}
