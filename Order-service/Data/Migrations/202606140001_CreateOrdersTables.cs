using FluentMigrator;

namespace Data.Migrations
{
	[Migration(202606140001)]
	public sealed class CreateOrdersTables : Migration
	{
		public override void Up()
		{
			Execute.Sql("""
			CREATE SEQUENCE IF NOT EXISTS orders_order_number_seq START WITH 1001 INCREMENT BY 1;

			CREATE TABLE IF NOT EXISTS orders
			(
				id uuid PRIMARY KEY,
				order_number bigint NOT NULL DEFAULT nextval('orders_order_number_seq'),
				user_id uuid NOT NULL,
				status integer NOT NULL,
				total_amount numeric(18, 2) NOT NULL,
				currency varchar(3) NOT NULL DEFAULT 'RUB',
				recipient_name varchar(200) NULL,
				recipient_phone varchar(50) NULL,
				delivery_address varchar(500) NOT NULL DEFAULT '',
				created_at_utc timestamp with time zone NOT NULL,
				updated_at_utc timestamp with time zone NULL,
				paid_at_utc timestamp with time zone NULL,
				collected_at_utc timestamp with time zone NULL,
				transferred_to_delivery_at_utc timestamp with time zone NULL,
				delivered_at_utc timestamp with time zone NULL,
				canceled_at_utc timestamp with time zone NULL,
				cancel_reason varchar(500) NULL,
				comment varchar(1000) NULL
			);

			ALTER TABLE orders ADD COLUMN IF NOT EXISTS order_number bigint;
			ALTER TABLE orders ADD COLUMN IF NOT EXISTS recipient_name varchar(200) NULL;
			ALTER TABLE orders ADD COLUMN IF NOT EXISTS recipient_phone varchar(50) NULL;
			ALTER TABLE orders ADD COLUMN IF NOT EXISTS delivery_address varchar(500) NOT NULL DEFAULT '';
			ALTER TABLE orders ALTER COLUMN currency SET DEFAULT 'RUB';
			ALTER TABLE orders ALTER COLUMN order_number SET DEFAULT nextval('orders_order_number_seq');
			UPDATE orders SET order_number = nextval('orders_order_number_seq') WHERE order_number IS NULL;
			SELECT setval('orders_order_number_seq', GREATEST(COALESCE((SELECT MAX(order_number) FROM orders), 1000), 1000));
			ALTER TABLE orders ALTER COLUMN order_number SET NOT NULL;
			CREATE UNIQUE INDEX IF NOT EXISTS ux_orders_order_number ON orders(order_number);
			CREATE INDEX IF NOT EXISTS ix_orders_user_id_created_at_utc ON orders(user_id, created_at_utc DESC);

			CREATE TABLE IF NOT EXISTS order_items
			(
				id uuid PRIMARY KEY,
				order_id uuid NOT NULL REFERENCES orders(id) ON DELETE CASCADE,
				product_id uuid NOT NULL,
				product_name varchar(300) NOT NULL,
				product_image_url varchar(1000) NULL,
				sku varchar(100) NULL,
				quantity integer NOT NULL,
				price_per_item numeric(18, 2) NOT NULL,
				total_price numeric(18, 2) NOT NULL,
				seller_id uuid NOT NULL,
				created_at_utc timestamp with time zone NOT NULL
			);

			CREATE INDEX IF NOT EXISTS ix_order_items_order_id ON order_items(order_id);
			CREATE INDEX IF NOT EXISTS ix_order_items_product_id ON order_items(product_id);
			CREATE INDEX IF NOT EXISTS ix_order_items_seller_id ON order_items(seller_id);

			CREATE TABLE IF NOT EXISTS order_status_history
			(
				id uuid PRIMARY KEY,
				order_id uuid NOT NULL REFERENCES orders(id) ON DELETE CASCADE,
				old_status integer NULL,
				new_status integer NOT NULL,
				changed_at_utc timestamp with time zone NOT NULL,
				changed_by_user_id uuid NULL,
				reason varchar(500) NULL,
				comment varchar(1000) NULL
			);

			CREATE INDEX IF NOT EXISTS ix_order_status_history_order_id_changed_at_utc ON order_status_history(order_id, changed_at_utc);
			""");
		}

		public override void Down()
		{
			Execute.Sql("""
			DROP TABLE IF EXISTS order_status_history;
			DROP TABLE IF EXISTS order_items;
			DROP TABLE IF EXISTS orders;
			DROP SEQUENCE IF EXISTS orders_order_number_seq;
			""");
		}
	}
}
