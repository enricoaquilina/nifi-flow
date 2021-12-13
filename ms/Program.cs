var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

public void ConfigureServices(IServiceCollection services)
{
	// Additional code...
	services.AddDbContext<CatalogContext>(options =>
	{
		options.UseSqlServer(Configuration["ConnectionString"],
		sqlServerOptionsAction: sqlOptions =>
		{
			sqlOptions.MigrationsAssembly(
				typeof(Startup).GetTypeInfo().Assembly.GetName().Name);

			//Configuring Connection Resiliency:
			sqlOptions.
				EnableRetryOnFailure(maxRetryCount: 5,
				maxRetryDelay: TimeSpan.FromSeconds(30),
				errorNumbersToAdd: null);

		});


		options.ConfigureWarnings(warnings => warnings.Throw(
			RelationalEventId.QueryClientEvaluationWarning));
	});

}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
