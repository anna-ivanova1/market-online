using CartService.Domain.Interfaces;

namespace CartService.API
{
	public static class Bootstrapper
	{
		public static void ConfigureContainer(IHostApplicationBuilder builder)
		{
			builder.Services.AddScoped<ICartService, Services.CartService>();
		}
	}
}
