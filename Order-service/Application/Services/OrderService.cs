using Application.DTO;
using Application.Interfaces;
using Application.Mappers;

namespace Application.Services
{
	public class OrderService : IOrderService
	{
		private readonly IOrderRepository _orderRepository;

		public OrderService(IOrderRepository orderRepository)
		{
			_orderRepository = orderRepository;
		}

		public async Task CreateOrderAsync(Guid userId, CreateOrderDTO newOrder, CancellationToken ct)
		{
			ct.ThrowIfCancellationRequested();

			if (userId == Guid.Empty)
				throw new ArgumentException("User id is required", nameof(userId));

			var order = newOrder.ToEntity(userId);

			await _orderRepository.CreateAsync(order, ct);
		}

		public async Task<List<OrderDTO>> GetOrdersAsync(Guid? userId, CancellationToken ct)
		{
			ct.ThrowIfCancellationRequested();

			if (userId == Guid.Empty)
				throw new ArgumentException("User id is required", nameof(userId));

			var orders = await _orderRepository.GetOrdersAsync(userId, ct);

			return orders
				.Select(x => x.ToDto())
				.ToList();
		}

		public async Task<OrderDTO?> GetOrderByIdAsync(Guid id, CancellationToken ct)
		{
			ct.ThrowIfCancellationRequested();

			if (id == Guid.Empty)
				throw new ArgumentException("Order id is required", nameof(id));

			var order = await _orderRepository.GetOrderByIdAsync(id, ct);

			return order?.ToDto();
		}
	}
}
