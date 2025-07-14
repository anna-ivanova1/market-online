using CartService.Domain.Interfaces;

namespace CartService.API.Services
{
	/// <summary>
	/// A background service that listens for product updates.
	/// </summary>
	public class ProductUpdateBackgroundService : BackgroundService
	{
		private readonly IProductUpdateListener _listener;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProductUpdateBackgroundService"/> class.
		/// </summary>
		/// <param name="listener">An implementation of <see cref="IProductUpdateListener"/> responsible for handling product update logic.</param>
		public ProductUpdateBackgroundService(IProductUpdateListener listener)
		{
			_listener = listener;
		}

		/// <summary>
		/// This method is called when the background service starts.
		/// It starts the product update listener and delays indefinitely until the service is stopped.
		/// </summary>
		/// <param name="stoppingToken">A token that indicates when the service should stop.</param>
		/// <returns>A task that represents the background execution operation.</returns>
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			await _listener.Start();
			await Task.Delay(Timeout.Infinite, stoppingToken);
		}

		/// <summary>
		/// This method is called when the background service is stopping.
		/// It stops the product update listener.
		/// </summary>
		/// <param name="cancellationToken">A token that indicates the stop request.</param>
		/// <returns>A task that represents the stop operation.</returns>
		public override async Task StopAsync(CancellationToken cancellationToken)
		{
			await _listener.Stop();
		}
	}
}
