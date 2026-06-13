using Application.DTO;
using Application.Interfaces;
using Application.Mappers;
using Models.Entities.enums;

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


		public async Task<List<OrderStatusHistoryDTO>?> GetOrderStatusHistoryAsync(Guid id, CancellationToken ct)
		{
			ct.ThrowIfCancellationRequested();

			if (id == Guid.Empty)
				throw new ArgumentException("Order id is required", nameof(id));

			var exists = await _orderRepository.ExistsAsync(id, ct);
			if (!exists)
				return null;

			var statusHistory = await _orderRepository.GetOrderStatusHistoryAsync(id, ct);

			return statusHistory
				.Select(x => x.ToDto())
				.ToList();
		}

		public async Task<OrderDTO?> UpdateOrderStatusAsync(Guid id, UpdateOrderStatusDTO orderStatusDto, CancellationToken ct)
		{
			ct.ThrowIfCancellationRequested();

			if (id == Guid.Empty)
				throw new ArgumentException("Order id is required", nameof(id));

			var order = await _orderRepository.UpdateStatusAsync(
				id,
				orderStatusDto.Status,
				orderStatusDto.ChangedByUserId,
				orderStatusDto.Reason,
				orderStatusDto.Comment,
				ct);

			return order?.ToDto();
		}

		public async Task<CancelOrderResult> CancelOrderAsync(Guid id, CancelOrderDTO cancelOrderDto, Guid? changedByUserId, CancellationToken ct)
		{
			ct.ThrowIfCancellationRequested();

			if (id == Guid.Empty)
				throw new ArgumentException("Order id is required", nameof(id));

			var order = await _orderRepository.GetOrderByIdAsync(id, ct);
			if (order is null)
				return CancelOrderResult.Failed(CancelOrderError.NotFound);

			var cancellationError = GetCancellationError(order.Status);
			if (cancellationError.HasValue)
				return CancelOrderResult.Failed(cancellationError.Value);

			var canceledOrder = await _orderRepository.CancelAsync(
				id,
				changedByUserId,
				cancelOrderDto.Reason,
				cancelOrderDto.Comment,
				ct);

			if (canceledOrder is null)
				return CancelOrderResult.Failed(CancelOrderError.NotFound);

			if (canceledOrder.Status != OrderStatus.Canceled)
			{
				var currentCancellationError = GetCancellationError(canceledOrder.Status);
				return CancelOrderResult.Failed(currentCancellationError ?? CancelOrderError.CannotBeCanceled);
			}

			return CancelOrderResult.Success(canceledOrder.ToDto());
		}

		private static CancelOrderError? GetCancellationError(OrderStatus status)
		{
			if (status == OrderStatus.Canceled)
				return CancelOrderError.AlreadyCanceled;

			if (!CanCancel(status))
				return CancelOrderError.CannotBeCanceled;

			return null;
		}

		private static bool CanCancel(OrderStatus status)
		{
			return status is OrderStatus.Created or OrderStatus.Paid or OrderStatus.Collecting;
		}
	}
}
