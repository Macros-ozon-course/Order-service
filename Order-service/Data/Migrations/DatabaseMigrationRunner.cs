using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Migrations
{
	public static class DatabaseMigrationRunner
	{
		public static void MigrateUp(IServiceProvider serviceProvider)
		{
			using var scope = serviceProvider.CreateScope();
			var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

			runner.MigrateUp();
		}
	}
}
