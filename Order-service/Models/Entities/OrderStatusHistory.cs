using Models.Entities.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
	public class OrderStatusHistory
	{
		public Guid Id { get; set; }

		public Guid OrderId { get; set; }

		public OrderStatus? OldStatus { get; set; }

		public OrderStatus NewStatus { get; set; }

		public DateTime ChangedAtUtc { get; set; } = DateTime.UtcNow;

		public Guid? ChangedByUserId { get; set; }

		public string? Reason { get; set; }

		public string? Comment { get; set; }

		public Order Order { get; set; } = null!;
	}
}
