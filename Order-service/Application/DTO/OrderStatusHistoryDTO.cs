using Models.Entities.enums;

namespace Application.DTO
{
	public class OrderStatusHistoryDTO
	{
		public Guid Id { get; set; }

		public Guid OrderId { get; set; }

		public OrderStatus? OldStatus { get; set; }

		public OrderStatus NewStatus { get; set; }

		public DateTime ChangedAtUtc { get; set; }

		public Guid? ChangedByUserId { get; set; }

		public string? Reason { get; set; }

		public string? Comment { get; set; }
	}
}
