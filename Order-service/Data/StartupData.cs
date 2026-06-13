using Application.Interfaces;
using Data.DB;
using Data.Repositories;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Data
{
	public class StartupData
	{
		public static void Configure(IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("DB")
				?? throw new InvalidOperationException("Connection string 'DB' is not configured.");

			services.AddScoped<IConnectionFactory, DbConnectionFactory>();
			services.AddScoped<IOrderRepository, OrderRepository>();

			services
				.AddFluentMigratorCore()
				.ConfigureRunner(runner => runner
					.AddPostgres()
					.WithGlobalConnectionString(connectionString)
					.ScanIn(typeof(StartupData).Assembly).For.Migrations())
				.AddLogging(logging => logging.AddFluentMigratorConsole());
		}
	}
}
