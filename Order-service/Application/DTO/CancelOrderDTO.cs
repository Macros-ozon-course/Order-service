namespace Application.DTO
{
	public class CancelOrderDTO
	{
		public string Reason { get; set; } = null!;

		public string? Comment { get; set; }
	}
}
