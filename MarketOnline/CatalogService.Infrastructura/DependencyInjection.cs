using CatalogService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CatalogService.Infrastructure
{
	public static class DependencyInjection
	{
		public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
		{
			builder.Services.AddDbContext<CatalogDbContext>(options =>
				options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
		}
	}
}
