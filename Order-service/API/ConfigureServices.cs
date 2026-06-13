using API.Validators.Orders;
using Application;
using Data;
using FluentValidation;

namespace API
{
	public class ConfigureServices
	{
		public static void Configure(IServiceCollection services, IConfiguration configuration)
		{
			StartupApplication.Configure(services, configuration);
			StartupData.Configure(services, configuration);

			services.AddValidatorsFromAssemblyContaining<CreateOrderRequestValidator>();
		}
	}
}
