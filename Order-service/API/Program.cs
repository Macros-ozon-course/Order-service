using API;
using Data.DB;
using Microsoft.EntityFrameworkCore;
using Npgsql;

internal class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddCors(options =>
		{
			options.AddDefaultPolicy(policy =>
			{
				policy.SetIsOriginAllowed(origin => true)
					  .AllowAnyMethod()
					  .AllowAnyHeader()
					  .AllowCredentials();
			});
		});

		var connectionString = builder.Configuration.GetConnectionString("DB");
		var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
		dataSourceBuilder.EnableDynamicJson();
		var dataSource = dataSourceBuilder.Build();

		builder.Services.AddDbContext<AppDbContext>(opt =>
		{
			opt.UseNpgsql(dataSource);
		}, ServiceLifetime.Scoped);

		builder.Services.AddDistributedMemoryCache();
		ConfigureServices.Configure(builder.Services, builder.Configuration);

		builder.Services.AddControllers();
		// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
		builder.Services.AddOpenApi();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.MapOpenApi();
		}

		app.UseHttpsRedirection();

		app.UseAuthorization();

		app.MapControllers();

		app.Run();
	}
}