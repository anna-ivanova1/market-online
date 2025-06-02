using CartService.Domain.Interfaces;

namespace CartService.API.Services
{
	public class ProductUpdateBackgroundService : BackgroundService
	{
		private readonly IProductUpdateListener _listener;

		public ProductUpdateBackgroundService(IProductUpdateListener listener)
		{
			_listener = listener;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			await _listener.Start();

			await Task.Delay(Timeout.Infinite, stoppingToken);
		}

		public override async Task StopAsync(CancellationToken cancellationToken)
		{
			await _listener.Stop();
		}
	}
}
