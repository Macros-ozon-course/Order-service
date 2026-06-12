namespace Application.DTO
{
	public class CancelOrderResult
	{
		public bool IsSuccess => Error is null;

		public CancelOrderError? Error { get; private init; }

		public OrderDTO? Order { get; private init; }

		public static CancelOrderResult Success(OrderDTO order)
		{
			return new CancelOrderResult
			{
				Order = order
			};
		}

		public static CancelOrderResult Failed(CancelOrderError error)
		{
			return new CancelOrderResult
			{
				Error = error
			};
		}
	}
}
