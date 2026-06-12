namespace API.Contracts.Orders
{
	public class CancelOrderRequest
	{
		public string Reason { get; set; } = null!;

		public string? Comment { get; set; }
	}
}
