using Models.Entities;
using Models.Entities.enums;

namespace Application.Interfaces
{
	public interface IOrderRepository
	{
		public Task CreateAsync(Order order, CancellationToken ct);

		public Task<List<Order>> GetOrdersAsync(Guid? userId, CancellationToken ct);

		public Task<Order?> GetOrderByIdAsync(Guid id, CancellationToken ct);

		public Task<Order?> UpdateStatusAsync(Guid id, OrderStatus status, Guid? changedByUserId, string? reason, string? comment, CancellationToken ct);
	}
}
