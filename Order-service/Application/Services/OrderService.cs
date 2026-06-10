using Application.DTO;
using Application.Interfaces;
using Application.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}
}
