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
		private readonly IValidator<CreateOrderRequest> _validator;

		public OrderController(
			IOrderService orderService,
			IValidator<CreateOrderRequest> validator)
		{
			_orderService = orderService;
			_validator = validator;
		}

		[HttpGet]
		public async Task<IActionResult> GetOrders([FromQuery] Guid? userId, CancellationToken ct)
		{
			var orders = await _orderService.GetOrdersAsync(userId, ct);

			return Ok(orders.Select(x => x.ToResponse()).ToList());
		}

		[HttpPost]
		public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
		{
			var validationResult = await _validator.ValidateAsync(request);
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
