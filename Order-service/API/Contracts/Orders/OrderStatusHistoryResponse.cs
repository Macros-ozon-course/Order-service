namespace API.Contracts.Orders
{
	public class OrderStatusHistoryResponse
	{
		public Guid Id { get; set; }

		public Guid OrderId { get; set; }

		public string? OldStatus { get; set; }

		public string NewStatus { get; set; } = null!;

		public DateTime ChangedAtUtc { get; set; }

		public Guid? ChangedByUserId { get; set; }

		public string? Reason { get; set; }

		public string? Comment { get; set; }
	}
}
