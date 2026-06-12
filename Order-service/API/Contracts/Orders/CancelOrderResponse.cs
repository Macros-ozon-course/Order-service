namespace API.Contracts.Orders
{
	public class CancelOrderResponse
	{
		public Guid Id { get; set; }

		public string Status { get; set; } = null!;

		public DateTime CanceledAtUtc { get; set; }
	}
}
