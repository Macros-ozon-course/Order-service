using API.Contracts.Orders;
using Application.DTO;

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
	}
}