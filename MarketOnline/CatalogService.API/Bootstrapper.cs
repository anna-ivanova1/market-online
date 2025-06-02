using CatalogService.API.Interfaces;
using CatalogService.API.Services;
using CatalogService.API.Services.LinkBuilders;
using CatalogService.Domain.Interfaces;
using CatalogService.Infrastructure.Data;
using CatalogService.Infrastructure.Publishers;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.API
{
	public static class Bootstrapper
	{
		public static void ConfigureContainer(IHostApplicationBuilder builder)
		{
			var dbPath = Path.Combine(AppContext.BaseDirectory, "Catalog.db");

			builder.Services.AddDbContext<CatalogDbContext>(options =>
				options.UseSqlite($"Data Source={dbPath}"));

			builder.Services.AddHttpContextAccessor();
			builder.Services.AddScoped<IProductLinkBuilder, ProductLinkBuilder>();
			builder.Services.AddScoped<IProductRepository, ProductRepository>();
			builder.Services.AddScoped<IProductService, ProductService>();
			builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
			builder.Services.AddScoped<ICategoryService, CategoryService>();
			builder.Services.AddScoped<IProductUpdatePublisher, ProductUpdatePublisher>();
		}
	}
}
