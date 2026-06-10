using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
	public class CreateOrderItemDTO
	{
		public Guid ProductId { get; set; }
		public string ProductName { get; set; } = null!;
		public string? ProductImageUrl { get; set; }
		public string? Sku { get; set; }
		public int Quantity { get; set; }
		public decimal PricePerItem { get; set; }
		public decimal TotalPrice { get; set; }
		public Guid SellerId { get; set; }
	}
}
