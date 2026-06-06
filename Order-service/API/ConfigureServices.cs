using Application;
using Data;

namespace API
{
	public class ConfigureServices
	{
		public static void Configure(IServiceCollection services, IConfiguration configuration)
		{
			StartupApplication.Configure(services, configuration);
			StartupData.Configure(services);
		}
	}
}
