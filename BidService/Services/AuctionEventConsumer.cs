using System.Text;
using System.Text.Json;
using BidService.Data;
using BidService.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BidService.Services
{
    public class AuctionEventConsumer : BackgroundService
    {
        private IConnection? _connection;
        private IChannel? _channel;
        private IAsyncBasicConsumer? _consumer;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AuctionEventConsumer> _logger;
        private const string ExchangeName = "auction-events";
        private const string AuctionCreatedQueue = "bid-service-auction-created";
        private const string AuctionCreatedRoutingKey = "auction.created";

        public AuctionEventConsumer(IServiceProvider serviceProvider, ILogger<AuctionEventConsumer> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await InitializeRabbitMQ(stoppingToken);
            _logger.LogInformation("Auction Event Consumer started and listening for messages");
        }

        private async Task InitializeRabbitMQ(CancellationToken stoppingToken)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost",
                    Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672"),
                    DispatchConsumersAsync = true
                };

                _connection = await factory.CreateConnectionAsync(stoppingToken);
                _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

                await _channel.ExchangeDeclareAsync(
                    exchange: ExchangeName,
                    type: ExchangeType.Topic,
                    durable: true,
                    autoDelete: false,
                    cancellationToken: stoppingToken
                );

                var queueDeclareOk = await _channel.QueueDeclareAsync(
                    queue: AuctionCreatedQueue,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    cancellationToken: stoppingToken
                );

                await _channel.QueueBindAsync(
                    queue: AuctionCreatedQueue,
                    exchange: ExchangeName,
                    routingKey: AuctionCreatedRoutingKey,
                    cancellationToken: stoppingToken
                );

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += HandleMessage;

                await _channel.BasicConsumeAsync(
                    queue: AuctionCreatedQueue,
                    autoAck: false,
                    consumerTag: "BidService-Consumer",
                    consumer: consumer,
                    cancellationToken: stoppingToken
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing RabbitMQ connection");
                throw;
            }
        }

        private async Task HandleMessage(object model, BasicDeliverEventArgs ea)
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _logger.LogInformation($"Received message: {message}");

                var auctionMessage = JsonSerializer.Deserialize<AuctionMessage>(message);
                if (auctionMessage == null)
                {
                    _logger.LogWarning("Failed to deserialize auction message");
                    await _channel!.BasicNackAsync(ea.DeliveryTag, false, false);
                    return;
                }

                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<BidDbContext>();
                    _logger.LogInformation($"Processing auction event for AuctionId: {auctionMessage.AuctionId}");
                    // Auction data is now available in the BidService for processing
                    // This can be extended to store auction metadata if needed
                    await dbContext.SaveChangesAsync();
                }

                await _channel!.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message");
                await _channel!.BasicNackAsync(ea.DeliveryTag, false, true);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel != null)
            {
                await _channel.CloseAsync(cancellationToken);
                _channel.Dispose();
            }

            if (_connection != null)
            {
                await _connection.CloseAsync(cancellationToken);
                _connection.Dispose();
            }

            await base.StopAsync(cancellationToken);
        }
    }
}
