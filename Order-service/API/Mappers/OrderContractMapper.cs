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
				Currency = string.IsNullOrWhiteSpace(request.Currency) ? "RUB" : request.Currency.Trim().ToUpperInvariant(),
				RecipientName = string.IsNullOrWhiteSpace(request.RecipientName) ? null : request.RecipientName.Trim(),
				RecipientPhone = string.IsNullOrWhiteSpace(request.RecipientPhone) ? null : request.RecipientPhone.Trim(),
				DeliveryAddress = request.DeliveryAddress.Trim(),
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
				OrderNumber = dto.OrderNumber,
				Status = dto.Status.ToString(),
				StatusText = dto.Status.ToText(),
				CanceledAtUtc = dto.CanceledAtUtc!.Value
			};
		}

		public static OrderResponse ToResponse(this OrderDTO dto)
		{
			return new OrderResponse
			{
				Id = dto.Id,
				OrderNumber = dto.OrderNumber,
				UserId = dto.UserId,
				Status = dto.Status.ToString(),
				StatusText = dto.Status.ToText(),
				TotalAmount = dto.TotalAmount,
				Currency = dto.Currency,
				RecipientName = dto.RecipientName,
				RecipientPhone = dto.RecipientPhone,
				DeliveryAddress = dto.DeliveryAddress,
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


		public static OrderStatusHistoryResponse ToResponse(this OrderStatusHistoryDTO dto)
		{
			return new OrderStatusHistoryResponse
			{
				Id = dto.Id,
				OrderId = dto.OrderId,
				OldStatus = dto.OldStatus?.ToString(),
				OldStatusText = dto.OldStatus?.ToText(),
				NewStatus = dto.NewStatus.ToString(),
				NewStatusText = dto.NewStatus.ToText(),
				ChangedAtUtc = dto.ChangedAtUtc,
				ChangedByUserId = dto.ChangedByUserId,
				Reason = dto.Reason,
				Comment = dto.Comment
			};
		}

		private static CreateOrderItemDTO ToDto(this CreateOrderItemRequest request)
		{
			return new CreateOrderItemDTO
			{
				ProductId = request.ProductId,
				ProductName = request.ProductName.Trim(),
				ProductImageUrl = string.IsNullOrWhiteSpace(request.ProductImageUrl) ? null : request.ProductImageUrl.Trim(),
				Sku = string.IsNullOrWhiteSpace(request.Sku) ? null : request.Sku.Trim(),
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

		private static string ToText(this OrderStatus status)
		{
			return status switch
			{
				OrderStatus.Created => "Создан",
				OrderStatus.Paid => "Оплачен",
				OrderStatus.Collecting => "Собирается",
				OrderStatus.TransferredToDelivery => "Передан в доставку",
				OrderStatus.Delivered => "Доставлен",
				OrderStatus.Canceled => "Отменен",
				_ => status.ToString()
			};
		}
	}
}
