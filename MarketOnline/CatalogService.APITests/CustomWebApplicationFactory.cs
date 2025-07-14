using CatalogService.API;
using CatalogService.API.Services;
using CatalogService.Domain.Interfaces;
using CatalogService.Infrastructure.Data;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CatalogService.APITests
{
	internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
	{
		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureServices(services =>
			{
				services.AddDbContext<CatalogDbContext>();
				services.AddScoped<IProductRepository, ProductRepository>();
				services.AddScoped<IProductService, ProductService>();
				services.AddScoped<ICategoryRepository, CategoryRepository>();
				services.AddScoped<ICategoryService, CategoryService>();
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
}
