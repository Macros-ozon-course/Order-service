using Application.DTO;

namespace Application.Interfaces
{
	public interface IOrderService
	{
		public Task CreateOrderAsync(Guid buyerId, CreateOrderDTO orderDto, CancellationToken ct);

		public Task<List<OrderDTO>> GetOrdersAsync(Guid? userId, CancellationToken ct);
	}
}
