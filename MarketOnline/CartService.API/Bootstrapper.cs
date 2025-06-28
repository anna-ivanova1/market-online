using CartService.Domain.Interfaces;
using CartService.Infrastructure.Data;

namespace CartService.API
{
	public static class Bootstrapper
	{
		public static void ConfigureContainer(IHostApplicationBuilder builder)
		{
			builder.Services.AddScoped<ICartRepository, CartRepository>();
			builder.Services.AddScoped<ICartService, Services.CartService>();
		}
	}
}
