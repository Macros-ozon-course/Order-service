namespace API.Contracts.Orders
{
	public class CancelOrderResponse
	{
		public Guid Id { get; set; }

		public long OrderNumber { get; set; }

		public string Status { get; set; } = null!;

		public string StatusText { get; set; } = null!;

		public DateTime CanceledAtUtc { get; set; }
	}
}
