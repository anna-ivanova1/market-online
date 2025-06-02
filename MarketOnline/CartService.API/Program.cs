using Asp.Versioning;
using CartService.API;
using CartService.API.Services;
using CartService.API.Swagger;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

Bootstrapper.ConfigureContainer(builder);
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddHostedService<ProductUpdateBackgroundService>();

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(
options =>
{
	var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options =>
{
	// Add a custom operation filter which sets default values
	options.OperationFilter<SwaggerDefaultValues>();
});

builder.Services.AddApiVersioning(options =>
{
	options.AssumeDefaultVersionWhenUnspecified = true;
	options.DefaultApiVersion = new ApiVersion(1, 0);
	options.ReportApiVersions = true;
	options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services
	.AddApiVersioning()
	.AddApiExplorer(options =>
	{
		options.GroupNameFormat = "'v'VVV";
		options.SubstituteApiVersionInUrl = true;
	});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		var descriptions = app.DescribeApiVersions();

		foreach (var description in descriptions)
		{
			var url = $"/swagger/{description.GroupName}/swagger.json";
			var name = description.GroupName.ToUpperInvariant();
			options.SwaggerEndpoint(url, name);
		}
	});
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }