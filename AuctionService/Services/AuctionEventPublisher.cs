using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace AuctionService.Services
{
    public interface IAuctionEventPublisher
    {
        Task PublishAuctionCreatedAsync(object auctionData);
    }

    public class AuctionEventPublisher : IAuctionEventPublisher
    {
        private IConnection? _connection;
        private IModel? _channel;
        private readonly ILogger<AuctionEventPublisher> _logger;
        private const string ExchangeName = "auction-events";

        public AuctionEventPublisher(ILogger<AuctionEventPublisher> logger)
        {
            _logger = logger;
        }

        private void EnsureConnection()
        {
            if (_channel != null && _channel.IsOpen)
                return;

            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost",
                    Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672")
                };

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(
                    exchange: ExchangeName,
                    type: ExchangeType.Topic,
                    durable: true,
                    autoDelete: false
                );

                _logger.LogInformation("RabbitMQ connection established");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to establish RabbitMQ connection");
                throw;
            }
        }

        public async Task PublishAuctionCreatedAsync(object auctionData)
        {
            await Task.Run(() =>
            {
                try
                {
                    EnsureConnection();

                    var json = JsonSerializer.Serialize(auctionData);
                    var body = Encoding.UTF8.GetBytes(json);

                    _channel!.BasicPublish(
                        exchange: ExchangeName,
                        routingKey: "auction.created",
                        basicProperties: null,
                        body: body
                    );

                    _logger.LogInformation($"Published auction created event: {json}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error publishing auction created event");
                    throw;
                }
            });
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
