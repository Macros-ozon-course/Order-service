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
				Currency = dto.Currency.Trim().ToUpperInvariant(),

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

		private static OrderItem ToEntity(this CreateOrderItemDTO dto, Guid orderId, DateTime createdAtUtc)
		{
			return new OrderItem
			{
				Id = Guid.NewGuid(),
				OrderId = orderId,

				ProductId = dto.ProductId,
				ProductName = dto.ProductName.Trim(),
				ProductImageUrl = dto.ProductImageUrl,
				Sku = dto.Sku,

				Quantity = dto.Quantity,
				PricePerItem = dto.PricePerItem,
				TotalPrice = dto.Quantity * dto.PricePerItem,

				SellerId = dto.SellerId,

				CreatedAtUtc = createdAtUtc
			};
		}
	}
}