using CartService.API.Listeners;
using CartService.Domain.Interfaces;
using CartService.Infrastructure.Data;

namespace CartService.API
{
	/// <summary>
	/// Provides application-wide configuration during startup.
	/// </summary>
	public static class Bootstrapper
	{
		/// <summary>
		/// Configures services and dependencies for the application.
		/// </summary>
		/// <param name="builder">The <see cref="IHostApplicationBuilder"/> used to configure the application's services and container.</param>
		public static void ConfigureContainer(IHostApplicationBuilder builder)
		{
			builder.Services.AddScoped<ICartRepository, CartRepository>();
			builder.Services.AddScoped<ICartService, Services.CartService>();
			builder.Services.AddSingleton<IProductUpdateListener, ProductUpdateListener>();
		}
	}
}
