using Application.Interfaces;
using Dapper;
using Models.Entities;
using Models.Entities.enums;

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

		public async Task<List<Order>> GetOrdersAsync(Guid? userId, CancellationToken ct)
		{
			const string sql = """
		SELECT
			o.id AS "OrderId",
			o.user_id AS "UserId",
			o.status AS "Status",
			o.total_amount AS "TotalAmount",
			o.currency AS "Currency",
			o.created_at_utc AS "CreatedAtUtc",
			o.updated_at_utc AS "UpdatedAtUtc",
			o.paid_at_utc AS "PaidAtUtc",
			o.collected_at_utc AS "CollectedAtUtc",
			o.transferred_to_delivery_at_utc AS "TransferredToDeliveryAtUtc",
			o.delivered_at_utc AS "DeliveredAtUtc",
			o.canceled_at_utc AS "CanceledAtUtc",
			o.cancel_reason AS "CancelReason",
			o.comment AS "Comment",
			oi.id AS "ItemId",
			oi.order_id AS "ItemOrderId",
			oi.product_id AS "ProductId",
			oi.product_name AS "ProductName",
			oi.product_image_url AS "ProductImageUrl",
			oi.sku AS "Sku",
			oi.quantity AS "Quantity",
			oi.price_per_item AS "PricePerItem",
			oi.total_price AS "ItemTotalPrice",
			oi.seller_id AS "SellerId",
			oi.created_at_utc AS "ItemCreatedAtUtc"
		FROM orders o
		LEFT JOIN order_items oi ON oi.order_id = o.id
		WHERE (@UserId IS NULL OR o.user_id = @UserId)
		ORDER BY o.created_at_utc DESC, oi.created_at_utc ASC;
		""";

			using var connection = _connectionFactory.CreateConnection();

			var rows = await connection.QueryAsync<OrderWithItemRow>(
				new CommandDefinition(sql, new { UserId = userId }, cancellationToken: ct));

			var orders = new Dictionary<Guid, Order>();

			foreach (var row in rows)
			{
				if (!orders.TryGetValue(row.OrderId, out var order))
				{
					order = new Order
					{
						Id = row.OrderId,
						UserId = row.UserId,
						Status = row.Status,
						TotalAmount = row.TotalAmount,
						Currency = row.Currency,
						CreatedAtUtc = row.CreatedAtUtc,
						UpdatedAtUtc = row.UpdatedAtUtc,
						PaidAtUtc = row.PaidAtUtc,
						CollectedAtUtc = row.CollectedAtUtc,
						TransferredToDeliveryAtUtc = row.TransferredToDeliveryAtUtc,
						DeliveredAtUtc = row.DeliveredAtUtc,
						CanceledAtUtc = row.CanceledAtUtc,
						CancelReason = row.CancelReason,
						Comment = row.Comment,
						Items = new List<OrderItem>(),
						StatusHistory = new List<OrderStatusHistory>()
					};

					orders.Add(order.Id, order);
				}

				if (row.ItemId.HasValue)
				{
					order.Items.Add(new OrderItem
					{
						Id = row.ItemId.Value,
						OrderId = row.ItemOrderId!.Value,
						ProductId = row.ProductId!.Value,
						ProductName = row.ProductName!,
						ProductImageUrl = row.ProductImageUrl,
						Sku = row.Sku,
						Quantity = row.Quantity!.Value,
						PricePerItem = row.PricePerItem!.Value,
						TotalPrice = row.ItemTotalPrice!.Value,
						SellerId = row.SellerId!.Value,
						CreatedAtUtc = row.ItemCreatedAtUtc!.Value
					});
				}
			}

			return orders.Values.ToList();
		}

		private sealed class OrderWithItemRow
		{
			public Guid OrderId { get; set; }

			public Guid UserId { get; set; }

			public OrderStatus Status { get; set; }

			public decimal TotalAmount { get; set; }

			public string Currency { get; set; } = null!;

			public DateTime CreatedAtUtc { get; set; }

			public DateTime? UpdatedAtUtc { get; set; }

			public DateTime? PaidAtUtc { get; set; }

			public DateTime? CollectedAtUtc { get; set; }

			public DateTime? TransferredToDeliveryAtUtc { get; set; }

			public DateTime? DeliveredAtUtc { get; set; }

			public DateTime? CanceledAtUtc { get; set; }

			public string? CancelReason { get; set; }

			public string? Comment { get; set; }

			public Guid? ItemId { get; set; }

			public Guid? ItemOrderId { get; set; }

			public Guid? ProductId { get; set; }

			public string? ProductName { get; set; }

			public string? ProductImageUrl { get; set; }

			public string? Sku { get; set; }

			public int? Quantity { get; set; }

			public decimal? PricePerItem { get; set; }

			public decimal? ItemTotalPrice { get; set; }

			public Guid? SellerId { get; set; }

			public DateTime? ItemCreatedAtUtc { get; set; }
		}
	}
}
