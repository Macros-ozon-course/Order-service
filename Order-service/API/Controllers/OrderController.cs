using System.Security.Claims;
using API.Contracts.Orders;
using API.Mappers;
using Application.DTO;
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
		private readonly IValidator<CancelOrderRequest> _cancelOrderValidator;

		public OrderController(
			IOrderService orderService,
			IValidator<CreateOrderRequest> createOrderValidator,
			IValidator<UpdateOrderStatusRequest> updateOrderStatusValidator,
			IValidator<CancelOrderRequest> cancelOrderValidator)
		{
			_orderService = orderService;
			_createOrderValidator = createOrderValidator;
			_updateOrderStatusValidator = updateOrderStatusValidator;
			_cancelOrderValidator = cancelOrderValidator;
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

		[HttpGet("{id:guid}/status-history")]
		public async Task<IActionResult> GetOrderStatusHistory(Guid id, CancellationToken ct)
		{
			var statusHistory = await _orderService.GetOrderStatusHistoryAsync(id, ct);

			if (statusHistory is null)
			{
				return NotFound(new
				{
					code = "ORDER_NOT_FOUND",
					message = "Заказ не найден"
				});
			}

			return Ok(statusHistory.Select(x => x.ToResponse()).ToList());
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

		[HttpPost("{id:guid}/cancel")]
		public async Task<IActionResult> CancelOrder(Guid id, [FromBody] CancelOrderRequest? request, CancellationToken ct)
		{
			if (request is null)
				return BadRequest(new[]
				{
					new
					{
						field = nameof(CancelOrderRequest),
						message = "Request body is required."
					}
				});

			var validationResult = await _cancelOrderValidator.ValidateAsync(request, ct);
			if (!validationResult.IsValid)
			{
				return BadRequest(validationResult.Errors.Select(x => new
				{
					field = x.PropertyName,
					message = x.ErrorMessage
				}));
			}

			var result = await _orderService.CancelOrderAsync(id, request.ToDto(), GetChangedByUserId(), ct);

			if (!result.IsSuccess)
				return ToCancelOrderErrorResponse(result.Error!.Value);

			return Ok(result.Order!.ToCancelResponse());
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

		private IActionResult ToCancelOrderErrorResponse(CancelOrderError error)
		{
			return error switch
			{
				CancelOrderError.NotFound => NotFound(new
				{
					code = "ORDER_NOT_FOUND",
					message = "Заказ не найден"
				}),
				CancelOrderError.AlreadyCanceled => Conflict(new
				{
					code = "ORDER_ALREADY_CANCELED",
					message = "Заказ уже отменен"
				}),
				CancelOrderError.CannotBeCanceled => Conflict(new
				{
					code = "ORDER_CANNOT_BE_CANCELED",
					message = "Заказ нельзя отменить в текущем статусе"
				}),
				_ => Conflict()
			};
		}

		private Guid? GetChangedByUserId()
		{
			var userIdValue = User.FindFirst("user_id")?.Value
				?? User.FindFirst("userId")?.Value
				?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
				?? User.FindFirst("sub")?.Value;

			if (Guid.TryParse(userIdValue, out var userId))
				return userId;

			return null;
		}
	}
}
