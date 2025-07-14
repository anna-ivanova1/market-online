using System.Text;
using System.Text.Json;

using CartService.Domain.Interfaces;
using CartService.Domain.Messages;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CartService.API.Listeners
{
	/// <summary>
	/// Listens for product update messages from RabbitMQ and updates cart items accordingly.
	/// Supports retry logic via dead-letter exchanges.
	/// </summary>
	public class ProductUpdateListener : IProductUpdateListener, IDisposable
	{
		private readonly IServiceScopeFactory _scopeFactory;

		private IConnection? _connection;
		private IChannel? _channel;

		private const string MainQueue = "product.update";
		private const string RetryQueue = "product.update.retry";
		private const string MainExchange = "product.exchange";
		private const string RetryExchange = "product.retry.exchange";
		private const string DeadLetterExchange = "x-dead-letter-exchange";
		private const string TTLMessage = "x-message-ttl";

		/// <summary>
		/// Initializes a new instance of the <see cref="ProductUpdateListener"/> class.
		/// </summary>
		/// <param name="scopeFactory">Factory used to create scoped service instances.</param>
		public ProductUpdateListener(IServiceScopeFactory scopeFactory)
		{
			_scopeFactory = scopeFactory;
		}

		/// <summary>
		/// Starts the listener by establishing the RabbitMQ connection and consuming messages from the queue.
		/// </summary>
		public async Task Start()
		{
			try
			{
				var factory = new ConnectionFactory
				{
					Uri = new Uri("amqp://guest:guest@localhost:5672")
				};

				_connection = await factory.CreateConnectionAsync();
				_channel = await _connection.CreateChannelAsync();

				// Declare exchanges
				await _channel.ExchangeDeclareAsync(MainExchange, ExchangeType.Direct, durable: true);
				await _channel.ExchangeDeclareAsync(RetryExchange, ExchangeType.Direct, durable: true);

				var args = new Dictionary<string, object?>()
				{
					{ DeadLetterExchange, RetryExchange }
				};

				// Declare and bind main queue
				await _channel.QueueDeclareAsync(MainQueue, durable: true, exclusive: false, autoDelete: false, arguments: args);
				await _channel.QueueBindAsync(MainQueue, MainExchange, routingKey: "");

				var args2 = new Dictionary<string, object?>
					{
						{ DeadLetterExchange, MainExchange },
						{ TTLMessage, 10000 }
					};
				// Declare and bind retry queue
				await _channel.QueueDeclareAsync(RetryQueue, durable: true, exclusive: false, autoDelete: false, arguments: args2);
				await _channel.QueueBindAsync(RetryQueue, RetryExchange, routingKey: "");

				// Set up the consumer
				var consumer = new AsyncEventingBasicConsumer(_channel);
				consumer.ReceivedAsync += ProductUpdatedReceivedAsync;

				await _channel.BasicConsumeAsync(queue: MainQueue, autoAck: false, consumer: consumer);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error initializing listener: {ex.Message}");
			}
		}

		/// <summary>
		/// Stops the listener and closes all connections and channels.
		/// </summary>
		public async Task Stop()
		{
			await CloseAll();
		}

		/// <summary>
		/// Disposes the listener, ensuring all resources are cleaned up.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Disposes the listener, ensuring all resources are cleaned up.
		/// </summary>
		/// <param name="disposing">disposing</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				CloseAll().Wait();
			}
		}

		/// <summary>
		/// Closes and disposes the RabbitMQ channel and connection if open.
		/// </summary>
		private async Task CloseAll()
		{
			if (_channel?.IsOpen == true)
				await _channel.CloseAsync();
			_channel?.Dispose();

			if (_connection?.IsOpen == true)
				await _connection.CloseAsync();
			_connection?.Dispose();
		}

		/// <summary>
		/// Handles incoming product update messages and updates cart items accordingly.
		/// Acknowledges successful messages or negatively acknowledges failed ones to trigger retry.
		/// </summary>
		/// <param name="sender">The message sender.</param>
		/// <param name="event">The event containing the RabbitMQ message.</param>
		private async Task ProductUpdatedReceivedAsync(object sender, BasicDeliverEventArgs @event)
		{
			try
			{
				var json = Encoding.UTF8.GetString(@event.Body.ToArray());

				var message = JsonSerializer.Deserialize<ProductUpdateMessage>(json);

				if (message == null)
				{
					Console.WriteLine("Warning: message is null after deserialization");
					if (_channel != null)
					{
						await _channel.BasicNackAsync(@event.DeliveryTag, multiple: false, requeue: false);
					}
					return;
				}

				using var scope = _scopeFactory.CreateScope();
				var cartService = scope.ServiceProvider.GetRequiredService<ICartService>();

				cartService.UpdateCartItems(message.Id, message.Name, message.Price);

				if (_channel != null)
				{
					await _channel.BasicAckAsync(@event.DeliveryTag, multiple: false);
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error processing message: {ex.Message}");
				if (_channel != null)
				{
					await _channel.BasicNackAsync(@event.DeliveryTag, multiple: false, requeue: false);
				}
			}
		}
	}
}
