using Models.Entities;

namespace Application.Interfaces
{
	public interface IOrderRepository
	{
		public Task CreateAsync(Order order, CancellationToken ct);

		public Task<List<Order>> GetOrdersAsync(Guid? userId, CancellationToken ct);
	}
}
