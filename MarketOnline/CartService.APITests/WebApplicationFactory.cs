using CartService.API;
using CartService.Domain.Interfaces;
using CartService.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		var projectDir = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\CartService.API");
		var fullPath = Path.GetFullPath(projectDir);

		builder.UseContentRoot(fullPath);

		builder.UseEnvironment("Testing");

		builder.ConfigureServices(services =>
		{
			services.AddScoped<ICartRepository, CartRepository>();
			services.AddScoped<ICartService, CartService.API.Services.CartService>();
			services.AddAutoMapper(typeof(MappingProfile));
		});
	}

	protected override IHost CreateHost(IHostBuilder builder)
	{
		builder.ConfigureServices(services =>
		{
		});

		return base.CreateHost(builder);
	}
}

