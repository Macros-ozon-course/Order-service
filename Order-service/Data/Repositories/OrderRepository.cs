using Application.Interfaces;
using Dapper;
using Models.Entities;


namespace Data.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly IConnectionFactory _connectionFactory;

		public OrderRepository(IConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public async Task CreateAsync(Order order, CancellationToken ct)
		{
			const string createOrderSql = """
		INSERT INTO orders
		(
			id,
			user_id,
			status,
			total_amount,
			currency,
			created_at_utc,
			updated_at_utc,
			paid_at_utc,
			collected_at_utc,
			transferred_to_delivery_at_utc,
			delivered_at_utc,
			canceled_at_utc,
			cancel_reason,
			comment
		)
		VALUES
		(
			@Id,
			@UserId,
			@Status,
			@TotalAmount,
			@Currency,
			@CreatedAtUtc,
			@UpdatedAtUtc,
			@PaidAtUtc,
			@CollectedAtUtc,
			@TransferredToDeliveryAtUtc,
			@DeliveredAtUtc,
			@CanceledAtUtc,
			@CancelReason,
			@Comment
		);
		""";

			const string createOrderItemSql = """
		INSERT INTO order_items
		(
			id,
			order_id,
			product_id,
			product_name,
			product_image_url,
			sku,
			quantity,
			price_per_item,
			total_price,
			seller_id,
			created_at_utc
		)
		VALUES
		(
			@Id,
			@OrderId,
			@ProductId,
			@ProductName,
			@ProductImageUrl,
			@Sku,
			@Quantity,
			@PricePerItem,
			@TotalPrice,
			@SellerId,
			@CreatedAtUtc
		);
		""";

			const string createStatusHistorySql = """
		INSERT INTO order_status_history
		(
			id,
			order_id,
			old_status,
			new_status,
			changed_at_utc,
			changed_by_user_id,
			reason,
			comment
		)
		VALUES
		(
			@Id,
			@OrderId,
			@OldStatus,
			@NewStatus,
			@ChangedAtUtc,
			@ChangedByUserId,
			@Reason,
			@Comment
		);
		""";

			using var connection = _connectionFactory.CreateConnection();
			connection.Open();

			using var transaction = connection.BeginTransaction();

			try
			{
				await connection.ExecuteAsync(
					new CommandDefinition(createOrderSql, order, transaction, cancellationToken: ct));

				foreach (var item in order.Items)
				{
					await connection.ExecuteAsync(
						new CommandDefinition(createOrderItemSql, item, transaction, cancellationToken: ct));
				}

				foreach (var history in order.StatusHistory)
				{
					await connection.ExecuteAsync(
						new CommandDefinition(createStatusHistorySql, history, transaction, cancellationToken: ct));
				}

				transaction.Commit();
			}
			catch
			{
				transaction.Rollback();
				throw;
			}
		}
	}
}
