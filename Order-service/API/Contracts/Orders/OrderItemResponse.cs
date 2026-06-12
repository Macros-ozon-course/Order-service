namespace API.Contracts.Orders
{
	public class OrderItemResponse
	{
		public Guid Id { get; set; }

		public Guid OrderId { get; set; }

		public Guid ProductId { get; set; }

		public string ProductName { get; set; } = null!;

		public string? ProductImageUrl { get; set; }

		public string? Sku { get; set; }

		public int Quantity { get; set; }

		public decimal PricePerItem { get; set; }

		public decimal TotalPrice { get; set; }

		public Guid SellerId { get; set; }

		public DateTime CreatedAtUtc { get; set; }
	}
}
