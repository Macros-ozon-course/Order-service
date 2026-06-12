using Models.Entities.enums;
using System;
using System.Collections.Generic;

namespace Application.DTO
{
	public class OrderDTO
	{
		public Guid Id { get; set; }

		public Guid UserId { get; set; }

		public OrderStatus Status { get; set; }

		public decimal TotalAmount { get; set; }

		public string Currency { get; set; } = null!;

		public DateTime CreatedAtUtc { get; set; }

		public DateTime? UpdatedAtUtc { get; set; }

		public DateTime? PaidAtUtc { get; set; }

		public DateTime? CollectedAtUtc { get; set; }

		public DateTime? TransferredToDeliveryAtUtc { get; set; }

		public DateTime? DeliveredAtUtc { get; set; }

		public DateTime? CanceledAtUtc { get; set; }

		public string? CancelReason { get; set; }

		public string? Comment { get; set; }

		public List<OrderItemDTO> Items { get; set; } = new List<OrderItemDTO>();
	}
}
