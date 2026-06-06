using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Data.DB
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<Order> Orders => Set<Order>();

		public DbSet<OrderItem> OrderItems => Set<OrderItem>();

		public DbSet<OrderStatusHistory> OrderStatusHistories => Set<OrderStatusHistory>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			ConfigureOrders(modelBuilder);
			ConfigureOrderItems(modelBuilder);
			ConfigureOrderStatusHistories(modelBuilder);
		}

		private static void ConfigureOrders(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Order>(entity =>
			{
				entity.ToTable("Orders");

				entity.HasKey(x => x.Id);

				entity.Property(x => x.UserId)
					.IsRequired();

				entity.Property(x => x.Status)
					.HasConversion<string>()
					.HasMaxLength(50)
					.IsRequired();

				entity.Property(x => x.TotalAmount)
					.HasColumnType("numeric(18,2)")
					.IsRequired();

				entity.Property(x => x.Currency)
					.HasMaxLength(10)
					.IsRequired();

				entity.Property(x => x.CreatedAtUtc)
					.IsRequired();

				entity.Property(x => x.CancelReason)
					.HasMaxLength(500);

				entity.Property(x => x.Comment)
					.HasMaxLength(1000);

				entity.HasMany(x => x.Items)
					.WithOne(x => x.Order)
					.HasForeignKey(x => x.OrderId)
					.OnDelete(DeleteBehavior.Cascade);

				entity.HasMany(x => x.StatusHistory)
					.WithOne(x => x.Order)
					.HasForeignKey(x => x.OrderId)
					.OnDelete(DeleteBehavior.Cascade);

				entity.HasIndex(x => x.UserId);

				entity.HasIndex(x => x.Status);

				entity.HasIndex(x => x.CreatedAtUtc);
			});
		}

		private static void ConfigureOrderItems(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<OrderItem>(entity =>
			{
				entity.ToTable("OrderItems");

				entity.HasKey(x => x.Id);

				entity.Property(x => x.OrderId)
					.IsRequired();

				entity.Property(x => x.ProductId)
					.IsRequired();

				entity.Property(x => x.ProductName)
					.HasMaxLength(255)
					.IsRequired();

				entity.Property(x => x.ProductImageUrl)
					.HasMaxLength(1000);

				entity.Property(x => x.Sku)
					.HasMaxLength(100);

				entity.Property(x => x.Quantity)
					.IsRequired();

				entity.Property(x => x.PricePerItem)
					.HasColumnType("numeric(18,2)")
					.IsRequired();

				entity.Property(x => x.TotalPrice)
					.HasColumnType("numeric(18,2)")
					.IsRequired();

				entity.Property(x => x.SellerId)
					.IsRequired();

				entity.Property(x => x.CreatedAtUtc)
					.IsRequired();

				entity.HasIndex(x => x.OrderId);

				entity.HasIndex(x => x.ProductId);

				entity.HasIndex(x => x.SellerId);
			});
		}

		private static void ConfigureOrderStatusHistories(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<OrderStatusHistory>(entity =>
			{
				entity.ToTable("OrderStatusHistories");

				entity.HasKey(x => x.Id);

				entity.Property(x => x.OrderId)
					.IsRequired();

				entity.Property(x => x.OldStatus)
					.HasConversion<string>()
					.HasMaxLength(50);

				entity.Property(x => x.NewStatus)
					.HasConversion<string>()
					.HasMaxLength(50)
					.IsRequired();

				entity.Property(x => x.ChangedAtUtc)
					.IsRequired();

				entity.Property(x => x.Reason)
					.HasMaxLength(500);

				entity.Property(x => x.Comment)
					.HasMaxLength(1000);

				entity.HasIndex(x => x.OrderId);

				entity.HasIndex(x => x.ChangedAtUtc);
			});
		}
	}
}
