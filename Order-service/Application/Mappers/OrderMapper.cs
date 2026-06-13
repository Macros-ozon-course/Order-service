using Application.DTO;
using Models.Entities;
using Models.Entities.enums;

namespace Application.Mappers
{
	public static class OrderMapper
	{
		public static Order ToEntity(this CreateOrderDTO dto, Guid userId)
		{
			var orderId = Guid.NewGuid();
			var now = DateTime.UtcNow;

			var items = dto.Items
				.Select(x => x.ToEntity(orderId, now))
				.ToList();

			return new Order
			{
				Id = orderId,
				UserId = userId,

				Status = OrderStatus.Created,

				TotalAmount = items.Sum(x => x.TotalPrice),
				Currency = string.IsNullOrWhiteSpace(dto.Currency) ? "RUB" : dto.Currency.Trim().ToUpperInvariant(),
				RecipientName = string.IsNullOrWhiteSpace(dto.RecipientName) ? null : dto.RecipientName.Trim(),
				RecipientPhone = string.IsNullOrWhiteSpace(dto.RecipientPhone) ? null : dto.RecipientPhone.Trim(),
				DeliveryAddress = dto.DeliveryAddress.Trim(),

				CreatedAtUtc = now,
				UpdatedAtUtc = null,

				PaidAtUtc = null,
				CollectedAtUtc = null,
				TransferredToDeliveryAtUtc = null,
				DeliveredAtUtc = null,
				CanceledAtUtc = null,

				CancelReason = null,
				Comment = null,

				Items = items,

				StatusHistory = new List<OrderStatusHistory>
				{
					new OrderStatusHistory
					{
						Id = Guid.NewGuid(),
						OrderId = orderId,

						OldStatus = null,
						NewStatus = OrderStatus.Created,

						ChangedAtUtc = now,
						ChangedByUserId = userId,

						Reason = "Order created",
						Comment = null
					}
				}
			};
		}

		public static OrderDTO ToDto(this Order order)
		{
			return new OrderDTO
			{
				Id = order.Id,
				OrderNumber = order.OrderNumber,
				UserId = order.UserId,
				Status = order.Status,
				TotalAmount = order.TotalAmount,
				Currency = order.Currency,
				RecipientName = order.RecipientName,
				RecipientPhone = order.RecipientPhone,
				DeliveryAddress = order.DeliveryAddress,
				CreatedAtUtc = order.CreatedAtUtc,
				UpdatedAtUtc = order.UpdatedAtUtc,
				PaidAtUtc = order.PaidAtUtc,
				CollectedAtUtc = order.CollectedAtUtc,
				TransferredToDeliveryAtUtc = order.TransferredToDeliveryAtUtc,
				DeliveredAtUtc = order.DeliveredAtUtc,
				CanceledAtUtc = order.CanceledAtUtc,
				CancelReason = order.CancelReason,
				Comment = order.Comment,
				Items = order.Items.Select(x => x.ToDto()).ToList()
			};
		}


		public static OrderStatusHistoryDTO ToDto(this OrderStatusHistory statusHistory)
		{
			return new OrderStatusHistoryDTO
			{
				Id = statusHistory.Id,
				OrderId = statusHistory.OrderId,
				OldStatus = statusHistory.OldStatus,
				NewStatus = statusHistory.NewStatus,
				ChangedAtUtc = statusHistory.ChangedAtUtc,
				ChangedByUserId = statusHistory.ChangedByUserId,
				Reason = statusHistory.Reason,
				Comment = statusHistory.Comment
			};
		}

		private static OrderItem ToEntity(this CreateOrderItemDTO dto, Guid orderId, DateTime createdAtUtc)
		{
			return new OrderItem
			{
				Id = Guid.NewGuid(),
				OrderId = orderId,

				ProductId = dto.ProductId,
				ProductName = dto.ProductName.Trim(),
				ProductImageUrl = string.IsNullOrWhiteSpace(dto.ProductImageUrl) ? null : dto.ProductImageUrl.Trim(),
				Sku = string.IsNullOrWhiteSpace(dto.Sku) ? null : dto.Sku.Trim(),

				Quantity = dto.Quantity,
				PricePerItem = dto.PricePerItem,
				TotalPrice = dto.Quantity * dto.PricePerItem,

				SellerId = dto.SellerId,

				CreatedAtUtc = createdAtUtc
			};
		}

		private static OrderItemDTO ToDto(this OrderItem item)
		{
			return new OrderItemDTO
			{
				Id = item.Id,
				OrderId = item.OrderId,
				ProductId = item.ProductId,
				ProductName = item.ProductName,
				ProductImageUrl = item.ProductImageUrl,
				Sku = item.Sku,
				Quantity = item.Quantity,
				PricePerItem = item.PricePerItem,
				TotalPrice = item.TotalPrice,
				SellerId = item.SellerId,
				CreatedAtUtc = item.CreatedAtUtc
			};
		}
	}
}
