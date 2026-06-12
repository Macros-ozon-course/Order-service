using API.Contracts.Orders;
using Application.DTO;
using Models.Entities.enums;

namespace API.Mappers
{
	public static class OrderContractMapper
	{
		public static CreateOrderDTO ToDto(this CreateOrderRequest request)
		{
			return new CreateOrderDTO
			{
				TotalAmount = request.TotalAmount,
				Currency = request.Currency.Trim().ToUpperInvariant(),
				Items = request.Items.Select(x => x.ToDto()).ToList()
			};
		}

		public static UpdateOrderStatusDTO ToDto(this UpdateOrderStatusRequest request)
		{
			return new UpdateOrderStatusDTO
			{
				Status = Enum.Parse<OrderStatus>(request.Status, true),
				ChangedByUserId = request.ChangedByUserId,
				Reason = string.IsNullOrWhiteSpace(request.Reason) ? null : request.Reason.Trim(),
				Comment = string.IsNullOrWhiteSpace(request.Comment) ? null : request.Comment.Trim()
			};
		}


		public static CancelOrderDTO ToDto(this CancelOrderRequest request)
		{
			return new CancelOrderDTO
			{
				Reason = request.Reason.Trim(),
				Comment = string.IsNullOrWhiteSpace(request.Comment) ? null : request.Comment.Trim()
			};
		}

		public static CancelOrderResponse ToCancelResponse(this OrderDTO dto)
		{
			return new CancelOrderResponse
			{
				Id = dto.Id,
				Status = dto.Status.ToString(),
				CanceledAtUtc = dto.CanceledAtUtc!.Value
			};
		}

		public static OrderResponse ToResponse(this OrderDTO dto)
		{
			return new OrderResponse
			{
				Id = dto.Id,
				UserId = dto.UserId,
				Status = dto.Status.ToString(),
				TotalAmount = dto.TotalAmount,
				Currency = dto.Currency,
				CreatedAtUtc = dto.CreatedAtUtc,
				UpdatedAtUtc = dto.UpdatedAtUtc,
				PaidAtUtc = dto.PaidAtUtc,
				CollectedAtUtc = dto.CollectedAtUtc,
				TransferredToDeliveryAtUtc = dto.TransferredToDeliveryAtUtc,
				DeliveredAtUtc = dto.DeliveredAtUtc,
				CanceledAtUtc = dto.CanceledAtUtc,
				CancelReason = dto.CancelReason,
				Comment = dto.Comment,
				Items = dto.Items.Select(x => x.ToResponse()).ToList()
			};
		}

		private static CreateOrderItemDTO ToDto(this CreateOrderItemRequest request)
		{
			return new CreateOrderItemDTO
			{
				ProductId = request.ProductId,
				ProductName = request.ProductName.Trim(),
				ProductImageUrl = request.ProductImageUrl,
				Sku = request.Sku,
				Quantity = request.Quantity,
				PricePerItem = request.PricePerItem,
				TotalPrice = request.TotalPrice,
				SellerId = request.SellerId
			};
		}

		private static OrderItemResponse ToResponse(this OrderItemDTO dto)
		{
			return new OrderItemResponse
			{
				Id = dto.Id,
				OrderId = dto.OrderId,
				ProductId = dto.ProductId,
				ProductName = dto.ProductName,
				ProductImageUrl = dto.ProductImageUrl,
				Sku = dto.Sku,
				Quantity = dto.Quantity,
				PricePerItem = dto.PricePerItem,
				TotalPrice = dto.TotalPrice,
				SellerId = dto.SellerId,
				CreatedAtUtc = dto.CreatedAtUtc
			};
		}
	}
}
