using Application.DTO;

namespace Application.Interfaces
{
	public interface IOrderService
	{
		public Task CreateOrderAsync(Guid buyerId, CreateOrderDTO orderDto, CancellationToken ct);

		public Task<List<OrderDTO>> GetOrdersAsync(Guid? userId, CancellationToken ct);

		public Task<OrderDTO?> GetOrderByIdAsync(Guid id, CancellationToken ct);

		public Task<List<OrderStatusHistoryDTO>?> GetOrderStatusHistoryAsync(Guid id, CancellationToken ct);

		public Task<OrderDTO?> UpdateOrderStatusAsync(Guid id, UpdateOrderStatusDTO orderStatusDto, CancellationToken ct);

		public Task<CancelOrderResult> CancelOrderAsync(Guid id, CancelOrderDTO cancelOrderDto, Guid? changedByUserId, CancellationToken ct);
	}
}
