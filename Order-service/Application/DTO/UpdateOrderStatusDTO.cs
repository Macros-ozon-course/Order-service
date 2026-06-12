using Models.Entities.enums;

namespace Application.DTO
{
	public class UpdateOrderStatusDTO
	{
		public OrderStatus Status { get; set; }

		public Guid? ChangedByUserId { get; set; }

		public string? Reason { get; set; }

		public string? Comment { get; set; }
	}
}
