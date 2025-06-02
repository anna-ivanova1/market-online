using CartService.Domain.Interfaces;
using CartService.Domain.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace CartService.API.Listeners
{
    public class ProductUpdateListener : IProductUpdateListener, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;

        private IConnection _connection;
        private IChannel _channel;

        private const string MainQueue = "product.update";
        private const string RetryQueue = "product.update.retry";
        private const string MainExchange = "product.exchange";
        private const string RetryExchange = "product.retry.exchange";


        public ProductUpdateListener(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

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

                await _channel.ExchangeDeclareAsync(MainExchange, ExchangeType.Direct, durable: true);
                await _channel.ExchangeDeclareAsync(RetryExchange, ExchangeType.Direct, durable: true);

                await _channel.QueueDeclareAsync(MainQueue, durable: true, exclusive: false, autoDelete: false,
                    arguments: new Dictionary<string, object>
                    {
                    { "x-dead-letter-exchange", RetryExchange }
                    });

                await _channel.QueueBindAsync(MainQueue, MainExchange, routingKey: "");

                await _channel.QueueDeclareAsync(RetryQueue, durable: true, exclusive: false, autoDelete: false,
                    arguments: new Dictionary<string, object>
                    {
                    { "x-dead-letter-exchange", MainExchange },
                    { "x-message-ttl", 10000 }
                    });

                await _channel.QueueBindAsync(RetryQueue, RetryExchange, routingKey: "");

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += ProductUpdatedReceivedAsync;

                await _channel.BasicConsumeAsync(queue: MainQueue, autoAck: false, consumer: consumer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        }

        public void Dispose()
        {
            CloseAll();
        }

        public async Task Stop()
        {
            await CloseAll();
        }

        private async Task CloseAll()
        {
            if (_channel?.IsOpen == true)
                await _channel.CloseAsync();
            _channel?.Dispose();
            if (_connection?.IsOpen == true)
                await _connection.CloseAsync();
            _connection?.Dispose();
        }

        private async Task ProductUpdatedReceivedAsync(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var json = Encoding.UTF8.GetString(@event.Body.ToArray());

                var message = JsonSerializer.Deserialize<ProductUpdateMessage>(json);

                if (message == null)
                {
                    Console.WriteLine("Warning: message is null after deserialization");
                    throw new ArgumentNullException(nameof(message), "Message is null after deserialization");
                }

                using var scope = _scopeFactory.CreateScope();
                var cartService = scope.ServiceProvider.GetRequiredService<ICartService>();

                cartService.UpdateCartItems(message.Id, message.Name, message.Price);

                await _channel.BasicAckAsync(@event.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
                await _channel.BasicNackAsync(@event.DeliveryTag, multiple: false, requeue: false);
            }
        }
    }
}
