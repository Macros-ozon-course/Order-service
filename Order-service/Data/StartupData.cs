using Application.Interfaces;
using Data.DB;
using Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Data
{
	public class StartupData
	{
		public static void Configure(IServiceCollection services)
		{
			services.AddScoped<IConnectionFactory, DbConnectionFactory>();
			services.AddScoped<IOrderRepository, OrderRepository>();
		}
	}
}
