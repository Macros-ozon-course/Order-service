using API.Contracts.Orders;
using API.Mappers;
using Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("api/v1/orders")]
	public class OrderController : ControllerBase
	{
		private readonly IOrderService _orderService;
		private readonly IValidator<CreateOrderRequest> _createOrderValidator;
		private readonly IValidator<UpdateOrderStatusRequest> _updateOrderStatusValidator;

		public OrderController(
			IOrderService orderService,
			IValidator<CreateOrderRequest> createOrderValidator,
			IValidator<UpdateOrderStatusRequest> updateOrderStatusValidator)
		{
			_orderService = orderService;
			_createOrderValidator = createOrderValidator;
			_updateOrderStatusValidator = updateOrderStatusValidator;
		}

		[HttpGet]
		public async Task<IActionResult> GetOrders([FromQuery] Guid? userId, CancellationToken ct)
		{
			var orders = await _orderService.GetOrdersAsync(userId, ct);

			return Ok(orders.Select(x => x.ToResponse()).ToList());
		}


		[HttpGet("{id:guid}")]
		public async Task<IActionResult> GetOrderById(Guid id, CancellationToken ct)
		{
			var order = await _orderService.GetOrderByIdAsync(id, ct);

			if (order is null)
				return NotFound();

			return Ok(order.ToResponse());
		}

		[HttpPatch("{id:guid}/status")]
		public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] UpdateOrderStatusRequest? request, CancellationToken ct)
		{
			if (request is null)
				return BadRequest(new[]
				{
					new
					{
						field = nameof(UpdateOrderStatusRequest),
						message = "Request body is required."
					}
				});

			var validationResult = await _updateOrderStatusValidator.ValidateAsync(request, ct);
			if (!validationResult.IsValid)
			{
				return BadRequest(validationResult.Errors.Select(x => new
				{
					field = x.PropertyName,
					message = x.ErrorMessage
				}));
			}

			var order = await _orderService.UpdateOrderStatusAsync(id, request.ToDto(), ct);

			if (order is null)
				return NotFound();

			return Ok(order.ToResponse());
		}

		[HttpPost]
		public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
		{
			var validationResult = await _createOrderValidator.ValidateAsync(request);
			if (!validationResult.IsValid)
			{
				return BadRequest(validationResult.Errors.Select(x => new
				{
					field = x.PropertyName,
					message = x.ErrorMessage
				}));
			}

			return Ok();
		}
	}
}
