using API;
using Data.Migrations;

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

		builder.Services.AddDistributedMemoryCache();
		ConfigureServices.Configure(builder.Services, builder.Configuration);

		builder.Services.AddControllers();
		// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
		builder.Services.AddOpenApi();

		var app = builder.Build();

		DatabaseMigrationRunner.MigrateUp(app.Services);

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.MapOpenApi();
		}

		app.UseHttpsRedirection();

		app.UseCors();

		app.UseAuthorization();

		app.MapControllers();

		app.Run();
	}
}
