namespace API.Contracts.Orders
{
	public class UpdateOrderStatusRequest
	{
		public string Status { get; set; } = null!;

		public Guid? ChangedByUserId { get; set; }

		public string? Reason { get; set; }

		public string? Comment { get; set; }
	}
}
