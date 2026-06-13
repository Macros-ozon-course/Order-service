namespace API.Contracts.Orders
{
	public class CreateOrderRequest
	{
		public Guid? UserId { get; set; }

		public decimal? TotalAmount { get; set; }

		public string? Currency { get; set; } = "RUB";

		public string? RecipientName { get; set; }

		public string? RecipientPhone { get; set; }

		public string DeliveryAddress { get; set; } = null!;

		public List<CreateOrderItemRequest> Items { get; set; } = new List<CreateOrderItemRequest>();
	}
}
