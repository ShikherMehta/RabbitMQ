using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.RabbitMq;
using RabbitMQ.Client;
using System.Text;

namespace OrderService.Workers
{
    public class OutboxPublisherWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly RabbitMqConnection _rabbitMq;

        public OutboxPublisherWorker(
            IServiceScopeFactory scopeFactory,
            RabbitMqConnection rabbitMq)
        {
            _scopeFactory = scopeFactory;
            _rabbitMq = rabbitMq;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await PublishMessages(stoppingToken);

                    await Task.Delay(
                        TimeSpan.FromSeconds(2),
                        stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Outbox publisher error: {ex.Message}");

                    await Task.Delay(
                        TimeSpan.FromSeconds(5),
                        stoppingToken);
                }
            }
        }

        private async Task PublishMessages(
            CancellationToken stoppingToken)
        {
            using var scope =
                _scopeFactory.CreateScope();

            var db =
                scope.ServiceProvider
                    .GetRequiredService<AppDbContext>();

            var connection =
                await _rabbitMq.GetConnectionAsync();

            await using var channel =
                await connection.CreateChannelAsync();

            // Configure exchanges, queues and bindings
            await RabbitMqTopology.ConfigureAsync(channel);

            // Get unprocessed Outbox messages
            var messages =
                await db.OutboxMessages
                    .Where(x => x.ProcessedAt == null)
                    .OrderBy(x => x.CreatedAt)
                    .Take(20)
                    .ToListAsync(stoppingToken);

            foreach (var message in messages)
            {
                try
                {
                  
                    var body =
                        Encoding.UTF8.GetBytes(
                            message.Payload);

                    var properties =
                        new BasicProperties
                        {
                            Persistent = true,

                            MessageId =
                                message.Id.ToString(),

                            Type =
                                message.EventType
                        };


                    await channel.BasicPublishAsync(
                        exchange:
                            RabbitMqTopology.PaymentExchange,

                        routingKey:
                            RabbitMqTopology.OrderCreatedRoutingKey,

                        mandatory:
                            true,

                        basicProperties:
                            properties,

                        body:
                            body,

                        cancellationToken:
                            stoppingToken);

                    message.ProcessedAt =
                        DateTime.UtcNow;

                    await db.SaveChangesAsync(
                        stoppingToken);

                    Console.WriteLine(
                        $"Outbox message published successfully: {message.Id}");
                }
                catch (Exception ex)
                {

                    message.RetryCount++;

                    await db.SaveChangesAsync(
                        stoppingToken);

                    Console.WriteLine(
                        $"Publish failed for message " +
                        $"{message.Id}: {ex.Message}");
                }
            }
        }
    }
}
