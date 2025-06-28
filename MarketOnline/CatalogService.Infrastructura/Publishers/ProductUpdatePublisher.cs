using CatalogService.Domain.Interfaces;
using CatalogService.Domain.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Text;
using System.Text.Json;

namespace CatalogService.Infrastructure.Publishers
{
	public class ProductUpdatePublisher : IProductUpdatePublisher
	{
		private const string ExchangeName = "product.exchange";

		public async Task PublishProductUpdate(ProductUpdateMessage message)
		{
			try
			{
				var factory = new ConnectionFactory { HostName = "localhost" };
				factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

				using var connection = await factory.CreateConnectionAsync();

				using var channel = await connection.CreateChannelAsync();
				await channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Direct, true);

				var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

				await channel.BasicPublishAsync(ExchangeName, string.Empty, true, body);

				await channel.CloseAsync();
				await connection.CloseAsync();

			}
			catch (OperationInterruptedException ex)
			{
				Console.WriteLine($"Exchange not found. Ensure CartService has declared the exchange and bindings. {ex.Message}");
			}
		}
	}
}