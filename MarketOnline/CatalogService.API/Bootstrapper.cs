using CatalogService.API.Services;
using CatalogService.Domain.Interfaces;
using CatalogService.Infrastructure.Data;

namespace CatalogService.API
{
	public static class Bootstrapper
	{
		public static void ConfigureContainer(IHostApplicationBuilder builder)
		{
			builder.Services.AddDbContext<CatalogDbContext>();

			builder.Services.AddScoped<IProductRepository, ProductRepository>();
			builder.Services.AddScoped<IProductService, ProductService>();
			builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
			builder.Services.AddScoped<ICategoryService, CategoryService>();
		}
	}
}
