using Models.Entities.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
	public class Order
	{
		public Guid Id { get; set; }

		public Guid UserId { get; set; }

		public OrderStatus Status { get; set; } = OrderStatus.Created;

		public decimal TotalAmount { get; set; }

		public string Currency { get; set; } = "KZT";

		public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

		public DateTime? UpdatedAtUtc { get; set; }

		public DateTime? PaidAtUtc { get; set; }

		public DateTime? CollectedAtUtc { get; set; }

		public DateTime? TransferredToDeliveryAtUtc { get; set; }

		public DateTime? DeliveredAtUtc { get; set; }

		public DateTime? CanceledAtUtc { get; set; }

		public string? CancelReason { get; set; }

		public string? Comment { get; set; }

		public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

		public ICollection<OrderStatusHistory> StatusHistory { get; set; } = new List<OrderStatusHistory>();
	}
}
