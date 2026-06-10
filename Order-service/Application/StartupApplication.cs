using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
	public class StartupApplication
	{
		public static void Configure(IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<IOrderService, OrderService>();
		}
	}
}
