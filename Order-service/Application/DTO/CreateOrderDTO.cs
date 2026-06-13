namespace Application.DTO
{
	public class CreateOrderDTO
	{
		public decimal? TotalAmount { get; set; }

		public string? Currency { get; set; }

		public string? RecipientName { get; set; }

		public string? RecipientPhone { get; set; }

		public string DeliveryAddress { get; set; } = null!;

		public List<CreateOrderItemDTO> Items { get; set; } = new List<CreateOrderItemDTO>();
	}
}
